import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { BotonPrincipal } from '../../components/boton-principal/boton-principal';
import { RegistroRequest } from '../../models/interfaces';

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
      email: ['', [Validators.required, Validators.email]],
      password: ['', [
        Validators.required,
        Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$/)
      ]],
      tipoUsuario: ['', Validators.required],
      nombre: ['', [Validators.required, Validators.pattern('[A-Za-zÁÉÍÓÚáéíóúÑñ\\s]{1,25}')]],
      apellidos: ['', [Validators.required, Validators.pattern('[A-Za-zÁÉÍÓÚáéíóúÑñ\\s]{1,50}')]],
      direccion: ['', Validators.required],
      codigoPostal: ['', Validators.required],
      poblacion: ['', Validators.required],
      provincia: ['', Validators.required],
      telefono: ['', Validators.required],
      nombreProtectora: ['', []]
    });

    this.registroForm.get('tipoUsuario')?.valueChanges.subscribe(tipo => {
      this.actualizarValidaciones(tipo);
    });
  }

  actualizarValidaciones(tipo: string): void {
    const nombreControl = this.registroForm.get('nombre');
    const apellidosControl = this.registroForm.get('apellidos');
    const nombreProtectoraControl = this.registroForm.get('nombreProtectora');

    nombreControl?.clearValidators();
    apellidosControl?.clearValidators();
    nombreProtectoraControl?.clearValidators();

    if (tipo === 'Adoptante') {
      nombreControl?.setValidators([Validators.required, Validators.pattern('[A-Za-zÁÉÍÓÚáéíóúÑñ\\s]{1,25}')]);
      apellidosControl?.setValidators([Validators.required, Validators.pattern('[A-Za-zÁÉÍÓÚáéíóúÑñ\\s]{1,50}')]);
      nombreProtectoraControl?.setValidators([]);
    } else if (tipo === 'Protectora') {
      nombreProtectoraControl?.setValidators([Validators.required]);
      nombreControl?.setValidators([]);
      apellidosControl?.setValidators([]);
    }

    nombreControl?.updateValueAndValidity();
    apellidosControl?.updateValueAndValidity();
    nombreProtectoraControl?.updateValueAndValidity();
  }

  onSubmit(): void {
    if (this.registroForm.valid) {
      this.loading = true;
      this.errorMessage = '';

      const payload: RegistroRequest = this.crearPayload();

      this.authService.registro(payload).subscribe({
        next: () => {
          this.router.navigateByUrl('/login');
        },
        error: err => {
          this.loading = false;
          this.errorMessage = err.error?.message || 'Error al registrar. El email puede estar en uso.';
        },
        complete: () => {
          this.loading = false;
        }
      });
    } else {
      this.registroForm.markAllAsTouched();
    }
  }

  crearPayload(): RegistroRequest {
    const v = this.registroForm.value;
    const data: RegistroRequest = {
      email: v.email,
      password: v.password,
      tipoUsuario: v.tipoUsuario,
      direccion: v.direccion,
      codigoPostal: v.codigoPostal,
      poblacion: v.poblacion,
      provincia: v.provincia,
      telefono: v.telefono
    };
    if (v.tipoUsuario === 'Adoptante') {
      data.nombre = v.nombre;
      data.apellidos = v.apellidos;
    }
    if (v.tipoUsuario === 'Protectora') {
      data.nombreProtectora = v.nombreProtectora;
    }
    return data;
  }

  campoInvalido(campo: string): boolean {
    const control = this.registroForm.get(campo);
    return !!(control && control.invalid && control.touched);
  }

  get esAdoptante(): boolean {
    return this.registroForm.get('tipoUsuario')?.value === 'Adoptante';
  }

  get esProtectora(): boolean {
    return this.registroForm.get('tipoUsuario')?.value === 'Protectora';
  }
}
