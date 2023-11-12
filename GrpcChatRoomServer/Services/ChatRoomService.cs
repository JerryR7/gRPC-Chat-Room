using System.Collections.Concurrent;
using Grpc.Core;
using GrpcChatRoom;

namespace GrpcChatRoomServer.Services;

public class ChatRoomService : ChatRoom.ChatRoomBase
{
    private readonly ILogger<ChatRoomService> _logger;

    public ChatRoomService(ILogger<ChatRoomService> logger)
    {
        _logger = logger;
    }

    static ConcurrentDictionary<string, MsgChannel> channels = new();

    private class MsgChannel
    {
        public string Name;
        public IServerStreamWriter<BroadcastMessage> Stream;
    }

    async Task Message(string speaker, string message)
    {
        var msg = new BroadcastMessage
        {
            Speaker = speaker,
            Time = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow),
            Message = message
        };
        var lostUsers = new List<string>();
        foreach (var kv in channels.ToArray())
        {
            try
            {
                await kv.Value.Stream.WriteAsync(msg);
            }
            catch
            {
                lostUsers.Add(kv.Value.Name);
                channels.TryRemove(kv.Key, out _);
            }
        }

        if (lostUsers.Any())
        {
            await Message("system", string.Join(", ", lostUsers.ToArray()) + " disconnected");
        }
    }

    public override async Task Join(IAsyncStreamReader<MessageRequest> requestStream,
        IServerStreamWriter<BroadcastMessage> responseStream, ServerCallContext context)
    {
        var clientIp = context.GetHttpContext().Connection.RemoteIpAddress!.ToString();
        _logger.LogInformation($"{clientIp}/{context.Peer} connected");
        var uid = string.Empty;
        var name = string.Empty;
        try
        {
            await foreach (var messageReq in requestStream.ReadAllAsync(context.CancellationToken))
            {
                uid = messageReq.UserId;
                name = messageReq.Name;
                var speaker = $"{name}@{clientIp}";
                var newMember = false;
                if (!channels.TryGetValue(uid, out var channel))
                {
                    _logger.LogInformation($"{uid}/{speaker}");
                    channel = new MsgChannel { Name = name, Stream = responseStream };
                    if (!channels.TryAdd(uid, channel))
                        throw new ApplicationException("Failed to join the chatroom");
                    newMember = true;
                }

                channel.Name = name;
                if (messageReq.Message == "/exit")
                    break;
                else if (newMember)
                    await Message("system", $"{name} joined");
                else
                    await Message(speaker, messageReq.Message);
            }
        }
        catch (Exception ex)
        {
            await Message("system", $"{name} {ex.Message}");
        }

        //_logger.LogInformation($"{context.Peer} disconnected");
        await Message($"system", $"{name} left");
        if (!string.IsNullOrEmpty(uid)) channels.TryRemove(uid, out _);
    }
}