// next
import NextLink from 'next/link';
// @mui
import { Stack, Typography, Link } from '@mui/material';
// layouts
import LoginLayout from '../../layouts/login';
// routes
import { PATH_AUTH } from '../../routes/paths';
//
import AuthWithSocial from './AuthWithSocial';
import AuthRegisterForm from './AuthRegisterForm';

// ----------------------------------------------------------------------

export default function Register() {
  return (
    <LoginLayout title="Abra sua conta Owaypay">
      <Stack spacing={2} sx={{ mb: 5, position: 'relative' }}>
        <Typography variant="h4">Crie sua conta gratuitamente</Typography>

        <Stack direction="row" spacing={0.5}>
          <Typography variant="body2">Já tem conta?</Typography>

          <Link component={NextLink} href={PATH_AUTH.login} variant="subtitle2">
            Entrar
          </Link>
        </Stack>
      </Stack>

      <AuthRegisterForm />

      <Typography
        component="div"
        sx={{ color: 'text.secondary', mt: 3, typography: 'caption', textAlign: 'center' }}
      >
        {'Ao criar sua conta, você concorda com os '}
        <Link underline="always" color="text.primary">
          Termos de Serviço
        </Link>
        {' e '}
        <Link underline="always" color="text.primary">
          Política de Privacidade
        </Link>
        {' da Owaypay.'}
      </Typography>

      <AuthWithSocial />
    </LoginLayout>
  );
}
