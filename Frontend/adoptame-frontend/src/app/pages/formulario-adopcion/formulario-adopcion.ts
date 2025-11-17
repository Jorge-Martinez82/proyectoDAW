import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { AnimalesService } from '../animales/animales.service';
import { AnimalDto } from '../../models/interfaces';
import { BotonVolver } from '../../components/boton-volver/boton-volver';
import { BotonPrincipal } from '../../components/boton-principal/boton-principal';
import { Spinner } from '../../components/spinner/spinner';
import { delay } from 'rxjs/operators';


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

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private animalService: AnimalesService
  ) { }

  ngOnInit(): void {
    this.inicializarFormulario();
    const uuidAnimal = this.route.snapshot.queryParamMap.get('uuid');

    if (uuidAnimal) {
      this.animalService.getAnimalById(uuidAnimal)
              .pipe(
                delay(1000)
              )
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
      nombre: ['', [Validators.required, Validators.pattern('[A-Za-zÁÉÍÓÚáéíóúÑñ\\s]{1,50}')]],
      apellidos: ['', [Validators.required, Validators.pattern('[A-Za-zÁÉÍÓÚáéíóúÑñ\\s]{1,50}')]],
      dni: ['', [Validators.required, Validators.pattern('[0-9]{8}[A-Za-z]{1}')]],
      direccion: ['', [Validators.required, Validators.pattern('[A-Za-z0-9ÁÉÍÓÚáéíóúÑñ\\s]{1,100}')]],
      localidad: ['', [Validators.required]],
      codigoPostal: ['', [Validators.required, Validators.pattern('[0-9]{5}')]],
      provincia: ['', [Validators.required]],
      mayorEdad: [false, [Validators.requiredTrue]]
    });
  }

  validarDNI(): boolean {
    const dniControl = this.formulario.get('dni');
    if (!dniControl || !dniControl.value) return false;

    const dni = dniControl.value.toUpperCase();
    const letras = 'TRWAGMYFPDXBNJZSQVHLCKE';
    const numero = parseInt(dni.substr(0, 8));
    const letra = dni.substr(8, 1);

    if (letras.charAt(numero % 23) !== letra) {
      dniControl.setErrors({ dniInvalido: true });
      return false;
    }

    return true;
  }

  onSubmit(): void {
    if (this.formulario.valid && this.validarDNI() && this.animal) {
      const solicitud = {
        ...this.formulario.value,
        animalUuid: this.animal.uuid
      };

    //llamada al back

      alert('Solicitud enviada con éxito');
    } else {
      this.formulario.markAllAsTouched();
      alert('Por favor, completa todos los campos correctamente');
    }
  }

  campoInvalido(campo: string): boolean {
    const control = this.formulario.get(campo);
    return !!(control && control.invalid && control.touched);
  }
}
