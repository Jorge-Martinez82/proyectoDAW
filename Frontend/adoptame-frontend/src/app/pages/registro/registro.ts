import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { BotonPrincipal } from '../../components/boton-principal/boton-principal';

@Component({
  selector: 'app-registro',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, BotonPrincipal],
  templateUrl: './registro.html',
  styleUrl: './registro.css'
})
export class Registro {
  private authService = inject(AuthService);
  private router = inject(Router);
  private fb = inject(FormBuilder);

  registroForm: FormGroup;
  errorMessage: string = '';
  loading: boolean = false;

  constructor() {
    this.registroForm = this.fb.group({
      nombre: ['', [
        Validators.required,
        Validators.pattern('[A-Za-zÁÉÍÓÚáéíóúÑñ\\s]{1,25}')
      ]],
      apellidos: ['', [
        Validators.required,
        Validators.pattern('[A-Za-zÁÉÍÓÚáéíóúÑñ\\s]{1,50}')
      ]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [
        Validators.required,
        Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$/)
      ]],
      tipo_usuario: ['', [Validators.required]]
    });
  }

  onSubmit(): void {
    if (this.registroForm.valid) {
      this.loading = true;
      this.errorMessage = '';

      console.log('Datos capturados del formulario:', this.registroForm.value);
      setTimeout(() => {
        this.loading = false;
        console.log('Registro simulado exitoso. Redirigiendo...');
        this.router.navigateByUrl('/login');
      }, 1500);

    } else {
      this.registroForm.markAllAsTouched();
    }
  }

  campoInvalido(campo: string): boolean {
    const control = this.registroForm.get(campo);
    return !!(control && control.invalid && control.touched);
  }
}
