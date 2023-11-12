using Grpc.Core;
using Grpc.Net.Client;
using GrpcChatRoom;

if (args.Length < 2)
{
    Console.WriteLine("Syntax: grpc-chat-client http://localhost:5040 userName");
    return;
}

var url = args[0];
var name = args[1];
Console.Clear();
using var channel = GrpcChannel.ForAddress(url);
var client = new ChatRoom.ChatRoomClient(channel);

// var createChatRoomResponse = await client.CreateAsync(new CreateChatRoomRequest { ChatRoomName = "Room1" });
// var chatRoomId = createChatRoomResponse.ChatRoomId;

var duplex = client.Join();
var uid = Guid.NewGuid().ToString();
var spkMsg = new MessageRequest
{
    UserId = uid,
    Name = name,
    Message = "/join"
};
await duplex.RequestStream.WriteAsync(spkMsg);
int x = 0, y = 2;
void Print(string msg, ConsoleColor color = ConsoleColor.White) {
    Console.ForegroundColor = color;
    int origX = Console.CursorLeft, origY = Console.CursorTop;
    Console.SetCursorPosition(x, y);
    Console.WriteLine(msg);
    x = Console.CursorLeft;
    y = Console.CursorTop;
    Console.SetCursorPosition(origX, origY);
    Console.ResetColor();
};
var rcvTask = Task.Run(async () =>
{
    try
    {
        await foreach (var resp in duplex.ResponseStream.ReadAllAsync(CancellationToken.None))
        {
            Print($"{resp.Time.ToDateTime().ToLocalTime():HH:mm:ss} [{resp.Speaker}] {resp.Message}", 
                resp.Speaker == "system" ? ConsoleColor.Yellow : ConsoleColor.Cyan);
        }
    }
    catch (RpcException ex) when (ex.StatusCode == StatusCode.Unavailable)
    {
        Print($"Connection broken", ConsoleColor.Magenta);
        Console.Clear();
        Environment.Exit(254);
    }
    catch (RpcException ex) {
        Print($"Error {ex.InnerException?.Message}", ConsoleColor.Magenta);
        Console.Clear();
        Environment.Exit(255);
    }
});


while (true)
{
    Console.SetCursorPosition(0, 0);
    Console.WriteLine(new String(' ', Console.WindowWidth) + new string('-', Console.WindowWidth));
    Console.SetCursorPosition(0, 0);
    var msg = Console.ReadLine();
    try
    {
        spkMsg.Message = msg;
        await duplex.RequestStream.WriteAsync(spkMsg);
        if (msg == "/exit") break;
    }
    catch (RpcException)
    {
        break;
    }
}
try { await duplex.RequestStream.CompleteAsync(); } catch { }
rcvTask.Wait();
Console.Clear();
