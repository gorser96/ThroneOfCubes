import './App.css';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Login from './pages/Auth/Login';
import Register from './pages/Auth/Register';
import MainMenu from './pages/MainMenu/MainMenu';
import CreateLobby from './pages/Lobby/CreateLobby';
import JoinLobby from './pages/Lobby/JoinLobby';
import WaitForPlayers from './pages/Lobby/WaitForPlayers';
import Game from './pages/Game/Game';
import Settings from './pages/Settings/Settings';
import ProtectedRoute from './components/common/ProtectedRoute';
import { AuthProvider } from './contexts/AuthContext';

function App() {
  return (
    <AuthProvider>
      <Router>
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route
            path="/lobby/create"
            element={
              <ProtectedRoute>
                <CreateLobby />
              </ProtectedRoute>
            }
          />
          <Route
            path="/lobby/join"
            element={
              <ProtectedRoute>
                <JoinLobby />
              </ProtectedRoute>
            }
          />
          <Route
            path="/lobby/:lobbyId/wait"
            element={
              <ProtectedRoute>
                <WaitForPlayers />
              </ProtectedRoute>
            }
          />
          <Route
            path="/game/:lobbyId"
            element={
              <ProtectedRoute>
                <Game />
              </ProtectedRoute>
            }
          />
          <Route
            path="/settings"
            element={
              <ProtectedRoute>
                <Settings />
              </ProtectedRoute>
            }
          />
          <Route
            path="/"
            element={
              <ProtectedRoute>
                <MainMenu />
              </ProtectedRoute>
            }
          />
        </Routes>
      </Router>
    </AuthProvider>
  );
}

export default App;
