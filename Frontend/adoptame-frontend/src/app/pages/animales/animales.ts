import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AnimalesService } from './animales.service';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { TarjetaAnimales } from '../../components/tarjeta-animales/tarjeta-animales';
import { NgxPaginationModule } from 'ngx-pagination';
import { AnimalDto, AnimalesResponse } from '../../models/interfaces';


@Component({
  selector: 'app-animales',
  imports: [CommonModule, FormsModule, TarjetaAnimales, NgxPaginationModule],
  templateUrl: './animales.html',
  styleUrl: './animales.css'
})
export class Animales implements OnInit {
  animales: AnimalDto[] = [];
  loading = true;
  error: string | null = null;

  currentPage = 1;
  pageSize = 12;
  totalCount = 0;

  filters = {
    tipo: 'todos',
    provincia: 'todos'
  };

  constructor(
    private animalService: AnimalesService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.obtenerFiltrosDeURL();
    this.cargarAnimales();
  }

  cargarAnimales() {
    this.loading = true;

    this.animalService.getAnimales(
      this.currentPage,
      this.pageSize,
      this.filters.tipo,
      this.filters.provincia
    ).subscribe({
      next: (response: AnimalesResponse) => {
        this.animales = response.data || [];
        this.totalCount = response.totalCount || 0;
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
    });
  }

  onPageChange(page: number) {
    this.currentPage = page;
    this.cargarAnimales();
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  buscar() {
    this.currentPage = 1; 
    this.cargarAnimales();
  }
}
