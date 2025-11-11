import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AnimalesService } from '../animales/animales.service';
import { AnimalDto } from '../../models/interfaces';

@Component({
  selector: 'app-detalle-animal',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './detalle-animal.html',
  styleUrls: ['./detalle-animal.css']
})
export class DetalleAnimal implements OnInit {
  animal: AnimalDto | null = null;
  animalNoEncontrado = false;
  loading = true;
  menuAbierto = false;

  constructor(
    private route: ActivatedRoute,
    private animalService: AnimalesService
  ) { }

  ngOnInit(): void {

    const uuidAnimal = this.route.snapshot.queryParamMap.get('uuid');

    if (uuidAnimal) {
      this.animalService.getAnimalById(uuidAnimal).subscribe({
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

  toggleMenu(): void {
    this.menuAbierto = !this.menuAbierto;
  }

  getImagenPath(imagen: string | null): string {
    if (!imagen) {
      return 'assets/img/default-animal.png';
    }
    return `assets/img/mascotas/${imagen}`;
  }


}
