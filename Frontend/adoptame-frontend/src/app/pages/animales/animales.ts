import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AnimalesService } from './animales.service';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { TarjetaAnimales } from '../../components/tarjeta-animales/tarjeta-animales';

@Component({
  selector: 'app-animales',
  imports: [CommonModule, FormsModule, TarjetaAnimales],
  templateUrl: './animales.html',
  styleUrl: './animales.css'
})
export class Animales implements OnInit {
  animales: any[] = [];
  filteredAnimales: any[] = [];
  loading = true;
  error: string | null = null;

  filters = {
    tipo: 'todos',
    provincia: 'todos'
  };

  tiposDisponibles = ['perros', 'gatos', 'conejos', 'tortugas'];

  constructor(
    private animalService: AnimalesService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.cargarAnimales();
    this.obtenerFiltrosDeURL();
  }

  cargarAnimales() {
    this.loading = true;
    this.animalService.getAnimales().subscribe({
      next: (response) => {
        this.animales = Array.isArray(response) ? response : response.items || [];
        this.aplicarFiltros();
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar los animales';
        console.error(err);
        this.loading = false;
      }
    });
  }

  obtenerFiltrosDeURL() {
    this.route.queryParams.subscribe((params) => {
      this.filters.tipo = params['tipo'] || 'todos';
      this.filters.provincia = params['provincia'] || 'todos';
      this.aplicarFiltros();
    });
  }

  aplicarFiltros() {
    this.filteredAnimales = this.animales.filter((animal) => {
      const cumpleTipo = this.filters.tipo === 'todos' ||
        this.normalizarTipo(animal.tipo) === this.normalizarTipo(this.filters.tipo);

      return cumpleTipo;
    });
  }

  normalizarTipo(tipo: any): string {
    const tipoMap: any = {
      'perros': 0,
      'gatos': 1,
      'conejos': 2,
      'tortugas': 3
    };
    return tipoMap[tipo?.toString().toLowerCase()] ?? tipo;
  }

  buscar() {
    this.aplicarFiltros();
  }
}
