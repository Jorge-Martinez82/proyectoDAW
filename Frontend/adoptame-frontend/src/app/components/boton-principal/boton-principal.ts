import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-boton-principal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './boton-principal.html',
  styleUrls: ['./boton-principal.css']
})
export class BotonPrincipal {
  @Input() texto: string = 'Buscar';
  @Input() disabled: boolean = false;
}

