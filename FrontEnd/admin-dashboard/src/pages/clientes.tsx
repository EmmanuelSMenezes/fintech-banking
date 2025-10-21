// next
import Head from 'next/head';
// @mui
import { Container, Box, Typography, Breadcrumbs, Link } from '@mui/material';
// layouts
import MainLayout from '../layouts/main';
// components
import Iconify from '../components/iconify';
// sections
import ClientesTable from '../sections/clientes/ClientesTable';

// ============================================================================

ClientesPage.getLayout = (page: React.ReactElement) => <MainLayout>{page}</MainLayout>;

// ============================================================================

export default function ClientesPage() {
  return (
    <>
      <Head>
        <title>Gerenciamento de Clientes | Owaypay - Painel Administrativo</title>
      </Head>

      <Container maxWidth="lg" sx={{ py: 4 }}>
        {/* HEADER */}
        <Box sx={{ mb: 4 }}>
          <Breadcrumbs sx={{ mb: 2 }}>
            <Link href="/" underline="hover" sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
              <Iconify icon="eva:home-fill" width={20} height={20} />
              Dashboard
            </Link>
            <Typography color="textPrimary">Clientes</Typography>
          </Breadcrumbs>

          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
            <Box>
              <Typography variant="h4" sx={{ mb: 1 }}>
                Gerenciamento de Clientes
              </Typography>
              <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                Visualize e gerencie todos os clientes da plataforma Owaypay
              </Typography>
            </Box>
          </Box>
        </Box>

        {/* CONTEÚDO */}
        <ClientesTable />
      </Container>
    </>
  );
}

