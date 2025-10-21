// next
import Head from 'next/head';
// @mui
import { Container, Box } from '@mui/material';
// layouts
import MainLayout from '../layouts/main';
// sections
import AdminDashboard from '../sections/dashboard/AdminDashboard';

// ----------------------------------------------------------------------

HomePage.getLayout = (page: React.ReactElement) => <MainLayout> {page} </MainLayout>;

// ----------------------------------------------------------------------

export default function HomePage() {
  return (
    <>
      <Head>
        <title>Bem vindo ao Owaypay - Painel Administrativo</title>
      </Head>

      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Box sx={{ mb: 4 }}>
          <AdminDashboard />
        </Box>
      </Container>
    </>
  );
}
