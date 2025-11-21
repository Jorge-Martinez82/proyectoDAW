import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, AbstractControl } from '@angular/forms';
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
  errorMessage = '';
  loading = false;

  constructor() {
    this.registroForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [
        Validators.required,
        Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$/)
      ]],
      tipoUsuario: ['', Validators.required],

      nombre: [''],
      apellidos: [''],
      codigoPostal: [''],
      poblacion: [''],
 
      direccion: [''],
      provincia: [''],
      telefono: [''],
 
      nombreProtectora: ['']
    });

    this.registroForm.get('tipoUsuario')?.valueChanges.subscribe(tipo => {
      this.actualizarValidaciones(tipo);
    });
  }

  get esAdoptante(): boolean {
    return this.registroForm.get('tipoUsuario')?.value === 'Adoptante';
  }
  get esProtectora(): boolean {
    return this.registroForm.get('tipoUsuario')?.value === 'Protectora';
  }

  private setValidators(ctrl: AbstractControl | null, validators: any[] = []): void {
    if (!ctrl) return;
    ctrl.setValidators(validators);
    ctrl.updateValueAndValidity({ emitEvent: false });
  }

  private limpiarCamposNoAplicables(): void {
    if (this.esProtectora) {
      ['nombre', 'apellidos', 'codigoPostal', 'poblacion'].forEach(c => {
        const ctrl = this.registroForm.get(c);
        if (ctrl) {
          ctrl.setValue('');
          ctrl.markAsPristine();
          ctrl.markAsUntouched();
        }
      });
    } else if (this.esAdoptante) {
      const protectoraCtrl = this.registroForm.get('nombreProtectora');
      if (protectoraCtrl) {
        protectoraCtrl.setValue('');
        protectoraCtrl.markAsPristine();
        protectoraCtrl.markAsUntouched();
      }
    }
  }

  actualizarValidaciones(tipo: string): void {

    this.setValidators(this.registroForm.get('direccion'), [Validators.required]);
    this.setValidators(this.registroForm.get('provincia'), [Validators.required]);
    this.setValidators(this.registroForm.get('telefono'), [Validators.required]);

    if (tipo === 'Adoptante') {
      this.setValidators(this.registroForm.get('nombre'), [Validators.required, Validators.pattern('[A-Za-zÁÉÍÓÚáéíóúÑñ\\s]{1,25}')]);
      this.setValidators(this.registroForm.get('apellidos'), [Validators.required, Validators.pattern('[A-Za-zÁÉÍÓÚáéíóúÑñ\\s]{1,50}')]);
      this.setValidators(this.registroForm.get('codigoPostal'), []);
      this.setValidators(this.registroForm.get('poblacion'), []);
      this.setValidators(this.registroForm.get('nombreProtectora'), []);
    } else if (tipo === 'Protectora') {
      this.setValidators(this.registroForm.get('nombreProtectora'), [Validators.required]);

      this.setValidators(this.registroForm.get('nombre'), []);
      this.setValidators(this.registroForm.get('apellidos'), []);
      this.setValidators(this.registroForm.get('codigoPostal'), []);
      this.setValidators(this.registroForm.get('poblacion'), []);
    }

    this.limpiarCamposNoAplicables();
  }

  onSubmit(): void {
    if (this.registroForm.invalid) {
      this.registroForm.markAllAsTouched();
      return;
    }
    this.loading = true;
    this.errorMessage = '';

    const v = this.registroForm.value;
    const payload: RegistroRequest = {
      email: v.email,
      password: v.password,
      tipoUsuario: v.tipoUsuario,
      direccion: v.direccion || '',
      codigoPostal: this.esAdoptante ? v.codigoPostal || '' : undefined,
      poblacion: this.esAdoptante ? v.poblacion || '' : undefined,
      provincia: v.provincia || '',
      telefono: v.telefono || ''
    };

    if (this.esAdoptante) {
      payload.nombre = v.nombre;
      payload.apellidos = v.apellidos;
    } else if (this.esProtectora) {
      payload.nombreProtectora = v.nombreProtectora;
    }

    this.authService.registro(payload).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigateByUrl('/login');
      },
      error: err => {
        this.loading = false;
        this.errorMessage = err.error?.mensaje || 'Error al registrar. El email puede estar en uso.';
      }
    });
  }

  campoInvalido(campo: string): boolean {
    const control = this.registroForm.get(campo);
    return !!(control && control.invalid && control.touched);
  }
}
