import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './Settings.css';

const Settings = () => {
  const [volume, setVolume] = useState(50); // Пример настройки звука
  const navigate = useNavigate();

  const handleVolumeChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setVolume(Number(e.target.value));
  };

  const handleGoBack = () => {
    navigate('/');  // Перенаправляем на главную страницу (MainMenu)
  };

  return (
    <div className="settings">
      <h2>Настройки</h2>
      <div>
        <label>Громкость</label>
        <input
          type="range"
          min="0"
          max="100"
          value={volume}
          onChange={handleVolumeChange}
        />
        <span>{volume}</span>
      </div>
      {/* Добавь другие настройки (графика, управление и т.д.) */}
      <button onClick={handleGoBack}>Вернуться в главное меню</button>
    </div>
  );
};

export default Settings;
