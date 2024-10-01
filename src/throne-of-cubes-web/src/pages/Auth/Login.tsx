import React, { useState } from 'react';
import { loginUser, LoginData } from '../../services/api/authService';
import { useAuth } from '../../contexts/AuthContext';
import { useNavigate } from 'react-router-dom';

const Login = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [errors, setErrors] = useState<string[]>([]);
  const [isLoading, setIsLoading] = useState(false); // Добавляем состояние для загрузки
  const { login } = useAuth();
  const navigate = useNavigate();

  const validateForm = () => {
    const errors = [];
    if (!username) {
      errors.push('Введите имя пользователя');
    }
    if (!password) {
      errors.push('Введите пароль');
    }
    return errors;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const validationErrors = validateForm();
    if (validationErrors.length > 0) {
      setErrors(validationErrors);
      return;
    }

    const loginData: LoginData = { username, password };

    try {
      setIsLoading(true); // Устанавливаем загрузку в true
      const userData = await loginUser(loginData);
      login(userData); // Авторизация
      setIsLoading(false);
      navigate('/'); // Перенаправление на главное меню
    } catch (error: any) {
      setErrors([error.message]);
    } finally {
      setIsLoading(false); // Устанавливаем загрузку в false после завершения запроса
    }
  };

  return (
    <div className="login">
      <h2>Авторизация</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Имя пользователя</label>
          <input
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            disabled={isLoading} // Отключаем ввод во время загрузки
          />
        </div>
        <div>
          <label>Пароль</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            disabled={isLoading} // Отключаем ввод во время загрузки
          />
        </div>
        {errors.length > 0 && (
          <div className="errors">
            {errors.map((error, index) => (
              <p key={index} style={{ color: 'red' }}>
                {error}
              </p>
            ))}
          </div>
        )}
        <button type="submit" disabled={isLoading}>
          {isLoading ? 'Вход...' : 'Войти'}{' '}
          {/* Меняем текст кнопки во время загрузки */}
        </button>
      </form>
      {isLoading && <p>Загрузка...</p>} {/* Показываем индикатор загрузки */}
      <p>
        Нет учетной записи? <a href="/register">Зарегистрироваться</a>
      </p>
    </div>
  );
};

export default Login;
