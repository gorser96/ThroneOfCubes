import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const JoinLobby = () => {
  const [lobbyCode, setLobbyCode] = useState('');
  const navigate = useNavigate();

  const handleJoinLobby = () => {
    // Логика присоединения к лобби
    console.log(`Присоединение к лобби с ID: ${lobbyCode}`);
    navigate(`/lobby/${lobbyCode}/wait`);
  };

  return (
    <div className="join-lobby">
      <h2>Присоединиться к лобби</h2>
      <input
        type="text"
        placeholder="Введите код лобби"
        value={lobbyCode}
        onChange={(e) => setLobbyCode(e.target.value)}
      />
      <button onClick={handleJoinLobby}>Присоединиться</button>
    </div>
  );
};

export default JoinLobby;
