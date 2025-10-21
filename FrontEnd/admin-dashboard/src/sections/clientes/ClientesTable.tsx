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
  IconButton,
  Menu,
  MenuItem,
  TextField,
  Stack,
} from '@mui/material';
// icons
import Iconify from '../../components/iconify';
// api
import axios from '../../utils/axios';

// ============================================================================
// TIPOS
// ============================================================================

interface Cliente {
  id: string;
  email: string;
  fullName: string;
  isActive: boolean;
  createdAt: string;
}

interface ClientesResponse {
  users: Cliente[];
  page: number;
  pageSize: number;
  total: number;
}

// ============================================================================
// COMPONENTE
// ============================================================================

export default function ClientesTable() {
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [data, setData] = useState<ClientesResponse | null>(null);
  const [page, setPage] = useState(0);
  const [pageSize, setPageSize] = useState(10);
  const [searchTerm, setSearchTerm] = useState('');
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const [selectedCliente, setSelectedCliente] = useState<Cliente | null>(null);

  useEffect(() => {
    fetchClientes();
  }, [page, pageSize]);

  const fetchClientes = async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await axios.get('/api/admin/users', {
        params: {
          page: page + 1,
          pageSize: pageSize,
        },
      });
      setData(response.data.data);
    } catch (err: any) {
      setError(err.response?.data?.message || 'Erro ao carregar clientes');
      console.error('Clientes error:', err);
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

  const handleMenuOpen = (event: React.MouseEvent<HTMLElement>, cliente: Cliente) => {
    setAnchorEl(event.currentTarget);
    setSelectedCliente(cliente);
  };

  const handleMenuClose = () => {
    setAnchorEl(null);
    setSelectedCliente(null);
  };

  const handleViewDetails = () => {
    console.log('Ver detalhes:', selectedCliente);
    handleMenuClose();
  };

  const handleToggleActive = async () => {
    if (!selectedCliente) return;
    try {
      // Aqui você faria a chamada para ativar/desativar o cliente
      console.log('Toggle active:', selectedCliente.id);
      handleMenuClose();
      fetchClientes();
    } catch (err) {
      console.error('Erro ao atualizar cliente:', err);
    }
  };

  if (error) {
    return (
      <Alert severity="error" sx={{ mb: 3 }}>
        {error}
      </Alert>
    );
  }

  const filteredClientes = data?.users.filter(
    (cliente) =>
      cliente.email.toLowerCase().includes(searchTerm.toLowerCase()) ||
      cliente.fullName.toLowerCase().includes(searchTerm.toLowerCase())
  ) || [];

  return (
    <Card>
      <Box sx={{ p: 3 }}>
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
          <Typography variant="h6">Gerenciamento de Clientes</Typography>
          <TextField
            placeholder="Buscar por nome ou email..."
            size="small"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            InputProps={{
              startAdornment: <Iconify icon="eva:search-fill" sx={{ mr: 1, color: 'text.disabled' }} />,
            }}
          />
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
                    <TableCell>Nome</TableCell>
                    <TableCell>Email</TableCell>
                    <TableCell>Status</TableCell>
                    <TableCell>Data de Criação</TableCell>
                    <TableCell align="center">Ações</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {filteredClientes && filteredClientes.length > 0 ? (
                    filteredClientes.map((cliente) => (
                      <TableRow key={cliente.id} hover>
                        <TableCell>
                          <Typography variant="body2">{cliente.fullName}</Typography>
                        </TableCell>
                        <TableCell>
                          <Typography variant="body2" sx={{ fontFamily: 'monospace' }}>
                            {cliente.email}
                          </Typography>
                        </TableCell>
                        <TableCell>
                          <Chip
                            label={cliente.isActive ? 'Ativo' : 'Inativo'}
                            size="small"
                            color={cliente.isActive ? 'success' : 'default'}
                            variant="soft"
                          />
                        </TableCell>
                        <TableCell>
                          <Typography variant="body2">
                            {new Date(cliente.createdAt).toLocaleDateString('pt-BR')}
                          </Typography>
                        </TableCell>
                        <TableCell align="center">
                          <IconButton
                            size="small"
                            onClick={(e) => handleMenuOpen(e, cliente)}
                          >
                            <Iconify icon="eva:more-vertical-fill" />
                          </IconButton>
                        </TableCell>
                      </TableRow>
                    ))
                  ) : (
                    <TableRow>
                      <TableCell colSpan={5} align="center" sx={{ py: 3 }}>
                        <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                          Nenhum cliente encontrado
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

      {/* MENU DE AÇÕES */}
      <Menu
        anchorEl={anchorEl}
        open={Boolean(anchorEl)}
        onClose={handleMenuClose}
      >
        <MenuItem onClick={handleViewDetails}>
          <Iconify icon="eva:eye-fill" sx={{ mr: 2 }} />
          Ver Detalhes
        </MenuItem>
        <MenuItem onClick={handleToggleActive}>
          <Iconify icon="eva:edit-fill" sx={{ mr: 2 }} />
          {selectedCliente?.isActive ? 'Desativar' : 'Ativar'}
        </MenuItem>
      </Menu>
    </Card>
  );
}

