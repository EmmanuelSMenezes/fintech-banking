// next
import Head from 'next/head';
import { useEffect, useState } from 'react';
// @mui
import { Container, Box, Typography, Grid, Card, Stack, CircularProgress, Alert, Chip } from '@mui/material';
// layouts
import MainLayout from '../layouts/main';
// auth
import AuthGuard from '../auth/AuthGuard';
// components
import Iconify from '../components/iconify';
// api
import axios from '../utils/axios';

// ============================================================================
// ADMIN DASHBOARD HOME PAGE
// ============================================================================

HomePage.getLayout = (page: React.ReactElement) => (
  <AuthGuard>
    <MainLayout>{page}</MainLayout>
  </AuthGuard>
);

// ============================================================================

interface DashboardData {
  stats: {
    totalTransactions: number;
    totalAmount: number;
    pendingTransactions: number;
    activeUsers: number;
  };
  recentTransactions: Array<{
    id: string;
    type: string;
    amount: number;
    status: string;
    date: string;
  }>;
}

export default function HomePage() {
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [data, setData] = useState<DashboardData | null>(null);

  useEffect(() => {
    fetchDashboard();
  }, []);

  const fetchDashboard = async () => {
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
      <Container maxWidth="lg" sx={{ py: 4, display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: '400px' }}>
        <CircularProgress />
      </Container>
    );
  }

  if (error) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Alert severity="error">{error}</Alert>
      </Container>
    );
  }

  if (!data) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Alert severity="warning">Nenhum dado disponível</Alert>
      </Container>
    );
  }

  const { stats, recentTransactions } = data;

  return (
    <>
      <Head>
        <title>Bem vindo ao Owaypay - Painel Administrativo</title>
        <meta name="description" content="Painel administrativo Owaypay" />
      </Head>

      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Box sx={{ mb: 4 }}>
          <Typography variant="h3" sx={{ mb: 3, fontWeight: 700 }}>
            Bem-vindo ao Owaypay
          </Typography>
          <Typography variant="body1" sx={{ color: 'text.secondary', mb: 4 }}>
            Painel de Administração
          </Typography>

          {/* CARDS DE ESTATÍSTICAS */}
          <Grid container spacing={3} sx={{ mb: 4 }}>
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
          <Card sx={{ p: 3 }}>
            <Typography variant="h6" sx={{ mb: 3 }}>
              Transações Recentes
            </Typography>
            {recentTransactions && recentTransactions.length > 0 ? (
              <Stack spacing={2}>
                {recentTransactions.map((transaction) => (
                  <Box
                    key={transaction.id}
                    sx={{
                      display: 'flex',
                      justifyContent: 'space-between',
                      alignItems: 'center',
                      p: 2,
                      borderRadius: 1,
                      bgcolor: 'background.neutral',
                    }}
                  >
                    <Box>
                      <Typography variant="body2" sx={{ fontWeight: 600 }}>
                        {transaction.type}
                      </Typography>
                      <Typography variant="caption" sx={{ color: 'text.secondary' }}>
                        {new Date(transaction.date).toLocaleDateString('pt-BR')}
                      </Typography>
                    </Box>
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
                      <Typography variant="body2" sx={{ fontWeight: 600 }}>
                        R$ {(transaction.amount / 100).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                      </Typography>
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
                    </Box>
                  </Box>
                ))}
              </Stack>
            ) : (
              <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                Nenhuma transação encontrada
              </Typography>
            )}
          </Card>
        </Box>
      </Container>
    </>
  );
}
