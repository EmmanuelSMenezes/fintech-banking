import { alpha } from '@mui/material/styles';

// ----------------------------------------------------------------------

export type ColorSchema = 'primary' | 'secondary' | 'info' | 'success' | 'warning' | 'error';

declare module '@mui/material/styles/createPalette' {
  interface TypeBackground {
    neutral: string;
  }
  interface SimplePaletteColorOptions {
    lighter: string;
    darker: string;
  }
  interface PaletteColor {
    lighter: string;
    darker: string;
  }
}

// SETUP COLORS

const GREY = {
  0: '#FFFFFF',
  100: '#F9FAFB',
  200: '#F4F6F8',
  300: '#DFE3E8',
  400: '#C4CDD5',
  500: '#919EAB',
  600: '#637381',
  700: '#454F5B',
  800: '#212B36',
  900: '#161C24',
};

// Owaypay Brand Colors
const PRIMARY = {
  lighter: '#E6F2FF',
  light: '#80CCFF',
  main: '#0066FF', // Azul Owaypay
  dark: '#0052CC',
  darker: '#003D99',
  contrastText: '#FFFFFF',
};

const SECONDARY = {
  lighter: '#F0E6FF',
  light: '#B399FF',
  main: '#6633FF',
  dark: '#4D1A99',
  darker: '#330066',
  contrastText: '#FFFFFF',
};

const INFO = {
  lighter: '#CAFDF5',
  light: '#61F3F3',
  main: '#00B8D9',
  dark: '#006C9C',
  darker: '#003768',
  contrastText: '#FFFFFF',
};

// Owaypay Success - Verde vibrante
const SUCCESS = {
  lighter: '#E6F9F0',
  light: '#80D9B3',
  main: '#00B050',
  dark: '#008040',
  darker: '#005030',
  contrastText: '#FFFFFF',
};

// Owaypay Warning - Laranja
const WARNING = {
  lighter: '#FFF4E6',
  light: '#FFD699',
  main: '#FF9500',
  dark: '#CC7700',
  darker: '#994D00',
  contrastText: GREY[800],
};

// Owaypay Error - Vermelho
const ERROR = {
  lighter: '#FFE6E8',
  light: '#FF9999',
  main: '#E81B23',
  dark: '#B31418',
  darker: '#800D0F',
  contrastText: '#FFFFFF',
};

const COMMON = {
  common: { black: '#000000', white: '#FFFFFF' },
  primary: PRIMARY,
  secondary: SECONDARY,
  info: INFO,
  success: SUCCESS,
  warning: WARNING,
  error: ERROR,
  grey: GREY,
  divider: alpha(GREY[500], 0.24),
  action: {
    hover: alpha(GREY[500], 0.08),
    selected: alpha(GREY[500], 0.16),
    disabled: alpha(GREY[500], 0.8),
    disabledBackground: alpha(GREY[500], 0.24),
    focus: alpha(GREY[500], 0.24),
    hoverOpacity: 0.08,
    disabledOpacity: 0.48,
  },
};

export default function palette(themeMode: 'light' | 'dark') {
  const light = {
    ...COMMON,
    mode: 'light',
    text: {
      primary: GREY[800],
      secondary: GREY[600],
      disabled: GREY[500],
    },
    background: { paper: '#FFFFFF', default: '#FFFFFF', neutral: GREY[200] },
    action: {
      ...COMMON.action,
      active: GREY[600],
    },
  } as const;

  const dark = {
    ...COMMON,
    mode: 'dark',
    text: {
      primary: '#FFFFFF',
      secondary: GREY[500],
      disabled: GREY[600],
    },
    background: {
      paper: GREY[800],
      default: GREY[900],
      neutral: alpha(GREY[500], 0.16),
    },
    action: {
      ...COMMON.action,
      active: GREY[500],
    },
  } as const;

  return themeMode === 'light' ? light : dark;
}
