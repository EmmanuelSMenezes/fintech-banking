// routes
import { PATH_AUTH, PATH_DOCS, PATH_PAGE, PATH_CLIENTE } from '../../../routes/paths';
// config
import { PATH_AFTER_LOGIN } from '../../../config-global';
// components
import Iconify from '../../../components/iconify';

// ============================================================================
// OWAYPAY INTERNET BANKING NAVIGATION
// ============================================================================

const navConfig = [
  {
    title: 'Dashboard',
    icon: <Iconify icon="eva:home-fill" />,
    path: '/',
  },
  {
    title: 'Minha Conta',
    icon: <Iconify icon="eva:person-fill" />,
    path: '/conta',
    children: [
      {
        subheader: 'Perfil',
        items: [
          { title: 'Meu Perfil', path: PATH_CLIENTE.perfil },
        ],
      },
    ],
  },
];

export default navConfig;
