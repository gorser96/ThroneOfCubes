import { useNavigate } from 'react-router-dom';
import { AuthContext } from '../../contexts/AuthContext';
import './MainMenu.css'; // Импортируем CSS файл
import { useContext } from 'react';

const MainMenu = () => {
  const navigate = useNavigate();
  const { logout, user } = useContext(AuthContext);

  const handleCreateLobby = () => {
    // Логика для создания лобби
    console.log('Создать лобби');
    navigate('/lobby/create'); // Перенаправление на страницу создания лобби
  };

  const handleJoinLobby = () => {
    // Логика для поиска и присоединения к существующему лобби
    console.log('Присоединиться к лобби');
    navigate('/lobby/join'); // Перенаправление на страницу списка лобби
  };

  const handleSettings = () => {
    navigate('/settings');
  };

  const handleExit = () => {
    logout();
    navigate('/login');
  };

  return (
    <div className="main-menu">
      <h1>ThroneOfCubes</h1>
      {user && <p>Добро пожаловать, {user.username}!</p>}
      <nav>
        <button onClick={handleCreateLobby}>Создать лобби</button>
        <button onClick={handleJoinLobby}>Присоединиться к лобби</button>
        <button onClick={handleSettings}>Настройки</button>
        <button onClick={handleExit}>Выйти</button>
      </nav>
    </div>
  );
};

export default MainMenu;
