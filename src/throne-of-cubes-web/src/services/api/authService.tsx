import { User } from '../../contexts/AuthContext';

export interface RegisterData {
  username: string;
  password: string;
}

export interface LoginData {
  username: string;
  password: string;
}

// Utility function to save and retrieve tokens
const TOKEN_KEY = 'authToken';

export const saveToken = (token: string) => {
  localStorage.setItem(TOKEN_KEY, token);
};

export const getToken = () => {
  return localStorage.getItem(TOKEN_KEY);
};

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
  const response = await fetch('/api/login', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(data),
  });

  if (!response.ok) {
    if (response.status === 401) {
      throw new Error('Неверное имя пользователя или пароль');
    } else {
      throw new Error('Ошибка при авторизации, попробуйте позже');
    }
  }

  const result = await response.json();

  // Save token to localStorage
  if (result.token) {
    saveToken(result.token);
  }

  return {
    id: result.userId,
    username: result.username,
  };
};

// Example of adding token to future requests
export const fetchWithAuth = async (url: string, options: RequestInit = {}) => {
  const token = getToken();

  if (token) {
    options.headers = {
      ...options.headers,
      Authorization: `Bearer ${token}`,
    };
  }

  const response = await fetch(url, options);

  if (!response.ok) {
    throw new Error('Ошибка при выполнении запроса');
  }

  return response.json();
};
