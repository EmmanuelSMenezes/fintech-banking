'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import Link from 'next/link';

interface User {
  id: string;
  email: string;
  fullName: string;
}

interface Balance {
  accountId: string;
  balance: number;
  currency: string;
}

export default function DashboardPage() {
  const router = useRouter();
  const [user, setUser] = useState<User | null>(null);
  const [balance, setBalance] = useState<Balance | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const token = localStorage.getItem('token');
    const userData = localStorage.getItem('user');

    if (!token || !userData) {
      router.push('/signin');
      return;
    }

    setUser(JSON.parse(userData));
    fetchBalance(token);
  }, [router]);

  const fetchBalance = async (token: string) => {
    try {
      const apiUrl = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5167';
      const response = await fetch(`${apiUrl}/api/transactions/balance`, {
        headers: { Authorization: `Bearer ${token}` },
      });

      if (!response.ok) throw new Error('Erro ao buscar saldo');
      const data = await response.json();
      setBalance(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Erro ao carregar saldo');
    } finally {
      setLoading(false);
    }
  };

  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    router.push('/signin');
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto mb-4"></div>
          <p className="text-gray-600">Carregando...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <header className="bg-white shadow">
        <div className="max-w-7xl mx-auto px-4 py-4 flex justify-between items-center">
          <h1 className="text-2xl font-bold text-blue-600">FinTech Banking</h1>
          <div className="flex items-center gap-4">
            <span className="text-gray-700">{user?.fullName}</span>
            <button
              onClick={handleLogout}
              className="bg-red-600 hover:bg-red-700 text-white px-4 py-2 rounded-lg transition"
            >
              Sair
            </button>
          </div>
        </div>
      </header>

      {/* Main Content */}
      <main className="max-w-7xl mx-auto px-4 py-8">
        {error && (
          <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg mb-6">
            {error}
          </div>
        )}

        {/* Balance Card */}
        <div className="bg-gradient-to-r from-blue-600 to-blue-800 text-white rounded-lg shadow-lg p-8 mb-8">
          <p className="text-blue-100 mb-2">Saldo Disponível</p>
          <h2 className="text-4xl font-bold">
            {balance ? `R$ ${balance.balance.toFixed(2)}` : 'R$ 0,00'}
          </h2>
        </div>

        {/* Quick Actions */}
        <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
          <Link href="/pix-qrcode">
            <div className="bg-white rounded-lg shadow p-6 hover:shadow-lg transition cursor-pointer">
              <div className="text-3xl mb-2">📱</div>
              <h3 className="font-semibold text-gray-800">PIX QR Code</h3>
              <p className="text-gray-600 text-sm">Gerar código para receber</p>
            </div>
          </Link>

          <Link href="/withdrawal">
            <div className="bg-white rounded-lg shadow p-6 hover:shadow-lg transition cursor-pointer">
              <div className="text-3xl mb-2">💰</div>
              <h3 className="font-semibold text-gray-800">Saque</h3>
              <p className="text-gray-600 text-sm">Solicitar saque</p>
            </div>
          </Link>

          <Link href="/history">
            <div className="bg-white rounded-lg shadow p-6 hover:shadow-lg transition cursor-pointer">
              <div className="text-3xl mb-2">📊</div>
              <h3 className="font-semibold text-gray-800">Histórico</h3>
              <p className="text-gray-600 text-sm">Ver transações</p>
            </div>
          </Link>
        </div>

        {/* Recent Transactions */}
        <div className="bg-white rounded-lg shadow p-6">
          <h3 className="text-lg font-semibold text-gray-800 mb-4">Transações Recentes</h3>
          <p className="text-gray-600">Nenhuma transação ainda</p>
        </div>
      </main>
    </div>
  );
}

