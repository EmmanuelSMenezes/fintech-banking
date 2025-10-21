// API URL - Conecta à API Principal (FinTechBanking.API)
const API_URL = 'http://localhost:5064/api';

// Auth
export const register = async (email, password, fullName, document, phoneNumber) => {
  try {
    const response = await fetch(`${API_URL}/auth/register`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, password, fullName, document, phoneNumber })
    });

    if (!response.ok) {
      const error = await response.json();
      return { success: false, message: error.message || 'Erro ao registrar' };
    }

    const data = await response.json();
    return { success: true, data };
  } catch (error) {
    return { success: false, message: error.message };
  }
};

export const login = async (email, password) => {
  try {
    const response = await fetch(`${API_URL}/auth/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, password })
    });

    if (!response.ok) {
      const error = await response.json();
      return { success: false, message: error.message || 'Erro ao fazer login' };
    }

    const data = await response.json();
    // Mapear resposta da API para o formato esperado pelo frontend
    return {
      success: true,
      data: {
        token: data.accessToken,
        refreshToken: data.refreshToken,
        expiresIn: data.expiresIn,
        user: {
          email: email
        }
      }
    };
  } catch (error) {
    return { success: false, message: error.message };
  }
};

// Helper para adicionar token ao header
const getAuthHeaders = (token) => ({
  'Authorization': `Bearer ${token}`,
  'Content-Type': 'application/json'
});

// Accounts
export const getBalance = async (token) => {
  try {
    const response = await fetch(`${API_URL}/accounts/balance`, {
      headers: { 'Authorization': `Bearer ${token}` }
    });

    if (!response.ok) {
      return { success: false, message: 'Erro ao obter saldo' };
    }

    const data = await response.json();
    return { success: true, data };
  } catch (error) {
    return { success: false, message: error.message };
  }
};

export const getAccountDetails = async (token) => {
  try {
    const response = await fetch(`${API_URL}/accounts/details`, {
      headers: { 'Authorization': `Bearer ${token}` }
    });

    if (!response.ok) {
      return { success: false, message: 'Erro ao obter detalhes' };
    }

    const data = await response.json();
    return { success: true, data };
  } catch (error) {
    return { success: false, message: error.message };
  }
};

// Transactions
export const generatePixQrCode = async (token, amount, description, recipientKey) => {
  try {
    const response = await fetch(`${API_URL}/transactions/pix-qrcode`, {
      method: 'POST',
      headers: getAuthHeaders(token),
      body: JSON.stringify({ amount, description, recipientKey })
    });

    if (!response.ok) {
      return { success: false, message: 'Erro ao gerar QR Code' };
    }

    const data = await response.json();
    return { success: true, data };
  } catch (error) {
    return { success: false, message: error.message };
  }
};

export const requestWithdrawal = async (token, amount, accountNumber, bankCode) => {
  try {
    const response = await fetch(`${API_URL}/transactions/withdrawal`, {
      method: 'POST',
      headers: getAuthHeaders(token),
      body: JSON.stringify({ amount, accountNumber, bankCode })
    });

    if (!response.ok) {
      return { success: false, message: 'Erro ao solicitar saque' };
    }

    const data = await response.json();
    return { success: true, data };
  } catch (error) {
    return { success: false, message: error.message };
  }
};

export const getTransactionStatus = async (token, transactionId) => {
  try {
    const response = await fetch(`${API_URL}/transactions/${transactionId}`, {
      headers: { 'Authorization': `Bearer ${token}` }
    });

    if (!response.ok) {
      return { success: false, message: 'Erro ao obter status' };
    }

    const data = await response.json();
    return { success: true, data };
  } catch (error) {
    return { success: false, message: error.message };
  }
};

export const getTransactionHistory = async (token) => {
  try {
    const response = await fetch(`${API_URL}/transactions/history`, {
      headers: { 'Authorization': `Bearer ${token}` }
    });

    if (!response.ok) {
      return { success: false, message: 'Erro ao obter histórico' };
    }

    const data = await response.json();
    return { success: true, data };
  } catch (error) {
    return { success: false, message: error.message };
  }
};

