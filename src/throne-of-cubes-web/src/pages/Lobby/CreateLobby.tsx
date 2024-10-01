import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const CreateLobby = () => {
  const [lobbyId, setLobbyId] = useState('');
  const [maxPlayers, setMaxPlayers] = useState(10); // Количество игроков в лобби
  const navigate = useNavigate();

  const handleCreateLobby = () => {
    // Логика создания лобби, генерация lobbyId и т.д.
    const generatedLobbyId = 'abc123'; // Это пример, на практике будет запрос на сервер
    setLobbyId(generatedLobbyId);
    console.log(`Лобби создано с ID: ${generatedLobbyId}`);
    navigate(`/lobby/${generatedLobbyId}/wait`);
  };

  return (
    <div className="create-lobby">
      <h2>Создание лобби</h2>
      <label>
        Количество игроков (2-10):
        <input
          type="number"
          min="2"
          max="10"
          value={maxPlayers}
          onChange={(e) => setMaxPlayers(Number(e.target.value))}
        />
      </label>
      <button onClick={handleCreateLobby}>Создать лобби</button>
    </div>
  );
};

export default CreateLobby;
