import { Component, OnInit } from '@angular/core';
import { AdoptantesService } from '../../services/perfil.service';
import { AdoptanteDto } from '../../models/interfaces';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.html',
  standalone: true,
  imports: [CommonModule]
})
export class Perfil implements OnInit {
  pestanaSeleccionada: string = 'datosPersonales';
  usuario: AdoptanteDto | null = null;
  cargando: boolean = false;
  error: string | null = null;

  constructor(private adoptantesService: AdoptantesService) { }

  ngOnInit(): void {
    this.cargarPerfil();
  }

  cargarPerfil() {
    this.cargando = true;
    this.adoptantesService.getPerfil().subscribe({
      next: (datos) => {
        this.usuario = datos;
        this.cargando = false;
      },
      error: (err) => {
        this.error = 'No se pudo cargar el perfil.';
        this.cargando = false;
      }
    });
  }

  seleccionarPestana(pestana: string): void {
    this.pestanaSeleccionada = pestana;
  }

  actualizarDatos(): void {
    if (this.usuario) {
      this.adoptantesService.actualizarPerfil(this.usuario).subscribe({
        next: (actualizado) => {
          alert('Datos actualizados correctamente');
          this.usuario = actualizado;
        },
        error: () => alert('Error al actualizar los datos')
      });
    }
  }
}
