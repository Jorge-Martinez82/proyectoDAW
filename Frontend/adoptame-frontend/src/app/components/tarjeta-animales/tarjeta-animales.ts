import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-tarjeta-animales',
  imports: [CommonModule, RouterLink],
  templateUrl: './tarjeta-animales.html',
  styleUrl: './tarjeta-animales.css'
})
export class TarjetaAnimales {
  @Input() animal: any;

  addToFavorites(event: Event) {
    event.preventDefault();
    alert('Añadido a favoritos');
  }

  share(event: Event) {
    event.preventDefault();
    alert('Compartido');
  }
}

