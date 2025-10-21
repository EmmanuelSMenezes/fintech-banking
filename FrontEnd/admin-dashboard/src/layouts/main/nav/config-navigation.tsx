// routes
import { PATH_AUTH, PATH_DOCS, PATH_PAGE, PATH_ADMIN } from '../../../routes/paths';
// config
import { PATH_AFTER_LOGIN } from '../../../config-global';
// components
import Iconify from '../../../components/iconify';

// ============================================================================
// OWAYPAY ADMIN DASHBOARD NAVIGATION
// ============================================================================

const navConfig = [
  {
    title: 'Dashboard',
    icon: <Iconify icon="eva:home-fill" />,
    path: '/',
  },
  {
    title: 'Gerenciamento',
    icon: <Iconify icon="eva:people-fill" />,
    path: '/admin',
    children: [
      {
        subheader: 'Administração',
        items: [
          { title: 'Clientes', path: PATH_ADMIN.clientes },
          { title: 'Transações', path: PATH_ADMIN.transacoes },
        ],
      },
    ],
  },
];

export default navConfig;
