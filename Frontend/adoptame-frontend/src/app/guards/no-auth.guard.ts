import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const noAuthGuard = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // si ya esta autenticado, envia a home y bloquea el acceso
  if (authService.isLoggedIn()) {
    router.navigate(['/home']);
    return false;
  }

  // permite acceso si no esta autenticado
  return true;
};
