import { Component, OnInit } from '@angular/core';
import { AdoptantesService } from '../../services/perfil.service';
import { AdoptanteDto } from '../../models/interfaces';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { BotonPrincipal } from '../../components/boton-principal/boton-principal';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.html',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, BotonPrincipal]
})
export class Perfil implements OnInit {
  pestanaSeleccionada: string = 'datosPersonales';
  usuario: AdoptanteDto | null = null;
  cargando: boolean = false;
  error: string | null = null;
  form!: FormGroup;
  guardando: boolean = false;

  constructor(private adoptantesService: AdoptantesService, private fb: FormBuilder) { }

  ngOnInit(): void {
    this.inicializarFormulario();
    this.cargarPerfil();
  }

  inicializarFormulario(): void {
    this.form = this.fb.group({
      nombre: ['', [Validators.required, Validators.maxLength(50)]],
      apellidos: ['', [Validators.required, Validators.maxLength(80)]],
      direccion: ['', [Validators.required, Validators.maxLength(120)]],
      codigoPostal: ['', [Validators.required, Validators.pattern('^[0-9]{5}$')]],
      poblacion: ['', [Validators.required, Validators.maxLength(80)]],
      provincia: ['', [Validators.required, Validators.maxLength(80)]],
      telefono: ['', [Validators.required, Validators.pattern('^[0-9+\\s-]{7,20}$')]],
      email: ['', [Validators.required, Validators.email]]
    });
  }

  cargarPerfil() {
    this.cargando = true;
    this.adoptantesService.getPerfil().subscribe({
      next: (datos: AdoptanteDto) => {
        this.usuario = datos;
        this.form.patchValue({
          nombre: datos.nombre || '',
          apellidos: datos.apellidos || '',
          direccion: datos.direccion || '',
          codigoPostal: datos.codigoPostal || '',
          poblacion: datos.poblacion || '',
          provincia: datos.provincia || '',
          telefono: datos.telefono || '',
          email: datos.email || ''
        });
        this.cargando = false;
      },
      error: () => {
        this.error = 'No se pudo cargar el perfil.';
        this.cargando = false;
      }
    });
  }

  campoInvalido(campo: string): boolean {
    const c = this.form.get(campo);
    return !!(c && c.invalid && (c.dirty || c.touched));
  }

  seleccionarPestana(pestana: string): void {
    this.pestanaSeleccionada = pestana;
  }

  actualizarDatos(): void {
    if (this.form.invalid || !this.usuario) {
      this.form.markAllAsTouched();
      return;
    }
    this.guardando = true;
    // Construir AdoptanteDto completo para PUT
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
        alert('Datos actualizados correctamente');
      },
      error: () => {
        this.guardando = false;
        alert('Error al actualizar los datos');
      }
    });
  }
}
