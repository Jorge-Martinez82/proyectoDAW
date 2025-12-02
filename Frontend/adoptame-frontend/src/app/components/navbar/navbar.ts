import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink, RouterLinkActive, CommonModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar {
  authService = inject(AuthService);

  // estado del menú móvil
  isMenuOpen = false;

  // alterna el menú móvil
  toggleMenu(): void {
    this.isMenuOpen = !this.isMenuOpen;
  }

  // cierra el menú móvil
  closeMenu(): void {
    this.isMenuOpen = false;
  }

  // cierra sesión del usuario
  logout(): void {
    if (window.confirm('¿Estás seguro de que quieres cerrar sesión?')) {
      this.authService.logout();
      this.closeMenu();
    }
  }
}
