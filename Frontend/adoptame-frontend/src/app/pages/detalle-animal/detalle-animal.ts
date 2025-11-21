import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AnimalesService } from '../../services/animales.service';
import { AnimalDto } from '../../models/interfaces';
import { BotonVolver } from '../../components/boton-volver/boton-volver';
import { Spinner } from '../../components/spinner/spinner';
import { delay } from 'rxjs/operators';
import { BotonPrincipal } from '../../components/boton-principal/boton-principal';


@Component({
  selector: 'app-detalle-animal',
  standalone: true,
  imports: [CommonModule, RouterLink, BotonVolver, Spinner, BotonPrincipal],
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

    const uuidAnimal = this.route.snapshot.paramMap.get('uuid');

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

  toggleMenu(): void {
    this.menuAbierto = !this.menuAbierto;
  }
}
