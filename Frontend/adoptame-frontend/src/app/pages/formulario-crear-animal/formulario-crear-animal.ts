import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AnimalesService } from '../../services/animales.service';
import { BotonPrincipal } from '../../components/boton-principal/boton-principal';
import { AuthService } from '../../services/auth.service';
import { ProtectorasService } from '../../services/protectoras.service';
import { BotonVolver } from '../../components/boton-volver/boton-volver';


@Component({
  selector: 'app-formulario-crear-animal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, BotonPrincipal, BotonVolver],
  templateUrl: './formulario-crear-animal.html',
  styleUrl: './formulario-crear-animal.css'
})
export class FormularioCrearAnimal implements OnInit {
  form!: FormGroup;
  enviando = false;
  protectoraUuid: string | null = null;

  tipos = ['Perro', 'Gato', 'Roedor', 'Reptil'];

  constructor(
    private fb: FormBuilder,
    private animalesService: AnimalesService,
    private router: Router,
    private auth: AuthService,
    private protectorasService: ProtectorasService
  ) {}

  // crea el formulario y obtiene uuid de protectora
  ngOnInit(): void {
    this.form = this.fb.group({
      nombre: ['', [Validators.required, Validators.maxLength(80)]],
      tipo: ['', [Validators.required]],
      raza: ['', [Validators.maxLength(60)]],
      edad: [null, []],
      genero: ['', [Validators.required]],
      descripcion: ['', [Validators.maxLength(500)]],
      imagenPrincipal: ['', []]
    });

    this.protectorasService.getPerfilProtectora().subscribe({
      next: (p) => {
        this.protectoraUuid = p ? p.uuid : null;
      },
      error: () => {
        this.protectoraUuid = null;
      }
    });
  }

  // valida y envia datos para crear animal
  onSubmit(): void {
    if (this.form.invalid || !this.protectoraUuid) {
      this.form.markAllAsTouched();
      alert('Formulario inválido');
      return;
    }
    this.enviando = true;
    const dto: any = {
      ...this.form.value,
      uuid: this.protectoraUuid,
      protectoraId: 0
    };
    this.animalesService.createAnimal(dto).subscribe({
      next: () => {
        this.enviando = false;
        alert('Animal creado');
        this.router.navigate(['/perfil'], { queryParams: { tab: 'animales' } });
      },
      error: () => {
        this.enviando = false;
        alert('No se pudo crear el animal');
      }
    });
  }
}
