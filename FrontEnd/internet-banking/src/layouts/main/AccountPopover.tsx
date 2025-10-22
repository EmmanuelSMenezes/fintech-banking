import { useState } from 'react';
// next
import { useRouter } from 'next/router';
// @mui
import {
  Box,
  Avatar,
  Divider,
  MenuItem,
  IconButton,
  Popover,
  Typography,
} from '@mui/material';
// auth
import { useAuthContext } from '../../auth/useAuthContext';
// components
import Iconify from '../../components/iconify';

// ============================================================================

export default function AccountPopover() {
  const router = useRouter();
  const { user, logout } = useAuthContext();

  const [open, setOpen] = useState<HTMLButtonElement | null>(null);

  const handleOpen = (event: React.MouseEvent<HTMLButtonElement>) => {
    setOpen(event.currentTarget);
  };

  const handleClose = () => {
    setOpen(null);
  };

  const handleLogout = () => {
    handleClose();
    logout();
    router.push('/auth/login');
  };

  return (
    <>
      <IconButton
        onClick={handleOpen}
        sx={{
          p: 0,
          ...(open && {
            '&:before': {
              zIndex: 1,
              content: "''",
              width: '100%',
              height: '100%',
              borderRadius: '50%',
              position: 'absolute',
              bgcolor: (theme) => theme.palette.primary.main,
              opacity: 0.08,
            },
          }),
        }}
      >
        <Avatar sx={{ bgcolor: 'primary.main', color: 'white' }}>
          {user?.email ? user.email.charAt(0).toUpperCase() : 'C'}
        </Avatar>
      </IconButton>

      <Popover
        open={Boolean(open)}
        anchorEl={open}
        onClose={handleClose}
        anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}
        transformOrigin={{ vertical: 'top', horizontal: 'right' }}
        PaperProps={{
          sx: {
            p: 0,
            mt: 1.5,
            ml: 0.75,
            width: 180,
          },
        }}
      >
        <Box sx={{ my: 1.5, px: 2.5 }}>
          <Typography variant="subtitle2" noWrap>
            {user?.name || 'Cliente'}
          </Typography>
          <Typography variant="body2" sx={{ color: 'text.secondary' }} noWrap>
            {user?.email}
          </Typography>
        </Box>

        <Divider sx={{ borderStyle: 'dashed' }} />

        <MenuItem onClick={handleClose} sx={{ m: 1 }}>
          <Iconify icon="eva:settings-2-fill" sx={{ mr: 2 }} />
          Configurações
        </MenuItem>

        <Divider sx={{ borderStyle: 'dashed' }} />

        <MenuItem onClick={handleLogout} sx={{ m: 1 }}>
          <Iconify icon="eva:logout-fill" sx={{ mr: 2 }} />
          Sair
        </MenuItem>
      </Popover>
    </>
  );
}

