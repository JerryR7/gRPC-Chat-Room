<p align="center">
  <img src="https://github.com/dotnet/vscode-csharp/blob/main/images/csharpIcon.png" alt="Telegram Bot Logo">
</p>

<h1 align="center">.NET 7.0 gRPC Chat Room</h1>

<p align="center">
  <img src="https://img.shields.io/badge/version-1.0.0-brightgreen" alt="Version 1.0.0">
  <img src="https://img.shields.io/badge/.NET-7.0-blue" alt=".NET Core 3.1">
</p>

<div align="center">
GrpcChatRoom is a simple, yet powerful chat room application built using .NET 7.0 and gRPC. It leverages the bi-directional streaming feature of gRPC to enable real-time communication between users.
</div>

## Features

- **Chat Room Creation**: Users can create new chat rooms with unique names.
- **Join Chat Room**: Users can join an existing chat room using their username.
- **Real-time Messaging**: Once in a chat room, users can send and receive messages in real-time.
- **Broadcast Messages**: Messages are broadcasted to all members of the chat room.

## Prerequisites

- [.NET 7.0 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- [gRPC](https://grpc.io/docs/languages/csharp/quickstart/)

## How to Run

1. **Clone the Repository**

    ```bash
    git clone https://github.com/JerryR7/gRPC-Chat-Room.git
    ```

2. **Install Dependencies**

   Navigate to the solution's root directory and run:

    ```bash
    dotnet restore
    ```

3. **Start the Server**

    ```bash
    dotnet run --project GrpcChatRoomServer
    ```

4. **Join the Chat Room (Client)**

   Open another terminal and execute:

    ```bash
    dotnet GrpcChatRoomClient.dll http://localhost:5040 userName
    ```

   Replace `userName` with your desired username.

## Project Structure

- **.proto File**: Defines the gRPC service and message contracts.
- **Server**: Hosts the gRPC service that manages chat rooms and message broadcasting.
- **Client**: Connects to the server, joins chat rooms, and handles sending/receiving messages.

## Versioning

This project uses [Semantic Versioning](https://semver.org/). The current version is 1.0.0. For the versions available, see the tags on this repository.

## Contributing

Contributions are welcome! Feel free to submit pull requests, report bugs, or request new features. Please read `CONTRIBUTING.md` for guidelines on contributing.

## License

This project is open-source and available under the MIT License. See the [LICENSE](LICENSE) file for details.

---
