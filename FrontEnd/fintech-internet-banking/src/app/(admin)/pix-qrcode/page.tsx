'use client';

import { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import Link from 'next/link';

interface QRCodeData {
  qrCode: string;
  pixKey: string;
  amount?: number;
}

export default function PixQRCodePage() {
  const router = useRouter();
  const [amount, setAmount] = useState('');
  const [qrCode, setQrCode] = useState<QRCodeData | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [copied, setCopied] = useState(false);

  useEffect(() => {
    const token = localStorage.getItem('token');
    if (!token) {
      router.push('/signin');
    }
  }, [router]);

  const generateQRCode = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError('');

    try {
      const token = localStorage.getItem('token');
      if (!token) throw new Error('Token não encontrado');

      const apiUrl = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5167';
      const response = await fetch(`${apiUrl}/api/transactions/pix/qrcode`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({ amount: parseFloat(amount) || 0 }),
      });

      if (!response.ok) throw new Error('Erro ao gerar QR Code');
      const data = await response.json();
      setQrCode(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Erro ao gerar QR Code');
    } finally {
      setLoading(false);
    }
  };

  const copyToClipboard = (text: string) => {
    navigator.clipboard.writeText(text);
    setCopied(true);
    setTimeout(() => setCopied(false), 2000);
  };

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <header className="bg-white shadow">
        <div className="max-w-7xl mx-auto px-4 py-4 flex justify-between items-center">
          <Link href="/dashboard" className="text-2xl font-bold text-blue-600">
            ← FinTech Banking
          </Link>
        </div>
      </header>

      {/* Main Content */}
      <main className="max-w-2xl mx-auto px-4 py-8">
        <div className="bg-white rounded-lg shadow p-8">
          <h1 className="text-3xl font-bold text-gray-800 mb-2">PIX QR Code</h1>
          <p className="text-gray-600 mb-6">Gere um código QR para receber pagamentos via PIX</p>

          {error && (
            <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg mb-6">
              {error}
            </div>
          )}

          <form onSubmit={generateQRCode} className="space-y-4 mb-8">
            <div>
              <label className="block text-gray-700 font-semibold mb-2">Valor (opcional)</label>
              <div className="flex gap-2">
                <div className="flex-1 relative">
                  <span className="absolute left-3 top-3 text-gray-600">R$</span>
                  <input
                    type="number"
                    value={amount}
                    onChange={(e) => setAmount(e.target.value)}
                    className="w-full pl-8 pr-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:border-blue-500"
                    placeholder="0,00"
                    step="0.01"
                    min="0"
                  />
                </div>
                <button
                  type="submit"
                  disabled={loading}
                  className="bg-blue-600 hover:bg-blue-700 text-white font-semibold px-6 py-2 rounded-lg transition disabled:opacity-50"
                >
                  {loading ? 'Gerando...' : 'Gerar QR Code'}
                </button>
              </div>
            </div>
          </form>

          {qrCode && (
            <div className="border-t pt-8">
              <h2 className="text-xl font-semibold text-gray-800 mb-4">Seu QR Code</h2>

              {/* QR Code Display */}
              <div className="bg-gray-50 p-8 rounded-lg mb-6 flex justify-center">
                <div className="bg-white p-4 rounded-lg">
                  <div className="w-64 h-64 bg-gray-200 flex items-center justify-center rounded">
                    <div className="text-center">
                      <p className="text-gray-600 mb-2">📱 QR Code</p>
                      <p className="text-sm text-gray-500">{qrCode.qrCode.substring(0, 20)}...</p>
                    </div>
                  </div>
                </div>
              </div>

              {/* PIX Key */}
              <div className="bg-blue-50 border border-blue-200 rounded-lg p-4 mb-6">
                <p className="text-sm text-gray-600 mb-2">Chave PIX</p>
                <div className="flex items-center justify-between bg-white p-3 rounded border border-blue-200">
                  <code className="text-sm font-mono text-gray-800 break-all">{qrCode.pixKey}</code>
                  <button
                    onClick={() => copyToClipboard(qrCode.pixKey)}
                    className="ml-2 text-blue-600 hover:text-blue-700 font-semibold text-sm"
                  >
                    {copied ? '✓ Copiado' : 'Copiar'}
                  </button>
                </div>
              </div>

              {qrCode.amount && (
                <div className="bg-green-50 border border-green-200 rounded-lg p-4">
                  <p className="text-sm text-gray-600 mb-1">Valor</p>
                  <p className="text-2xl font-bold text-green-600">R$ {qrCode.amount.toFixed(2)}</p>
                </div>
              )}
            </div>
          )}
        </div>
      </main>
    </div>
  );
}

