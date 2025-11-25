import { Component, OnInit } from '@angular/core';
import { AdoptantesService } from '../../services/perfil.service';
import { AdoptanteDto, ProtectoraDto, SolicitudDto, AnimalDto } from '../../models/interfaces';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { BotonPrincipal } from '../../components/boton-principal/boton-principal';
import { AuthService } from '../../services/auth.service';
import { ProtectorasService } from '../../services/protectoras.service';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { SolicitudesService } from '../../services/solicitudes.service';
import { AnimalesService } from '../../services/animales.service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.html',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, BotonPrincipal, RouterLink]
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

  solicitudes: (SolicitudDto & { animal?: AnimalDto })[] = [];
  solicitudesCargando = false;
  solicitudesError: string | null = null;

  animales: AnimalDto[] = [];
  animalesCargando = false;
  animalesError: string | null = null;

  constructor(
    private adoptantesService: AdoptantesService,
    private protectorasService: ProtectorasService,
    private authService: AuthService,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private solicitudesService: SolicitudesService,
    private animalesService: AnimalesService
  ) { }

  ngOnInit(): void {
    this.rol = this.authService.getUserRole();
    this.inicializarFormulario();
    const tab = this.route.snapshot.queryParamMap.get('tab');
    if (tab) {
      this.pestanaSeleccionada = tab;
    }
    this.route.queryParamMap.subscribe(params => {
      const t = params.get('tab');
      if (t && t !== this.pestanaSeleccionada) {
        this.pestanaSeleccionada = t;
        if (t === 'solicitudes') this.cargarSolicitudes();
        if (t === 'animales') this.cargarAnimalesProtectora();
      }
    });
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
          if (this.pestanaSeleccionada === 'solicitudes') this.cargarSolicitudes();
        },
        error: () => {
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
          if (this.pestanaSeleccionada === 'solicitudes') this.cargarSolicitudes();
          if (this.pestanaSeleccionada === 'animales') this.cargarAnimalesProtectora();
        },
        error: () => {
          this.error = 'No se pudo cargar el perfil de la protectora.';
          this.cargando = false;
        }
      });
    } else {
      this.error = 'Rol de usuario desconocido.';
      this.cargando = false;
    }
  }

  cargarSolicitudes() {
    this.solicitudesError = null;
    if (this.rol === 'Adoptante') {
      this.cargarSolicitudesAdoptante();
    } else if (this.rol === 'Protectora') {
      this.cargarSolicitudesProtectora();
    }
  }

  cargarSolicitudesAdoptante() {
    this.solicitudesCargando = true;
    this.solicitudesService.getSolicitudesAdoptante().subscribe({
      next: (res) => {
        const base = (res.data || []) as SolicitudDto[];
        this.enriquecerConAnimales(base);
      },
      error: () => {
        this.solicitudes = [];
        this.solicitudesCargando = false;
        this.solicitudesError = 'No se pudieron cargar las solicitudes.';
      }
    });
  }

  cargarSolicitudesProtectora() {
    this.solicitudesCargando = true;
    this.solicitudesService.getSolicitudesProtectora().subscribe({
      next: (res) => {
        const base = (res.data || []) as SolicitudDto[];
        this.enriquecerConAnimales(base);
      },
      error: () => {
        this.solicitudes = [];
        this.solicitudesCargando = false;
        this.solicitudesError = 'No se pudieron cargar las solicitudes recibidas.';
      }
    });
  }

  enriquecerConAnimales(base: SolicitudDto[]) {
    this.solicitudes = base.map(s => ({ ...s }));
    const uniqueUuids = [...new Set(base.map(b => b.animalUuid))];
    if (uniqueUuids.length === 0) {
      this.solicitudesCargando = false;
      return;
    }
    forkJoin(uniqueUuids.map(u => this.animalesService.getAnimalById(u))).subscribe({
      next: (animals) => {
        const map = new Map<string, AnimalDto>(animals.map(a => [a.uuid, a]));
        this.solicitudes = this.solicitudes.map(s => ({ ...s, animal: map.get(s.animalUuid) }));
        this.solicitudesCargando = false;
      },
      error: () => {
        this.solicitudesCargando = false;
      }
    });
  }

  cargarAnimalesProtectora() {
    if (this.rol !== 'Protectora' || !this.protectora) return;
    this.animalesCargando = true;
    this.animalesError = null;
    this.animalesService.getAnimales(1, 50, undefined, undefined, this.protectora.uuid).subscribe({
      next: (res) => {
        this.animales = res.data || [];
        this.animalesCargando = false;
      },
      error: () => {
        this.animales = [];
        this.animalesCargando = false;
        this.animalesError = 'No se pudieron cargar los animales.';
      }
    });
  }

  aceptarSolicitud(id: number) {
    this.cambiarEstado(id, 'aceptada');
  }

  rechazarSolicitud(id: number) {
    this.cambiarEstado(id, 'rechazada');
  }

  cambiarEstado(id: number, estado: string) {
    const solicitud = this.solicitudes.find(s => s.id === id);
    if (!solicitud || solicitud.estado !== 'pendiente') return;
    solicitud.estado = 'actualizando';
    this.solicitudesService.actualizarEstado(id, estado).subscribe({
      next: () => {
        solicitud.estado = estado;
      },
      error: () => {
        solicitud.estado = 'pendiente';
        alert('No se pudo actualizar la solicitud');
      }
    });
  }

  campoInvalido(campo: string): boolean {
    const c = this.form.get(campo);
    return !!(c && c.invalid && (c.dirty || c.touched));
  }

  seleccionarPestana(pestana: string): void {
    this.pestanaSeleccionada = pestana;
    if (pestana === 'solicitudes') this.cargarSolicitudes();
    if (pestana === 'animales') this.cargarAnimalesProtectora();
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
        error: () => {
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
        error: () => {
          this.guardando = false;
          alert('Error al actualizar protectora.');
        }
      });
    } else {
      this.guardando = false;
      alert('No hay datos que actualizar.');
    }
  }
}
