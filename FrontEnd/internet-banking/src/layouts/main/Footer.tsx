// @mui
import { Box, Container, Typography } from '@mui/material';

// ============================================================================
// FOOTER COMPONENT
// ============================================================================

export default function Footer() {
  return (
    <Box
      component="footer"
      sx={{
        py: 3,
        textAlign: 'center',
        position: 'relative',
        bgcolor: 'background.default',
        borderTop: '1px solid',
        borderColor: 'divider',
      }}
    >
      <Container>
        <Typography variant="body2" sx={{ color: 'text.secondary' }}>
          © 2025 Owaypay. Todos os direitos reservados.
        </Typography>
        <Typography variant="caption" sx={{ color: 'text.secondary', display: 'block', mt: 1 }}>
          Internet Banking Seguro e Confiável
        </Typography>
      </Container>
    </Box>
  );
}
