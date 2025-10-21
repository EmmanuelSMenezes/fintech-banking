import { useEffect } from 'react';
// next
import { useRouter } from 'next/router';
// components
import LoadingScreen from '../components/loading-screen';
//
import { useAuthContext } from './useAuthContext';

// ----------------------------------------------------------------------

type AuthGuardProps = {
  children: React.ReactNode;
};

export default function AuthGuard({ children }: AuthGuardProps) {
  const { isAuthenticated, isInitialized } = useAuthContext();

  const { push } = useRouter();

  useEffect(() => {
    if (!isInitialized) {
      return;
    }

    if (!isAuthenticated) {
      push('/auth/login');
    }
  }, [isAuthenticated, isInitialized, push]);

  if (!isInitialized) {
    return <LoadingScreen />;
  }

  if (!isAuthenticated) {
    return <LoadingScreen />;
  }

  return <> {children} </>;
}
