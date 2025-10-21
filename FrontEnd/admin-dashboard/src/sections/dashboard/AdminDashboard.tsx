import { useEffect, useState } from 'react';
// @mui
import {
  Box,
  Card,
  Grid,
  Stack,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Typography,
  CircularProgress,
  Alert,
  Chip,
} from '@mui/material';
// icons
import Iconify from '../../components/iconify';
// api
import axios from '../../utils/axios';

// ============================================================================
// TIPOS
// ============================================================================

interface DashboardStats {
  totalTransactions: number;
  totalAmount: number;
  pendingTransactions: number;
  activeUsers: number;
}

interface Transaction {
  id: string;
  type: string;
  amount: number;
  status: string;
  date: string;
}

interface DashboardData {
  stats: DashboardStats;
  recentTransactions: Transaction[];
}

// ============================================================================
// COMPONENTE
// ============================================================================

export default function AdminDashboard() {
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [data, setData] = useState<DashboardData | null>(null);

  useEffect(() => {
    fetchDashboardData();
  }, []);

  const fetchDashboardData = async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await axios.get('/api/admin/dashboard');
      setData(response.data.data);
    } catch (err: any) {
      setError(err.response?.data?.message || 'Erro ao carregar dashboard');
      console.error('Dashboard error:', err);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: '400px' }}>
        <CircularProgress />
      </Box>
    );
  }

  if (error) {
    return (
      <Alert severity="error" sx={{ mb: 3 }}>
        {error}
      </Alert>
    );
  }

  if (!data) {
    return (
      <Alert severity="warning" sx={{ mb: 3 }}>
        Nenhum dado disponível
      </Alert>
    );
  }

  const { stats, recentTransactions } = data;

  return (
    <Box>
      {/* CARDS DE ESTATÍSTICAS */}
      <Grid container spacing={3} sx={{ mb: 4 }}>
        {/* Total de Transações */}
        <Grid item xs={12} sm={6} md={3}>
          <Card sx={{ p: 3 }}>
            <Stack spacing={2}>
              <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
                <Box>
                  <Typography variant="body2" sx={{ color: 'text.secondary', mb: 1 }}>
                    Total de Transações
                  </Typography>
                  <Typography variant="h4">{stats.totalTransactions}</Typography>
                </Box>
                <Iconify icon="eva:trending-up-fill" sx={{ color: 'primary.main', width: 32, height: 32 }} />
              </Box>
            </Stack>
          </Card>
        </Grid>

        {/* Valor Total */}
        <Grid item xs={12} sm={6} md={3}>
          <Card sx={{ p: 3 }}>
            <Stack spacing={2}>
              <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
                <Box>
                  <Typography variant="body2" sx={{ color: 'text.secondary', mb: 1 }}>
                    Valor Total
                  </Typography>
                  <Typography variant="h4">
                    R$ {(stats.totalAmount / 100).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                  </Typography>
                </Box>
                <Iconify icon="eva:trending-up-fill" sx={{ color: 'success.main', width: 32, height: 32 }} />
              </Box>
            </Stack>
          </Card>
        </Grid>

        {/* Transações Pendentes */}
        <Grid item xs={12} sm={6} md={3}>
          <Card sx={{ p: 3 }}>
            <Stack spacing={2}>
              <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
                <Box>
                  <Typography variant="body2" sx={{ color: 'text.secondary', mb: 1 }}>
                    Pendentes
                  </Typography>
                  <Typography variant="h4">{stats.pendingTransactions}</Typography>
                </Box>
                <Iconify icon="eva:clock-fill" sx={{ color: 'warning.main', width: 32, height: 32 }} />
              </Box>
            </Stack>
          </Card>
        </Grid>

        {/* Usuários Ativos */}
        <Grid item xs={12} sm={6} md={3}>
          <Card sx={{ p: 3 }}>
            <Stack spacing={2}>
              <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
                <Box>
                  <Typography variant="body2" sx={{ color: 'text.secondary', mb: 1 }}>
                    Usuários Ativos
                  </Typography>
                  <Typography variant="h4">{stats.activeUsers}</Typography>
                </Box>
                <Iconify icon="eva:people-fill" sx={{ color: 'info.main', width: 32, height: 32 }} />
              </Box>
            </Stack>
          </Card>
        </Grid>
      </Grid>

      {/* TRANSAÇÕES RECENTES */}
      <Card>
        <Box sx={{ p: 3 }}>
          <Typography variant="h6" sx={{ mb: 3 }}>
            Transações Recentes
          </Typography>

          <TableContainer>
            <Table>
              <TableHead>
                <TableRow sx={{ backgroundColor: 'background.neutral' }}>
                  <TableCell>ID</TableCell>
                  <TableCell>Tipo</TableCell>
                  <TableCell align="right">Valor</TableCell>
                  <TableCell>Status</TableCell>
                  <TableCell>Data</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {recentTransactions && recentTransactions.length > 0 ? (
                  recentTransactions.map((transaction) => (
                    <TableRow key={transaction.id} hover>
                      <TableCell>
                        <Typography variant="body2" sx={{ fontFamily: 'monospace' }}>
                          {transaction.id.substring(0, 8)}...
                        </Typography>
                      </TableCell>
                      <TableCell>
                        <Typography variant="body2">{transaction.type}</Typography>
                      </TableCell>
                      <TableCell align="right">
                        <Typography variant="body2" sx={{ fontWeight: 600 }}>
                          R$ {(transaction.amount / 100).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                        </Typography>
                      </TableCell>
                      <TableCell>
                        <Chip
                          label={transaction.status}
                          size="small"
                          color={
                            transaction.status === 'COMPLETED'
                              ? 'success'
                              : transaction.status === 'PENDING'
                              ? 'warning'
                              : 'error'
                          }
                          variant="soft"
                        />
                      </TableCell>
                      <TableCell>
                        <Typography variant="body2">
                          {new Date(transaction.date).toLocaleDateString('pt-BR')}
                        </Typography>
                      </TableCell>
                    </TableRow>
                  ))
                ) : (
                  <TableRow>
                    <TableCell colSpan={5} align="center" sx={{ py: 3 }}>
                      <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                        Nenhuma transação encontrada
                      </Typography>
                    </TableCell>
                  </TableRow>
                )}
              </TableBody>
            </Table>
          </TableContainer>
        </Box>
      </Card>
    </Box>
  );
}

