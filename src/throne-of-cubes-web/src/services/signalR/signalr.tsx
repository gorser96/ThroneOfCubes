import * as signalR from '@microsoft/signalr';

const connection = new signalR.HubConnectionBuilder()
  .withUrl("https://localhost:5001/gameHub")
  .build();

connection.start().then(() => {
  console.log("Connected to SignalR");
  
  // Присоединяемся к лобби
  connection.invoke("JoinLobby", "lobby123").catch(err => console.error(err));
  
  // Получаем сообщение из лобби
  connection.on("ReceiveMessage", (message) => {
    console.log("New message:", message);
  });

  // Обрабатываем события присоединения и выхода игроков
  connection.on("PlayerJoined", (playerId) => {
    console.log("Player joined:", playerId);
  });

  connection.on("PlayerLeft", (playerId) => {
    console.log("Player left:", playerId);
  });

  // Отправляем сообщение в лобби
  connection.invoke("SendMessageToLobby", "lobby123", "Hello, players!")
    .catch(err => console.error(err));
}).catch(err => console.error("Connection error:", err));

export default connection;