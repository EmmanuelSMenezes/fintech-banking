/**
 * FinTech Banking - Admin Dashboard API Services
 * API Interna (5036) - Endpoints para administradores
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
    role: string;
  };
}

export interface ClienteResponse {
  id: string;
  name: string;
  email: string;
  cpf: string;
  status: string;
  createdAt: string;
}

export interface UsuarioResponse {
  id: string;
  name: string;
  email: string;
  role: string;
  clienteId: string;
  status: string;
}

export interface TransacaoResponse {
  id: string;
  clienteId: string;
  type: string;
  amount: number;
  status: string;
  createdAt: string;
}

// ============================================================================
// AUTENTICAÇÃO
// ============================================================================

export const authService = {
  /**
   * Login do administrador
   */
  login: async (email: string, password: string): Promise<LoginResponse> => {
    const response = await axios.post('/api/auth/login', {
      email,
      password,
    });
    return response.data;
  },

  /**
   * Obter perfil do administrador
   */
  getProfile: async () => {
    const response = await axios.get('/api/admin/users/profile');
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
// USUÁRIOS (Clientes)
// ============================================================================

export const clienteService = {
  /**
   * Listar todos os usuários/clientes
   */
  list: async (page = 1, limit = 10) => {
    const response = await axios.get('/api/admin/users', {
      params: { page, limit },
    });
    return response.data;
  },

  /**
   * Obter usuário/cliente por ID
   */
  getById: async (id: string) => {
    const response = await axios.get(`/api/admin/users/${id}`);
    return response.data;
  },

  /**
   * Criar novo usuário/cliente
   */
  create: async (data: any) => {
    const response = await axios.post('/api/admin/users', data);
    return response.data;
  },

  /**
   * Atualizar usuário/cliente
   */
  update: async (id: string, data: any) => {
    const response = await axios.put(`/api/admin/users/${id}`, data);
    return response.data;
  },

  /**
   * Deletar usuário/cliente
   */
  delete: async (id: string) => {
    const response = await axios.delete(`/api/admin/users/${id}`);
    return response.data;
  },
};

// Alias para compatibilidade
export const usuarioService = clienteService;

// ============================================================================
// TRANSAÇÕES
// ============================================================================

export const transacaoService = {
  /**
   * Listar transações
   */
  list: async (page = 1, limit = 10, filters?: any) => {
    const response = await axios.get('/api/admin/transactions', {
      params: { page, limit, ...filters },
    });
    return response.data;
  },

  /**
   * Obter transação por ID
   */
  getById: async (id: string) => {
    const response = await axios.get(`/api/admin/transactions/${id}`);
    return response.data;
  },

  /**
   * Obter transações por usuário
   */
  getByCliente: async (clienteId: string, page = 1, limit = 10) => {
    const response = await axios.get(`/api/admin/users/${clienteId}/transactions`, {
      params: { page, limit },
    });
    return response.data;
  },
};

// ============================================================================
// WEBHOOKS
// ============================================================================

export const webhookService = {
  /**
   * Listar logs de webhooks
   */
  logs: async (page = 1, limit = 10) => {
    const response = await axios.get('/api/admin/webhooks/logs', {
      params: { page, limit },
    });
    return response.data;
  },

  /**
   * Obter log de webhook por ID
   */
  getLog: async (id: string) => {
    const response = await axios.get(`/api/admin/webhooks/logs/${id}`);
    return response.data;
  },
};

// ============================================================================
// LIBERAÇÕES MANUAIS
// ============================================================================

export const liberacaoService = {
  /**
   * Executar liberação manual
   */
  execute: async (data: any) => {
    const response = await axios.post('/api/admin/liberacoes', data);
    return response.data;
  },

  /**
   * Listar liberações
   */
  list: async (page = 1, limit = 10) => {
    const response = await axios.get('/api/admin/liberacoes', {
      params: { page, limit },
    });
    return response.data;
  },
};

// ============================================================================
// DASHBOARD
// ============================================================================

export const dashboardService = {
  /**
   * Obter dados do dashboard
   */
  getMetrics: async () => {
    const response = await axios.get('/api/admin/dashboard');
    return response.data;
  },
};

