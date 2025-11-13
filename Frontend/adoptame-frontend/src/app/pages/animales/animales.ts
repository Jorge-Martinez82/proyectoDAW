import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AnimalesService } from './animales.service';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TarjetaAnimales } from '../../components/tarjeta-animales/tarjeta-animales';
import { NgxPaginationModule } from 'ngx-pagination';
import { AnimalDto, AnimalesResponse } from '../../models/interfaces';
import { FormularioBusqueda, FiltrosBusqueda } from '../../components/formulario-busqueda/formulario-busqueda';
import { delay } from 'rxjs/operators'; // ✅ Importa delay
import { Spinner } from '../../components/spinner/spinner';

@Component({
  selector: 'app-animales',
  imports: [CommonModule, FormsModule, TarjetaAnimales, NgxPaginationModule, FormularioBusqueda, Spinner],
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

  constructor(
    private animalService: AnimalesService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit() {
    this.route.queryParams.subscribe((params) => {
      const tipo = params['tipo'] || 'todos';
      const provincia = params['provincia'] || 'todos';
      this.currentPage = Number(params['page']) || 1;

      this.cargarAnimales(tipo, provincia);
    });
  }

  cargarAnimales(tipo: string = 'todos', provincia: string = 'todos') {
    this.loading = true;

    this.animalService.getAnimales(
      this.currentPage,
      this.pageSize,
      tipo,
      provincia
    )
      .pipe(
        delay(1000) 
      )
      .subscribe({
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

  onPageChange(page: number) {
    this.currentPage = page;

    const currentFilters = this.route.snapshot.queryParams;

    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        tipo: currentFilters['tipo'] || 'todos',
        provincia: currentFilters['provincia'] || 'todos',
        page: page
      },
      queryParamsHandling: 'merge'
    });

    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  buscar(filtros: FiltrosBusqueda) {
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        tipo: filtros.tipo !== 'todos' ? filtros.tipo : null,
        provincia: filtros.provincia !== 'todos' ? filtros.provincia : null,
        page: 1
      }
    });
  }
}
