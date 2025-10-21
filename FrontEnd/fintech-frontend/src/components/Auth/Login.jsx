import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { login } from '../../services/api';
import './Auth.css';

export default function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');

    try {
      const response = await login(email, password);

      if (response.success) {
        // Armazenar token e dados do usuário
        localStorage.setItem('token', response.data.token);
        localStorage.setItem('refreshToken', response.data.refreshToken);
        localStorage.setItem('user', JSON.stringify(response.data.user));
        localStorage.setItem('expiresIn', response.data.expiresIn);

        console.log('✅ Login realizado com sucesso!');
        console.log('Token:', response.data.token.substring(0, 50) + '...');

        // Redirecionar para dashboard
        navigate('/dashboard');
      } else {
        setError(response.message || 'Erro ao fazer login');
        console.error('❌ Erro no login:', response.message);
      }
    } catch (err) {
      setError('Erro ao conectar com o servidor');
      console.error('❌ Erro de conexão:', err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-container">
      <div className="auth-card">
        <h1>FinTech Banking</h1>
        <h2>Login</h2>
        
        {error && <div className="error-message">{error}</div>}
        
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label>Email</label>
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </div>

          <div className="form-group">
            <label>Senha</label>
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>

          <button type="submit" disabled={loading}>
            {loading ? 'Entrando...' : 'Entrar'}
          </button>
        </form>

        <p className="auth-link">
          Não tem conta? <a href="/register">Registre-se</a>
        </p>
      </div>
    </div>
  );
}

