import { inject } from '@angular/core';
import { Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // permite acceso si el usuario esta autenticado
  if (authService.isLoggedIn()) {
    return true;
  }

  // guarda url a la que intentaba acceder para volver luego
  localStorage.setItem('returnUrl', state.url);

  // redirige a la pagina de login
  router.navigate(['/login']);
  return false;
};
