import { useEffect, useState } from 'react';
// @mui
import {
  Box,
  Card,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TablePagination,
  Typography,
  CircularProgress,
  Alert,
  Chip,
  TextField,
  Stack,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
} from '@mui/material';
// icons
import Iconify from '../../components/iconify';
// api
import axios from '../../utils/axios';

// ============================================================================
// TIPOS
// ============================================================================

interface Transacao {
  id: string;
  type: string;
  amount: number;
  status: string;
  date: string;
  userId?: string;
}

interface TransacoesResponse {
  transactions: Transacao[];
  page: number;
  pageSize: number;
  total: number;
}

// ============================================================================
// COMPONENTE
// ============================================================================

export default function TransacoesTable() {
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [data, setData] = useState<TransacoesResponse | null>(null);
  const [page, setPage] = useState(0);
  const [pageSize, setPageSize] = useState(10);
  const [statusFilter, setStatusFilter] = useState('');
  const [typeFilter, setTypeFilter] = useState('');

  useEffect(() => {
    fetchTransacoes();
  }, [page, pageSize, statusFilter, typeFilter]);

  const fetchTransacoes = async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await axios.get('/api/admin/transactions', {
        params: {
          page: page + 1,
          pageSize: pageSize,
          status: statusFilter || undefined,
          type: typeFilter || undefined,
        },
      });
      setData(response.data.data);
    } catch (err: any) {
      setError(err.response?.data?.message || 'Erro ao carregar transações');
      console.error('Transacoes error:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleChangePage = (event: unknown, newPage: number) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
    setPageSize(parseInt(event.target.value, 10));
    setPage(0);
  };

  if (error) {
    return (
      <Alert severity="error" sx={{ mb: 3 }}>
        {error}
      </Alert>
    );
  }

  return (
    <Card>
      <Box sx={{ p: 3 }}>
        <Box sx={{ mb: 3 }}>
          <Typography variant="h6" sx={{ mb: 2 }}>
            Relatório de Transações
          </Typography>

          {/* FILTROS */}
          <Stack direction={{ xs: 'column', sm: 'row' }} spacing={2}>
            <FormControl sx={{ minWidth: 150 }}>
              <InputLabel>Status</InputLabel>
              <Select
                value={statusFilter}
                label="Status"
                onChange={(e) => {
                  setStatusFilter(e.target.value);
                  setPage(0);
                }}
              >
                <MenuItem value="">Todos</MenuItem>
                <MenuItem value="PENDING">Pendente</MenuItem>
                <MenuItem value="COMPLETED">Concluída</MenuItem>
                <MenuItem value="FAILED">Falha</MenuItem>
              </Select>
            </FormControl>

            <FormControl sx={{ minWidth: 150 }}>
              <InputLabel>Tipo</InputLabel>
              <Select
                value={typeFilter}
                label="Tipo"
                onChange={(e) => {
                  setTypeFilter(e.target.value);
                  setPage(0);
                }}
              >
                <MenuItem value="">Todos</MenuItem>
                <MenuItem value="PIX">PIX</MenuItem>
                <MenuItem value="WITHDRAWAL">Saque</MenuItem>
                <MenuItem value="TRANSFER">Transferência</MenuItem>
              </Select>
            </FormControl>
          </Stack>
        </Box>

        {loading ? (
          <Box sx={{ display: 'flex', justifyContent: 'center', py: 3 }}>
            <CircularProgress />
          </Box>
        ) : (
          <>
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
                  {data?.transactions && data.transactions.length > 0 ? (
                    data.transactions.map((transacao) => (
                      <TableRow key={transacao.id} hover>
                        <TableCell>
                          <Typography variant="body2" sx={{ fontFamily: 'monospace' }}>
                            {transacao.id.substring(0, 8)}...
                          </Typography>
                        </TableCell>
                        <TableCell>
                          <Typography variant="body2">{transacao.type}</Typography>
                        </TableCell>
                        <TableCell align="right">
                          <Typography variant="body2" sx={{ fontWeight: 600 }}>
                            R$ {(transacao.amount / 100).toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                          </Typography>
                        </TableCell>
                        <TableCell>
                          <Chip
                            label={transacao.status}
                            size="small"
                            color={
                              transacao.status === 'COMPLETED'
                                ? 'success'
                                : transacao.status === 'PENDING'
                                ? 'warning'
                                : 'error'
                            }
                            variant="soft"
                          />
                        </TableCell>
                        <TableCell>
                          <Typography variant="body2">
                            {new Date(transacao.date).toLocaleDateString('pt-BR')}
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

            <TablePagination
              rowsPerPageOptions={[5, 10, 25]}
              component="div"
              count={data?.total || 0}
              rowsPerPage={pageSize}
              page={page}
              onPageChange={handleChangePage}
              onRowsPerPageChange={handleChangeRowsPerPage}
              labelRowsPerPage="Linhas por página:"
              labelDisplayedRows={({ from, to, count }) => `${from}–${to} de ${count}`}
            />
          </>
        )}
      </Box>
    </Card>
  );
}

