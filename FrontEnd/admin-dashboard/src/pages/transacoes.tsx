// next
import Head from 'next/head';
// @mui
import { Container, Box, Typography, Breadcrumbs, Link } from '@mui/material';
// layouts
import MainLayout from '../layouts/main';
// components
import Iconify from '../components/iconify';
// sections
import TransacoesTable from '../sections/transacoes/TransacoesTable';
// auth
import AuthGuard from '../auth/AuthGuard';

// ============================================================================

TransacoesPage.getLayout = (page: React.ReactElement) => (
  <AuthGuard>
    <MainLayout>{page}</MainLayout>
  </AuthGuard>
);

// ============================================================================

export default function TransacoesPage() {
  return (
    <>
      <Head>
        <title>Relatório de Transações | Owaypay - Painel Administrativo</title>
      </Head>

      <Container maxWidth="lg" sx={{ py: 4 }}>
        {/* HEADER */}
        <Box sx={{ mb: 4 }}>
          <Breadcrumbs sx={{ mb: 2 }}>
            <Link href="/" underline="hover" sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
              <Iconify icon="eva:home-fill" width={20} height={20} />
              Dashboard
            </Link>
            <Typography color="textPrimary">Transações</Typography>
          </Breadcrumbs>

          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
            <Box>
              <Typography variant="h4" sx={{ mb: 1 }}>
                Relatório de Transações
              </Typography>
              <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                Visualize todas as transações realizadas na plataforma Owaypay
              </Typography>
            </Box>
          </Box>
        </Box>

        {/* CONTEÚDO */}
        <TransacoesTable />
      </Container>
    </>
  );
}

