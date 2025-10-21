/**
 * FinTech Banking - Internet Banking API Services
 * API Interna (5036) - Endpoints para clientes
 */

import axios from '@/utils/axios';

// ============================================================================
// TIPOS
// ============================================================================

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  accessToken: string;
  refreshToken: string;
  expiresIn: string;
  user?: {
    id: string;
    email: string;
    name: string;
    cpf: string;
  };
}

export interface SaldoResponse {
  total: number;
  disponivel: number;
  bloqueado: number;
  moeda: string;
}

export interface ExtratoResponse {
  id: string;
  data: string;
  descricao: string;
  tipo: string;
  valor: number;
  saldo: number;
}

export interface TransacaoResponse {
  id: string;
  tipo: string;
  valor: number;
  status: string;
  data: string;
  descricao: string;
}

export interface CobrancaRequest {
  valor: number;
  descricao: string;
  dataVencimento: string;
}

export interface SaqueRequest {
  valor: number;
  banco: string;
  agencia: string;
  conta: string;
}

// ============================================================================
// AUTENTICAÇÃO
// ============================================================================

export const authService = {
  /**
   * Login do cliente
   */
  login: async (email: string, password: string): Promise<LoginResponse> => {
    const response = await axios.post('/api/auth/login', {
      email,
      password,
    });
    return response.data;
  },

  /**
   * Obter perfil do cliente
   */
  getProfile: async () => {
    const response = await axios.get('/api/cliente/perfil');
    return response.data;
  },

  /**
   * Alterar senha
   */
  changePassword: async (senhaAtual: string, novaSenha: string) => {
    const response = await axios.post('/api/cliente/alterar-senha', {
      senhaAtual,
      novaSenha,
    });
    return response.data;
  },

  /**
   * Logout
   */
  logout: async () => {
    const response = await axios.post('/api/auth/logout');
    return response.data;
  },
};

// ============================================================================
// SALDO E EXTRATO
// ============================================================================

export const contaService = {
  /**
   * Obter saldo da conta
   */
  getSaldo: async (): Promise<SaldoResponse> => {
    const response = await axios.get('/api/cliente/saldo');
    return response.data;
  },

  /**
   * Obter extrato
   */
  getExtrato: async (dataInicio?: string, dataFim?: string) => {
    const response = await axios.get('/api/cliente/extrato', {
      params: { dataInicio, dataFim },
    });
    return response.data;
  },

  /**
   * Obter histórico de transações
   */
  getTransacoes: async (page = 1, limit = 10) => {
    const response = await axios.get('/api/cliente/transacoes', {
      params: { page, limit },
    });
    return response.data;
  },

  /**
   * Obter transação por ID
   */
  getTransacao: async (id: string) => {
    const response = await axios.get(`/api/cliente/transacoes/${id}`);
    return response.data;
  },

  /**
   * Atualizar perfil
   */
  updatePerfil: async (data: any) => {
    const response = await axios.put('/api/cliente/perfil', data);
    return response.data;
  },
};

// ============================================================================
// COBRANÇAS
// ============================================================================

export const cobrancaService = {
  /**
   * Gerar cobrança
   */
  create: async (data: CobrancaRequest) => {
    const response = await axios.post('/api/cliente/cobrancas', data);
    return response.data;
  },

  /**
   * Listar cobranças
   */
  list: async (page = 1, limit = 10) => {
    const response = await axios.get('/api/cliente/cobrancas', {
      params: { page, limit },
    });
    return response.data;
  },

  /**
   * Obter cobrança por ID
   */
  getById: async (id: string) => {
    const response = await axios.get(`/api/cliente/cobrancas/${id}`);
    return response.data;
  },

  /**
   * Cancelar cobrança
   */
  cancel: async (id: string) => {
    const response = await axios.post(`/api/cliente/cobrancas/${id}/cancelar`);
    return response.data;
  },
};

// ============================================================================
// SAQUES
// ============================================================================

export const saqueService = {
  /**
   * Solicitar saque
   */
  create: async (data: SaqueRequest) => {
    const response = await axios.post('/api/cliente/saques', data);
    return response.data;
  },

  /**
   * Listar saques
   */
  list: async (page = 1, limit = 10) => {
    const response = await axios.get('/api/cliente/saques', {
      params: { page, limit },
    });
    return response.data;
  },

  /**
   * Obter saque por ID
   */
  getById: async (id: string) => {
    const response = await axios.get(`/api/cliente/saques/${id}`);
    return response.data;
  },

  /**
   * Cancelar saque
   */
  cancel: async (id: string) => {
    const response = await axios.post(`/api/cliente/saques/${id}/cancelar`);
    return response.data;
  },
};

// ============================================================================
// PIX
// ============================================================================

export const pixService = {
  /**
   * Gerar QR Code PIX
   */
  generateQRCode: async (valor: number, descricao?: string) => {
    const response = await axios.post('/api/cliente/pix/qrcode', {
      valor,
      descricao,
    });
    return response.data;
  },

  /**
   * Enviar PIX
   */
  send: async (chave: string, valor: number, descricao?: string) => {
    const response = await axios.post('/api/cliente/pix/send', {
      chave,
      valor,
      descricao,
    });
    return response.data;
  },
};

// ============================================================================
// DASHBOARD
// ============================================================================

export const dashboardService = {
  /**
   * Obter dados do dashboard do cliente
   */
  getMetrics: async () => {
    const response = await axios.get('/api/cliente/dashboard');
    return response.data;
  },
};

