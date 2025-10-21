import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { register } from '../../services/api';
import './Auth.css';

export default function Register() {
  const [formData, setFormData] = useState({
    email: '',
    password: '',
    fullName: '',
    document: '',
    phoneNumber: ''
  });
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');

    try {
      const response = await register(
        formData.email,
        formData.password,
        formData.fullName,
        formData.document,
        formData.phoneNumber
      );

      if (response.success) {
        console.log('✅ Registro realizado com sucesso!');
        console.log('Usuário criado:', response.data);

        // Redirecionar para login após sucesso
        setTimeout(() => {
          navigate('/login');
        }, 1500);
      } else {
        setError(response.message || 'Erro ao registrar');
        console.error('❌ Erro no registro:', response.message);
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
        <h2>Registrar</h2>
        
        {error && <div className="error-message">{error}</div>}
        
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label>Nome Completo</label>
            <input
              type="text"
              name="fullName"
              value={formData.fullName}
              onChange={handleChange}
              required
            />
          </div>

          <div className="form-group">
            <label>Email</label>
            <input
              type="email"
              name="email"
              value={formData.email}
              onChange={handleChange}
              required
            />
          </div>

          <div className="form-group">
            <label>CPF</label>
            <input
              type="text"
              name="document"
              value={formData.document}
              onChange={handleChange}
              placeholder="12345678901"
              required
            />
          </div>

          <div className="form-group">
            <label>Telefone</label>
            <input
              type="tel"
              name="phoneNumber"
              value={formData.phoneNumber}
              onChange={handleChange}
              placeholder="11999999999"
              required
            />
          </div>

          <div className="form-group">
            <label>Senha</label>
            <input
              type="password"
              name="password"
              value={formData.password}
              onChange={handleChange}
              required
            />
          </div>

          <button type="submit" disabled={loading}>
            {loading ? 'Registrando...' : 'Registrar'}
          </button>
        </form>

        <p className="auth-link">
          Já tem conta? <a href="/login">Faça login</a>
        </p>
      </div>
    </div>
  );
}

