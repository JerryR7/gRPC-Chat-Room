// using GrpcChatRoom;
// using GrpcChatRoomServer.Services;
// using NUnit.Framework;
// using NUnit.Framework.Internal;
//
// namespace GrpcChatRoomServer.Tests;
//
// public class WalkingSkeletonTest
// {
//     private const string ServerAddress = "localhost:50051"; // 假設 gRPC 服務器運行在本地，監聽 50051 端口
//     private ChatRoomService _server;
//     
//     [SetUp]
//     public void Setup()
//     {
//         _server = new ChatRoomService(new Logger());
//     }
//     
//     [TearDown]
//     public void TearDown()
//     {
//         _client = null;
//     }
//
//     [Test]
//     public void Test1()
//     {
//         Assert.Pass();
//     }
//
//     [Test]
//     public async Task TestChatRoomInteraction()
//     {
//         // 用戶1加入聊天室
//         var joinRequest1 = new JoinChatRequest { UserId = "user1", ChatRoomId = "room1" };
//         var response1 = await _client.JoinChatAsync(joinRequest1);
//         Assert.True(response1.Success);
//
//         // 用戶2加入同一聊天室
//         var joinRequest2 = new JoinChatRequest { UserId = "user2", ChatRoomId = "room1" };
//         var response2 = await _client.JoinChatAsync(joinRequest2);
//         Assert.True(response2.Success);
//
//         // 用戶1發送消息
//         var message1 = new SendMessageRequest { Content = "Hello from user1", UserId = "user1", ChatRoomId = "room1" };
//         var responseMessage1 = await _client.SendMessageAsync(message1);
//
//         // 用戶2接收消息
//         var message2 = await _client.SendMessageAsync(message1);
//         Assert.AreEqual("Hello from user1", message2.Content);
//
//         // 用戶1標記消息為已讀
//         var readRequest = new MarkMessageAsReadRequest { UserId = "user1", MessageId = responseMessage1.MessageId };
//         var responseRead = await _client.MarkMessageAsReadAsync(readRequest);
//         Assert.True(responseRead.Success);
//
//         // 用戶2檢查消息是否已讀
//         var checkReadRequest = new CheckMessageReadRequest { MessageId = responseMessage1.MessageId };
//         var responseCheckRead = await _client.CheckMessageReadAsync(checkReadRequest);
//         Assert.True(responseCheckRead.IsRead);
//     }
// }