import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormularioBusqueda, FiltrosBusqueda } from '../../components/formulario-busqueda/formulario-busqueda';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [FormularioBusqueda],
  templateUrl: './home.html',
  styleUrls: ['./home.css']
})
export class Home {
  constructor(private router: Router) { }

  // metodo para navegar a la pagina de animales con filtros
  buscar(filtros: FiltrosBusqueda): void {
    this.router.navigate(['/animales'], {
      queryParams: {
        tipo: filtros.tipo !== 'todos' ? filtros.tipo : null,
        provincia: filtros.provincia !== 'todos' ? filtros.provincia : null
      }
    });
  }
}

