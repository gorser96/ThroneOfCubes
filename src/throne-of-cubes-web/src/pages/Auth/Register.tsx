import React, { useState } from 'react';
import { registerUser, RegisterData } from '../../services/api/authService';

const Register = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [errors, setErrors] = useState<string[]>([]);

  const validateForm = () => {
    const errors = [];
    if (!username || username.length < 3) {
      errors.push('Имя пользователя должно содержать минимум 3 символа');
    }
    if (password.length < 8) {
      errors.push('Пароль должен быть не менее 8 символов');
    }
    if (password !== confirmPassword) {
      errors.push('Пароли не совпадают');
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

    const registerData: RegisterData = { username, password };

    try {
      await registerUser(registerData);
      // Здесь можно добавить логику, например, перенаправление на страницу авторизации
      console.log('Успешная регистрация');
    } catch (error: any) {
      setErrors([error.message]);
    }
  };

  return (
    <div className="register">
      <h2>Регистрация</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label>Имя пользователя</label>
          <input
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          />
        </div>
        <div>
          <label>Пароль</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </div>
        <div>
          <label>Подтвердите пароль</label>
          <input
            type="password"
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
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
        <button type="submit">Зарегистрироваться</button>
      </form>
      <p>
        Уже зарегистрированы? <a href="/login">Войти</a>
      </p>
    </div>
  );
};

export default Register;
