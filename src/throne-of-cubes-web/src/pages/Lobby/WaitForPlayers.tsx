import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';

const WaitForPlayers = () => {
  const { lobbyId } = useParams<{ lobbyId: string }>();
  const [players, setPlayers] = useState<string[]>([]); // Список игроков в лобби
  const navigate = useNavigate();

  useEffect(() => {
    // Логика получения данных о подключенных игроках
    const interval = setInterval(() => {
      // Пример получения списка игроков
      setPlayers(['Player1', 'Player2']); // Это пример
    }, 1000);

    return () => clearInterval(interval); // Очистка интервала
  }, [lobbyId]);

  const handleStartGame = () => {
    // Логика старта игры
    console.log('Игра началась');
    navigate(`/game/${lobbyId}`);
  };

  return (
    <div className="wait-for-players">
      <h2>Лобби {lobbyId}</h2>
      <p>Ожидание игроков...</p>
      <ul>
        {players.map((player) => (
          <li key={player}>{player}</li>
        ))}
      </ul>
      {players.length >= 2 && (
        <button onClick={handleStartGame}>Начать игру</button>
      )}
    </div>
  );
};

export default WaitForPlayers;
