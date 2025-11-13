import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-boton-principal',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './boton-principal.html',
  styleUrls: ['./boton-principal.css']
})
export class BotonPrincipal {
  @Input() texto: string = 'Buscar';
  @Input() disabled: boolean = false;
  @Input() tipo: 'submit' | 'button' | 'link' = 'submit'; 
  @Input() ruta: string | null = null; 
  @Input() queryParams: any = {}; 
}

