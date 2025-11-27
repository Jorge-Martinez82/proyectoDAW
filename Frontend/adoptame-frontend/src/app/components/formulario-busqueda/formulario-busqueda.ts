import { Component, Output, EventEmitter, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BotonPrincipal } from '../boton-principal/boton-principal';

export interface FiltrosBusqueda {
  tipo?: string; 
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
  @Input() modo: 'home' | 'animales' | 'protectoras' = 'home';
  @Output() busquedaRealizada = new EventEmitter<FiltrosBusqueda>();

  filtros: FiltrosBusqueda = {
    tipo: 'todos',
    provincia: 'todos'
  };

  provincias = ['Álava', 'Guipúzcoa', 'Vizcaya', 'Navarra'];
  tipos = ['Perro', 'Gato', 'Roedor', 'Reptil'];

  // metodo de busqueda con los filtros seleccionados
  buscar(): void {
    const resultado: FiltrosBusqueda = {
      provincia: this.filtros.provincia
    };

    if (this.modo !== 'protectoras') {
      resultado.tipo = this.filtros.tipo;
    }

    this.busquedaRealizada.emit(resultado);
  }
}
