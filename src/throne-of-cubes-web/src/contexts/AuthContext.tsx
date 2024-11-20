import { createContext, ReactNode, useEffect, useState } from 'react';
import { getToken, loginUser } from '../services/api/authService';

export interface User {
  id: number;
  username: string;
}

interface AuthContextProps {
  user: User | null;
  isAuthenticated: boolean;
  login: (username: string, password: string) => Promise<void>;
  logout: () => void;
}

export const AuthContext = createContext<AuthContextProps>({
  user: null,
  isAuthenticated: false,
  login: async () => {},
  logout: () => {},
});

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [user, setUser] = useState<User | null>(null);

  useEffect(() => {
    // Проверка токена при загрузке
    const token = getToken();
    if (token) {
      // Для реальной проверки можно использовать запрос на сервер
      setUser({ id: 1, username: 'DemoUser' }); // Заменить на реальную логику
    }
  }, []);

  const login = async (username: string, password: string) => {
    const user = await loginUser({ username, password });
    setUser(user);
  };

  const logout = () => {
    localStorage.removeItem('authToken');
    setUser(null);
  };

  return (
    <AuthContext.Provider
      value={{ user, isAuthenticated: !!user, login, logout }}
    >
      {children}
    </AuthContext.Provider>
  );
};
