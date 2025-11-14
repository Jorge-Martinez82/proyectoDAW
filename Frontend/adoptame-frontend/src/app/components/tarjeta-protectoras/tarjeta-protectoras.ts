import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { BotonPrincipal } from '../boton-principal/boton-principal';

@Component({
  selector: 'app-tarjeta-protectora',
  imports: [CommonModule, RouterLink, BotonPrincipal],
  templateUrl: './tarjeta-protectoras.html',
  styleUrl: './tarjeta-protectoras.css'
})
export class TarjetaProtectoras {
  @Input() protectora: any;
}
