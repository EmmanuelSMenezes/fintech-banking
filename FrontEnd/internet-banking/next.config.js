module.exports = {
  swcMinify: false,
  trailingSlash: true,
  reactStrictMode: true,
  env: {
    // FinTech Banking - Internet Banking
    NEXT_PUBLIC_API_URL: process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5036',
    NEXT_PUBLIC_API_INTERNAL: process.env.NEXT_PUBLIC_API_INTERNAL || 'http://localhost:5036',
    NEXT_PUBLIC_APP_NAME: 'FinTech Internet Banking',
    NEXT_PUBLIC_APP_VERSION: '1.0.0',
  },
  publicRuntimeConfig: {
    apiUrl: process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5036',
    apiInternal: process.env.NEXT_PUBLIC_API_INTERNAL || 'http://localhost:5036',
  },
};
