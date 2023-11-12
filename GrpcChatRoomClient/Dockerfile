FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["GrpcChatRoomClient/GrpcChatRoomClient.csproj", "GrpcChatRoomClient/"]
RUN dotnet restore "GrpcChatRoomClient/GrpcChatRoomClient.csproj"
COPY . .
WORKDIR "/src/GrpcChatRoomClient"
RUN dotnet build "GrpcChatRoomClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GrpcChatRoomClient.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GrpcChatRoomClient.dll"]
