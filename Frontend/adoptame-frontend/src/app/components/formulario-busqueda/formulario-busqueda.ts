import { Component, Output, EventEmitter, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BotonPrincipal } from '../boton-principal/boton-principal';

export interface FiltrosBusqueda {
  tipo: string;
  provincia: string;
}

@Component({
  selector: 'app-formulario-busqueda',
  standalone: true,
  imports: [CommonModule, FormsModule, BotonPrincipal],
  templateUrl: './formulario-busqueda.html',
  styleUrls: ['./formulario-busqueda.css']
})
export class FormularioBusqueda {
  @Input() modo: 'home' | 'animales' = 'home';
  @Output() busquedaRealizada = new EventEmitter<FiltrosBusqueda>();

  filtros: FiltrosBusqueda = {
    tipo: 'todos',
    provincia: 'todos'
  };

  buscar(): void {
    this.busquedaRealizada.emit(this.filtros);
  }
}
