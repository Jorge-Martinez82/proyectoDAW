import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AnimalesService } from '../../services/animales.service';
import { AnimalDto, CrearSolicitudRequest } from '../../models/interfaces';
import { BotonVolver } from '../../components/boton-volver/boton-volver';
import { BotonPrincipal } from '../../components/boton-principal/boton-principal';
import { Spinner } from '../../components/spinner/spinner';
import { delay } from 'rxjs/operators';
import { SolicitudesService } from '../../services/solicitudes.service';

@Component({
  selector: 'app-formulario-adopcion',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, BotonVolver, BotonPrincipal, Spinner],
  templateUrl: './formulario-adopcion.html',
  styleUrl: './formulario-adopcion.css'
})
export class FormularioAdopcion implements OnInit {
  formulario!: FormGroup;
  animal: AnimalDto | null = null;
  loading = true;
  animalNoEncontrado = false;
  enviando = false;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private animalService: AnimalesService,
    private solicitudesService: SolicitudesService
  ) { }

  ngOnInit(): void {
    this.inicializarFormulario();
    const uuidAnimal = this.route.snapshot.queryParamMap.get('uuid');

    if (uuidAnimal) {
      this.animalService.getAnimalById(uuidAnimal)
        .pipe(delay(500))
        .subscribe({
          next: (response) => {
            this.animal = response;
            this.loading = false;
          },
          error: (error) => {
            console.error('Error al obtener datos del animal:', error);
            this.animalNoEncontrado = true;
            this.loading = false;
          }
        });
    } else {
      this.animalNoEncontrado = true;
      this.loading = false;
    }
  }

  inicializarFormulario(): void {
    this.formulario = this.fb.group({
      comentario: ['', [Validators.required, Validators.maxLength(500)]],
    });
  }

  onSubmit(): void {
    if (this.formulario.invalid || !this.animal) {
      this.formulario.markAllAsTouched();
      alert('Formulario inválido');
      return;
    }

    const payload: CrearSolicitudRequest = {
      animalUuid: this.animal.uuid,
      comentario: this.formulario.value.comentario
    };

    this.enviando = true;
    this.solicitudesService.crearSolicitud(payload).subscribe({
      next: (solicitud) => {
        this.enviando = false;
        alert('Solicitud enviada. Estado inicial: ' + solicitud.estado);
        this.formulario.reset();
        this.router.navigate(['/perfil'], { queryParams: { tab: 'solicitudes' } });
      },
      error: (err) => {
        console.error('Error creando solicitud', err);
        this.enviando = false;
        alert('No se pudo enviar la solicitud');
      }
    });
  }

  campoInvalido(campo: string): boolean {
    const control = this.formulario.get(campo);
    return !!(control && control.invalid && control.touched);
  }
}
