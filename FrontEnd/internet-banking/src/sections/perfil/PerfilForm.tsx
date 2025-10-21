import { useEffect, useState } from 'react';
import * as Yup from 'yup';
// @mui
import {
  Box,
  Card,
  Stack,
  TextField,
  Button,
  Alert,
  CircularProgress,
  Typography,
  Divider,
} from '@mui/material';
// form
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
// api
import axios from '../../utils/axios';
// components
import FormProvider, { RHFTextField } from '../../components/hook-form';

// ============================================================================
// TIPOS
// ============================================================================

interface PerfilData {
  id: string;
  email: string;
  fullName: string;
  cpf: string;
  phoneNumber?: string;
  createdAt: string;
}

interface UpdatePerfilRequest {
  fullName: string;
  phoneNumber?: string;
}

// ============================================================================
// COMPONENTE
// ============================================================================

export default function PerfilForm() {
  const [loading, setLoading] = useState(true);
  const [updating, setUpdating] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState(false);
  const [perfil, setPerfil] = useState<PerfilData | null>(null);

  const PerfilSchema = Yup.object().shape({
    fullName: Yup.string().required('Nome completo é obrigatório'),
    phoneNumber: Yup.string(),
  });

  const methods = useForm<UpdatePerfilRequest>({
    resolver: yupResolver(PerfilSchema),
    defaultValues: {
      fullName: '',
      phoneNumber: '',
    },
  });

  const { handleSubmit, reset } = methods;

  useEffect(() => {
    fetchPerfil();
  }, []);

  const fetchPerfil = async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await axios.get('/api/cliente/perfil');
      setPerfil(response.data);
      reset({
        fullName: response.data.fullName,
        phoneNumber: response.data.phoneNumber || '',
      });
    } catch (err: any) {
      setError(err.response?.data?.message || 'Erro ao carregar perfil');
      console.error('Perfil error:', err);
    } finally {
      setLoading(false);
    }
  };

  const onSubmit = async (data: UpdatePerfilRequest) => {
    try {
      setUpdating(true);
      setError(null);
      setSuccess(false);
      
      await axios.put('/api/cliente/perfil', data);
      
      setSuccess(true);
      setTimeout(() => setSuccess(false), 3000);
      
      // Atualizar dados locais
      if (perfil) {
        setPerfil({
          ...perfil,
          fullName: data.fullName,
          phoneNumber: data.phoneNumber,
        });
      }
    } catch (err: any) {
      setError(err.response?.data?.message || 'Erro ao atualizar perfil');
    } finally {
      setUpdating(false);
    }
  };

  if (loading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: '400px' }}>
        <CircularProgress />
      </Box>
    );
  }

  if (!perfil) {
    return (
      <Alert severity="error">
        Erro ao carregar dados do perfil
      </Alert>
    );
  }

  return (
    <Card sx={{ p: 3 }}>
      {error && (
        <Alert severity="error" sx={{ mb: 3 }}>
          {error}
        </Alert>
      )}

      {success && (
        <Alert severity="success" sx={{ mb: 3 }}>
          Perfil atualizado com sucesso!
        </Alert>
      )}

      <FormProvider methods={methods} onSubmit={handleSubmit(onSubmit)}>
        <Stack spacing={3}>
          {/* INFORMAÇÕES BÁSICAS */}
          <Box>
            <Typography variant="h6" sx={{ mb: 2 }}>
              Informações Básicas
            </Typography>
            <Stack spacing={2}>
              <TextField
                fullWidth
                label="Email"
                value={perfil.email}
                disabled
                helperText="Email não pode ser alterado"
              />
              <RHFTextField
                name="fullName"
                label="Nome Completo"
              />
              <RHFTextField
                name="phoneNumber"
                label="Telefone"
                placeholder="(11) 99999-9999"
              />
            </Stack>
          </Box>

          <Divider />

          {/* INFORMAÇÕES DE CONTA */}
          <Box>
            <Typography variant="h6" sx={{ mb: 2 }}>
              Informações de Conta
            </Typography>
            <Stack spacing={2}>
              <TextField
                fullWidth
                label="CPF"
                value={perfil.cpf}
                disabled
                helperText="CPF não pode ser alterado"
              />
              <TextField
                fullWidth
                label="Data de Criação"
                value={new Date(perfil.createdAt).toLocaleDateString('pt-BR')}
                disabled
              />
            </Stack>
          </Box>

          <Divider />

          {/* BOTÕES */}
          <Box sx={{ display: 'flex', gap: 2, justifyContent: 'flex-end' }}>
            <Button
              variant="outlined"
              onClick={() => reset()}
              disabled={updating}
            >
              Cancelar
            </Button>
            <Button
              variant="contained"
              type="submit"
              loading={updating}
              disabled={updating}
            >
              Salvar Alterações
            </Button>
          </Box>
        </Stack>
      </FormProvider>
    </Card>
  );
}

