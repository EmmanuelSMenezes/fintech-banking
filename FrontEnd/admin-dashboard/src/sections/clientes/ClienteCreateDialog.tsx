import { useState } from 'react';
// @mui
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  Button,
  Box,
  Alert,
  CircularProgress,
  FormControlLabel,
  Switch,
} from '@mui/material';
// api
import axios from '../../utils/axios';

// ============================================================================

interface ClienteCreateDialogProps {
  open: boolean;
  onClose: () => void;
  onSuccess: () => void;
}

export default function ClienteCreateDialog({
  open,
  onClose,
  onSuccess,
}: ClienteCreateDialogProps) {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [formData, setFormData] = useState({
    email: '',
    password: '',
    fullName: '',
    document: '',
    phoneNumber: '',
    isActive: true,
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, type, checked } = e.target;
    setFormData({
      ...formData,
      [name]: type === 'checkbox' ? checked : value,
    });
  };

  const handleSubmit = async () => {
    try {
      setLoading(true);
      setError(null);

      // Validação básica
      if (!formData.email || !formData.password || !formData.fullName || !formData.document || !formData.phoneNumber) {
        setError('Todos os campos são obrigatórios');
        setLoading(false);
        return;
      }

      await axios.post('/api/admin/users', {
        email: formData.email,
        password: formData.password,
        fullName: formData.fullName,
        document: formData.document,
        phoneNumber: formData.phoneNumber,
      });

      // Reset form
      setFormData({
        email: '',
        password: '',
        fullName: '',
        document: '',
        phoneNumber: '',
        isActive: true,
      });

      onSuccess();
      onClose();
    } catch (err: any) {
      setError(err.response?.data?.message || 'Erro ao criar cliente');
      console.error('Create error:', err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle>Criar Novo Cliente</DialogTitle>
      <DialogContent sx={{ pt: 2 }}>
        {error && (
          <Alert severity="error" sx={{ mb: 2 }}>
            {error}
          </Alert>
        )}

        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
          <TextField
            fullWidth
            label="Email"
            name="email"
            type="email"
            value={formData.email}
            onChange={handleChange}
            disabled={loading}
            placeholder="cliente@owaypay.com"
          />

          <TextField
            fullWidth
            label="Senha"
            name="password"
            type="password"
            value={formData.password}
            onChange={handleChange}
            disabled={loading}
            placeholder="Mínimo 8 caracteres"
          />

          <TextField
            fullWidth
            label="Nome Completo"
            name="fullName"
            value={formData.fullName}
            onChange={handleChange}
            disabled={loading}
            placeholder="João Silva"
          />

          <TextField
            fullWidth
            label="CPF/CNPJ"
            name="document"
            value={formData.document}
            onChange={handleChange}
            disabled={loading}
            placeholder="000.000.000-00"
          />

          <TextField
            fullWidth
            label="Telefone"
            name="phoneNumber"
            value={formData.phoneNumber}
            onChange={handleChange}
            disabled={loading}
            placeholder="(11) 99999-9999"
          />
        </Box>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} disabled={loading}>
          Cancelar
        </Button>
        <Button
          onClick={handleSubmit}
          variant="contained"
          disabled={loading}
          startIcon={loading ? <CircularProgress size={20} /> : undefined}
        >
          {loading ? 'Criando...' : 'Criar Cliente'}
        </Button>
      </DialogActions>
    </Dialog>
  );
}

