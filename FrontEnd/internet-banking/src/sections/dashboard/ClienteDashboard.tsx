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
  Button,
} from '@mui/material';
// icons
import Iconify from '../../components/iconify';
// api
import axios from '../../utils/axios';

// ============================================================================
// TIPOS
// ============================================================================

interface Saldo {
  total: number;
  disponivel: number;
  bloqueado: number;
  moeda: string;
}

interface Transacao {
  id: string;
  tipo: string;
  valor: number;
  status: string;
  data: string;
  descricao: string;
}

// ============================================================================
// COMPONENTE
// ============================================================================

export default function ClienteDashboard() {
  const [loadingSaldo, setLoadingSaldo] = useState(true);
  const [loadingTransacoes, setLoadingTransacoes] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [saldo, setSaldo] = useState<Saldo | null>(null);
  const [transacoes, setTransacoes] = useState<Transacao[]>([]);

  useEffect(() => {
    fetchDashboardData();
  }, []);

  const fetchDashboardData = async () => {
    try {
      setError(null);

      // Buscar saldo
      try {
        setLoadingSaldo(true);
        const saldoResponse = await axios.get('/api/cliente/saldo');
        setSaldo(saldoResponse.data.data);
      } catch (err) {
        console.error('Erro ao buscar saldo:', err);
      } finally {
        setLoadingSaldo(false);
      }

      // Buscar transações
      try {
        setLoadingTransacoes(true);
        const transacoesResponse = await axios.get('/api/cliente/transacoes', {
          params: { page: 1, limit: 10 }
        });
        setTransacoes(transacoesResponse.data.data || []);
      } catch (err) {
        console.error('Erro ao buscar transações:', err);
      } finally {
        setLoadingTransacoes(false);
      }
    } catch (err: any) {
      setError(err.response?.data?.message || 'Erro ao carregar dados');
    }
  };

  return (
    <Box>
      {error && (
        <Alert severity="error" sx={{ mb: 3 }}>
          {error}
        </Alert>
      )}

      {/* CARD DE SALDO */}
      <Grid container spacing={3} sx={{ mb: 4 }}>
        <Grid item xs={12} md={4}>
          <Card sx={{ p: 3, background: 'linear-gradient(135deg, #0066FF 0%, #0052CC 100%)', color: 'white' }}>
            {loadingSaldo ? (
              <CircularProgress sx={{ color: 'white' }} />
            ) : saldo ? (
              <Stack spacing={2}>
                <Typography variant="body2" sx={{ opacity: 0.8 }}>
                  Saldo Total
                </Typography>
                <Typography variant="h3">
                  R$ {(saldo.total / 100).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                </Typography>
                <Box sx={{ display: 'flex', gap: 2, pt: 2 }}>
                  <Box>
                    <Typography variant="caption" sx={{ opacity: 0.8 }}>
                      Disponível
                    </Typography>
                    <Typography variant="h6">
                      R$ {(saldo.disponivel / 100).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                    </Typography>
                  </Box>
                  <Box>
                    <Typography variant="caption" sx={{ opacity: 0.8 }}>
                      Bloqueado
                    </Typography>
                    <Typography variant="h6">
                      R$ {(saldo.bloqueado / 100).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                    </Typography>
                  </Box>
                </Box>
              </Stack>
            ) : (
              <Alert severity="warning">Nenhum saldo disponível</Alert>
            )}
          </Card>
        </Grid>

        {/* AÇÕES RÁPIDAS */}
        <Grid item xs={12} md={8}>
          <Grid container spacing={2}>
            <Grid item xs={12} sm={6}>
              <Card sx={{ p: 3, textAlign: 'center', cursor: 'pointer', '&:hover': { boxShadow: 3 } }}>
                <Iconify icon="eva:arrow-ios-downward-fill" sx={{ color: 'success.main', width: 40, height: 40, mb: 1 }} />
                <Typography variant="subtitle2">Receber via Pix</Typography>
                <Typography variant="caption" sx={{ color: 'text.secondary' }}>
                  Gerar QR Code
                </Typography>
              </Card>
            </Grid>
            <Grid item xs={12} sm={6}>
              <Card sx={{ p: 3, textAlign: 'center', cursor: 'pointer', '&:hover': { boxShadow: 3 } }}>
                <Iconify icon="eva:arrow-ios-upward-fill" sx={{ color: 'error.main', width: 40, height: 40, mb: 1 }} />
                <Typography variant="subtitle2">Fazer Saque</Typography>
                <Typography variant="caption" sx={{ color: 'text.secondary' }}>
                  Transferência bancária
                </Typography>
              </Card>
            </Grid>
          </Grid>
        </Grid>
      </Grid>

      {/* TRANSAÇÕES RECENTES */}
      <Card>
        <Box sx={{ p: 3 }}>
          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
            <Typography variant="h6">Transações Recentes</Typography>
            <Button size="small" endIcon={<Iconify icon="eva:arrow-ios-forward-fill" />}>
              Ver todas
            </Button>
          </Box>

          {loadingTransacoes ? (
            <Box sx={{ display: 'flex', justifyContent: 'center', py: 3 }}>
              <CircularProgress />
            </Box>
          ) : (
            <TableContainer>
              <Table>
                <TableHead>
                  <TableRow sx={{ backgroundColor: 'background.neutral' }}>
                    <TableCell>Descrição</TableCell>
                    <TableCell>Tipo</TableCell>
                    <TableCell align="right">Valor</TableCell>
                    <TableCell>Status</TableCell>
                    <TableCell>Data</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {transacoes && transacoes.length > 0 ? (
                    transacoes.map((transacao) => (
                      <TableRow key={transacao.id} hover>
                        <TableCell>
                          <Typography variant="body2">{transacao.descricao}</Typography>
                        </TableCell>
                        <TableCell>
                          <Typography variant="body2">{transacao.tipo}</Typography>
                        </TableCell>
                        <TableCell align="right">
                          <Typography
                            variant="body2"
                            sx={{
                              fontWeight: 600,
                              color: transacao.tipo === 'ENTRADA' ? 'success.main' : 'error.main',
                            }}
                          >
                            {transacao.tipo === 'ENTRADA' ? '+' : '-'} R${' '}
                            {(transacao.valor / 100).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                          </Typography>
                        </TableCell>
                        <TableCell>
                          <Chip
                            label={transacao.status}
                            size="small"
                            color={
                              transacao.status === 'CONCLUIDA'
                                ? 'success'
                                : transacao.status === 'PENDENTE'
                                ? 'warning'
                                : 'error'
                            }
                            variant="soft"
                          />
                        </TableCell>
                        <TableCell>
                          <Typography variant="body2">
                            {new Date(transacao.data).toLocaleDateString('pt-BR')}
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
          )}
        </Box>
      </Card>
    </Box>
  );
}

