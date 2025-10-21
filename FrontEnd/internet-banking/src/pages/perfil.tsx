// next
import Head from 'next/head';
// @mui
import { Container, Box, Typography, Breadcrumbs, Link, Grid, Card, Stack } from '@mui/material';
// layouts
import MainLayout from '../layouts/main';
// components
import Iconify from '../components/iconify';
// sections
import PerfilForm from '../sections/perfil/PerfilForm';

// ============================================================================

PerfilPage.getLayout = (page: React.ReactElement) => <MainLayout>{page}</MainLayout>;

// ============================================================================

export default function PerfilPage() {
  return (
    <>
      <Head>
        <title>Meu Perfil | Owaypay - Internet Banking</title>
      </Head>

      <Container maxWidth="md" sx={{ py: 4 }}>
        {/* HEADER */}
        <Box sx={{ mb: 4 }}>
          <Breadcrumbs sx={{ mb: 2 }}>
            <Link href="/" underline="hover" sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
              <Iconify icon="eva:home-fill" width={20} height={20} />
              Dashboard
            </Link>
            <Typography color="textPrimary">Meu Perfil</Typography>
          </Breadcrumbs>

          <Box>
            <Typography variant="h4" sx={{ mb: 1 }}>
              Meu Perfil
            </Typography>
            <Typography variant="body2" sx={{ color: 'text.secondary' }}>
              Gerencie suas informações pessoais e dados de conta
            </Typography>
          </Box>
        </Box>

        {/* CONTEÚDO */}
        <PerfilForm />
      </Container>
    </>
  );
}

