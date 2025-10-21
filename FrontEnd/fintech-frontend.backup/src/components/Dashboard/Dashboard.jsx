import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { getBalance, getTransactionHistory } from '../../services/api';
import './Dashboard.css';

export default function Dashboard() {
  const [balance, setBalance] = useState(0);
  const [transactions, setTransactions] = useState([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();
  const token = localStorage.getItem('token');
  const user = JSON.parse(localStorage.getItem('user') || '{}');

  useEffect(() => {
    if (!token) {
      console.warn('⚠️ Token não encontrado, redirecionando para login');
      navigate('/login');
      return;
    }

    const fetchData = async () => {
      try {
        console.log('📊 Carregando dados do dashboard...');

        const balanceResponse = await getBalance(token);
        if (balanceResponse.success) {
          console.log('✅ Saldo carregado:', balanceResponse.data);
          setBalance(balanceResponse.data.balance || 0);
        } else {
          console.warn('⚠️ Erro ao carregar saldo:', balanceResponse.message);
        }

        const historyResponse = await getTransactionHistory(token);
        if (historyResponse.success) {
          console.log('✅ Histórico carregado:', historyResponse.data);
          setTransactions(historyResponse.data || []);
        } else {
          console.warn('⚠️ Erro ao carregar histórico:', historyResponse.message);
        }
      } catch (err) {
        console.error('❌ Erro ao carregar dados:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [token, navigate]);

  const handleLogout = () => {
    console.log('🚪 Fazendo logout...');
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('user');
    localStorage.removeItem('expiresIn');
    navigate('/login');
  };

  if (loading) {
    return <div className="dashboard-container"><p>Carregando...</p></div>;
  }

  return (
    <div className="dashboard-container">
      <header className="dashboard-header">
        <h1>FinTech Banking</h1>
        <div className="user-info">
          <span>{user.email}</span>
          <button onClick={handleLogout} className="logout-btn">Sair</button>
        </div>
      </header>

      <main className="dashboard-main">
        <section className="balance-section">
          <h2>Saldo</h2>
          <div className="balance-card">
            <p className="balance-label">Saldo Disponível</p>
            <p className="balance-amount">R$ {balance.toFixed(2)}</p>
          </div>
        </section>

        <section className="actions-section">
          <h2>Ações</h2>
          <div className="actions-grid">
            <button className="action-btn" onClick={() => navigate('/pix')}>
              <span className="action-icon">📱</span>
              <span>PIX QR Code</span>
            </button>
            <button className="action-btn" onClick={() => navigate('/withdrawal')}>
              <span className="action-icon">💰</span>
              <span>Saque</span>
            </button>
            <button className="action-btn" onClick={() => navigate('/history')}>
              <span className="action-icon">📋</span>
              <span>Histórico</span>
            </button>
          </div>
        </section>

        <section className="transactions-section">
          <h2>Últimas Transações</h2>
          <div className="transactions-list">
            {transactions.length === 0 ? (
              <p>Nenhuma transação</p>
            ) : (
              transactions.slice(0, 5).map(tx => (
                <div key={tx.id} className="transaction-item">
                  <div className="tx-info">
                    <p className="tx-type">{tx.transactionType}</p>
                    <p className="tx-date">{new Date(tx.createdAt).toLocaleDateString()}</p>
                  </div>
                  <div className="tx-amount">
                    <p className={`amount ${tx.transactionType === 'WITHDRAWAL' ? 'negative' : 'positive'}`}>
                      {tx.transactionType === 'WITHDRAWAL' ? '-' : '+'}R$ {tx.amount.toFixed(2)}
                    </p>
                    <p className={`status ${tx.status.toLowerCase()}`}>{tx.status}</p>
                  </div>
                </div>
              ))
            )}
          </div>
        </section>
      </main>
    </div>
  );
}

