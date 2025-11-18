import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { BotonPrincipal } from '../../components/boton-principal/boton-principal';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, BotonPrincipal],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  private authService = inject(AuthService);
  private router = inject(Router);
  private fb = inject(FormBuilder);

  loginForm: FormGroup;
  errorMessage: string = '';
  loading: boolean = false;

  constructor() {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]]
    });
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      this.loading = true;
      this.errorMessage = '';

      this.authService.login(this.loginForm.value).subscribe({
        next: (response) => {
          console.log('Login exitoso:', response);
          this.loading = false;

          const returnUrl = localStorage.getItem('returnUrl') || '/';
          localStorage.removeItem('returnUrl'); 
          this.router.navigateByUrl(returnUrl);
        },
        error: (error) => {
          console.error('Error en login:', error);
          this.loading = false;

          if (error.status === 401) {
            this.errorMessage = 'Credenciales incorrectas';
          } else if (error.status === 0) {
            this.errorMessage = 'No se pudo conectar con el servidor';
          } else {
            this.errorMessage = 'Error al iniciar sesión. Intenta de nuevo.';
          }
        }
      });
    } else {
      this.loginForm.markAllAsTouched();
    }
  }

  campoInvalido(campo: string): boolean {
    const control = this.loginForm.get(campo);
    return !!(control && control.invalid && control.touched);
  }
}
