import { Component, OnInit } from '@angular/core';
import { AdoptantesService } from '../../services/perfil.service';
import { AdoptanteDto, ProtectoraDto } from '../../models/interfaces';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { BotonPrincipal } from '../../components/boton-principal/boton-principal';
import { AuthService } from '../../services/auth.service';
import { ProtectorasService } from '../protectoras/protectoras.service';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.html',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, BotonPrincipal]
})
export class Perfil implements OnInit {
  pestanaSeleccionada: string = 'datosPersonales';
  usuario: AdoptanteDto | null = null;
  protectora: ProtectoraDto | null = null;
  rol: 'Protectora' | 'Adoptante' | null = null;

  cargando = false;
  error: string | null = null;
  form!: FormGroup;
  guardando = false;

  constructor(
    private adoptantesService: AdoptantesService,
    private protectorasService: ProtectorasService,
    private authService: AuthService,
    private fb: FormBuilder
  ) { }

  ngOnInit(): void {
    this.rol = this.authService.getUserRole();
    this.inicializarFormulario();
    this.cargarPerfil();
  }

  inicializarFormulario(): void {
    this.form = this.fb.group({
      nombre: ['', [Validators.required, Validators.maxLength(80)]],
      apellidos: ['', [Validators.maxLength(120)]],
      direccion: ['', [Validators.required, Validators.maxLength(150)]],
      codigoPostal: ['', [Validators.pattern('^[0-9]{5}$')]],
      poblacion: ['', [Validators.maxLength(80)]],
      provincia: ['', [Validators.required, Validators.maxLength(80)]],
      telefono: ['', [Validators.required, Validators.pattern('^[0-9+\\s-]{7,20}$')]],
      email: ['', [Validators.required, Validators.email]]
    });
  }

  cargarPerfil() {
    this.cargando = true;
    if (this.rol === 'Adoptante') {
      this.adoptantesService.getPerfil().subscribe({
        next: (datos: AdoptanteDto) => {
          this.usuario = datos;
          this.form.patchValue({
            nombre: datos.nombre,
            apellidos: datos.apellidos,
            direccion: datos.direccion,
            codigoPostal: datos.codigoPostal,
            poblacion: datos.poblacion,
            provincia: datos.provincia,
            telefono: datos.telefono,
            email: datos.email
          });
          this.cargando = false;
        },
        error: (err) => {
          this.error = 'No se pudo cargar el perfil de adoptante.';
          this.cargando = false;
        }
      });
    } else if (this.rol === 'Protectora') {
      this.protectorasService.getPerfilProtectora().subscribe({
        next: (datos: ProtectoraDto) => {
          this.protectora = datos;
          this.form.patchValue({
            nombre: datos.nombre,
            direccion: datos.direccion,
            provincia: datos.provincia,
            telefono: datos.telefono,
            email: datos.email
          });
          this.form.get('apellidos')?.reset();
          this.form.get('codigoPostal')?.reset();
          this.form.get('poblacion')?.reset();
          this.cargando = false;
        },
        error: (err) => {
          console.error('[Perfil] Error protectora:', err);
          this.error = 'No se pudo cargar el perfil de la protectora.';
          this.cargando = false;
        }
      });
    } else {
      console.warn('[Perfil] Rol desconocido, no se llama a servicios.');
      this.error = 'Rol de usuario desconocido.';
      this.cargando = false;
    }
  }

  campoInvalido(campo: string): boolean {
    const c = this.form.get(campo);
    return !!(c && c.invalid && (c.dirty || c.touched));
  }

  seleccionarPestana(pestana: string): void {
    this.pestanaSeleccionada = pestana;
  }

  actualizarDatos(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.guardando = true;

    if (this.rol === 'Adoptante' && this.usuario) {
      const payload: AdoptanteDto = {
        uuid: this.usuario.uuid,
        nombre: this.form.value.nombre,
        apellidos: this.form.value.apellidos,
        direccion: this.form.value.direccion,
        codigoPostal: this.form.value.codigoPostal,
        poblacion: this.form.value.poblacion,
        provincia: this.form.value.provincia,
        telefono: this.form.value.telefono,
        email: this.form.value.email
      };
      this.adoptantesService.actualizarPerfil(payload).subscribe({
        next: (actualizado: AdoptanteDto) => {
          this.usuario = actualizado;
          this.guardando = false;
          alert('Perfil de adoptante actualizado.');
        },
        error: (err) => {
          this.guardando = false;
          alert('Error al actualizar adoptante.');
        }
      });
    } else if (this.rol === 'Protectora' && this.protectora) {
      const payload: ProtectoraDto = {
        uuid: this.protectora.uuid,
        nombre: this.form.value.nombre,
        direccion: this.form.value.direccion,
        telefono: this.form.value.telefono,
        provincia: this.form.value.provincia,
        email: this.form.value.email,
        imagen: this.protectora.imagen,
        userId: this.protectora.userId,
        createdAt: this.protectora.createdAt
      };
      this.protectorasService.updateProtectora(payload).subscribe({
        next: (actualizada: ProtectoraDto) => {
          this.protectora = actualizada;
          this.guardando = false;
          alert('Protectora actualizada.');
        },
        error: (err) => {
          console.error('[Perfil] Error actualización protectora:', err);
          this.guardando = false;
          alert('Error al actualizar protectora.');
        }
      });
    } else {
      console.warn('[Perfil] Nada que actualizar.');
      this.guardando = false;
      alert('No hay datos que actualizar.');
    }
  }
}
