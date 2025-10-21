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

interface Cliente {
  id: string;
  email: string;
  fullName: string;
  isActive: boolean;
  createdAt: string;
}

interface ClienteEditDialogProps {
  open: boolean;
  cliente: Cliente | null;
  onClose: () => void;
  onSuccess: () => void;
}

export default function ClienteEditDialog({
  open,
  cliente,
  onClose,
  onSuccess,
}: ClienteEditDialogProps) {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [formData, setFormData] = useState({
    fullName: cliente?.fullName || '',
    email: cliente?.email || '',
    isActive: cliente?.isActive || true,
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, type, checked } = e.target;
    setFormData({
      ...formData,
      [name]: type === 'checkbox' ? checked : value,
    });
  };

  const handleSubmit = async () => {
    if (!cliente) return;

    try {
      setLoading(true);
      setError(null);

      await axios.put(`/api/admin/users/${cliente.id}`, {
        fullName: formData.fullName,
        email: formData.email,
        isActive: formData.isActive,
      });

      onSuccess();
      onClose();
    } catch (err: any) {
      setError(err.response?.data?.message || 'Erro ao atualizar cliente');
      console.error('Update error:', err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle>Editar Cliente</DialogTitle>
      <DialogContent sx={{ pt: 2 }}>
        {error && (
          <Alert severity="error" sx={{ mb: 2 }}>
            {error}
          </Alert>
        )}

        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
          <TextField
            fullWidth
            label="Nome Completo"
            name="fullName"
            value={formData.fullName}
            onChange={handleChange}
            disabled={loading}
          />

          <TextField
            fullWidth
            label="Email"
            name="email"
            type="email"
            value={formData.email}
            onChange={handleChange}
            disabled={loading}
          />

          <FormControlLabel
            control={
              <Switch
                name="isActive"
                checked={formData.isActive}
                onChange={handleChange}
                disabled={loading}
              />
            }
            label={formData.isActive ? 'Ativo' : 'Inativo'}
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
          {loading ? 'Salvando...' : 'Salvar'}
        </Button>
      </DialogActions>
    </Dialog>
  );
}

