import { User } from '../../contexts/AuthContext';

export interface RegisterData {
  username: string;
  password: string;
}

export interface LoginData {
  username: string;
  password: string;
}

export const registerUser = async (data: RegisterData) => {
  const response = await fetch('/api/register', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(data),
  });

  if (!response.ok) {
    throw new Error('Ошибка при регистрации, попробуйте позже');
  }

  return response.json();
};

export const loginUser = async (data: LoginData): Promise<User> => {
  return new Promise((resolve) => resolve({ id: 1, username: 'test' }));
  const response = await fetch('/api/login', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(data),
  });

  if (!response.ok) {
    throw new Error(
      'Ошибка при авторизации, проверьте имя пользователя и пароль'
    );
  }

  return response.json();
};
