// next
import Head from 'next/head';
// @mui
import { Container, Box } from '@mui/material';
// layouts
import MainLayout from '../layouts/main';
// sections
import AdminDashboard from '../sections/dashboard/AdminDashboard';
// auth
import AuthGuard from '../auth/AuthGuard';

// ============================================================================
// ADMIN DASHBOARD HOME PAGE
// ============================================================================

HomePage.getLayout = (page: React.ReactElement) => (
  <AuthGuard>
    <MainLayout>{page}</MainLayout>
  </AuthGuard>
);

// ============================================================================

export default function HomePage() {
  return (
    <>
      <Head>
        <title>Bem vindo ao Owaypay - Painel Administrativo</title>
        <meta name="description" content="Painel administrativo Owaypay" />
      </Head>

      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Box sx={{ mb: 4 }}>
          <AdminDashboard />
        </Box>
      </Container>
    </>
  );
}
