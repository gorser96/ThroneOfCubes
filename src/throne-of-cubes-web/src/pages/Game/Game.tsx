import { useParams } from 'react-router-dom';

const Game = () => {
  const { lobbyId } = useParams<{ lobbyId: string }>(); // Получаем ID лобби из URL

  return (
    <div className="game">
      <h2>Игра началась в лобби {lobbyId}</h2>
      {/* Здесь будет основная игровая логика */}
    </div>
  );
};

export default Game;
