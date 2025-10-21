// next
import Head from 'next/head';
// @mui
import { Container, Box } from '@mui/material';
// layouts
import MainLayout from '../layouts/main';
// sections
import ClienteDashboard from '../sections/dashboard/ClienteDashboard';
// auth
import AuthGuard from '../auth/AuthGuard';

// ============================================================================
// INTERNET BANKING HOME PAGE
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
        <title>Bem vindo ao Owaypay - Internet Banking</title>
        <meta name="description" content="Internet Banking Owaypay" />
      </Head>

      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Box sx={{ mb: 4 }}>
          <ClienteDashboard />
        </Box>
      </Container>
    </>
  );
}
