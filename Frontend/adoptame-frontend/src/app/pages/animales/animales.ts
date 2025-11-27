import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AnimalesService } from '../../services/animales.service';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TarjetaAnimales } from '../../components/tarjeta-animales/tarjeta-animales';
import { NgxPaginationModule } from 'ngx-pagination';
import { AnimalDto, AnimalesResponse } from '../../models/interfaces';
import { FormularioBusqueda, FiltrosBusqueda } from '../../components/formulario-busqueda/formulario-busqueda';
import { delay } from 'rxjs/operators';
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

  // inicializa suscripcion a cambios en query params
  ngOnInit() {
    this.route.queryParams.subscribe((params) => {
      const tipo = params['tipo'] || 'todos';
      const provincia = params['provincia'] || 'todos';
      const protectoraUuid = params['protectoraUuid'] || null;
      this.currentPage = Number(params['page']) || 1;

      this.cargarAnimales(tipo, provincia, protectoraUuid);
    });
  }

  // obtiene animales segun filtros y pagina
  cargarAnimales(
    tipo: string = 'todos',
    provincia: string = 'todos',
    protectoraUuid: string | null = null
  ) {
    this.loading = true;

    this.animalService.getAnimales(
      this.currentPage,
      this.pageSize,
      tipo,
      provincia,
      protectoraUuid
    )
      .pipe(delay(1000))
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

  // cambia de pagina y actualiza filtros en la url
  onPageChange(page: number) {
    this.currentPage = page;
    const currentFilters = this.route.snapshot.queryParams;

    const queryParams: any = {
      tipo: currentFilters['tipo'] || 'todos',
      provincia: currentFilters['provincia'] || 'todos',
      page: page
    };

    if (currentFilters['protectoraUuid']) {
      queryParams.protectoraUuid = currentFilters['protectoraUuid'];
    }

    this.router.navigate([], {
      relativeTo: this.route,
      queryParams,
      queryParamsHandling: 'merge'
    });

    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  // aplica nuevos filtros de busqueda y reinicia pagina
  buscar(filtros: FiltrosBusqueda) {
    const queryParams: any = {
      tipo: filtros.tipo !== 'todos' ? filtros.tipo : null,
      provincia: filtros.provincia !== 'todos' ? filtros.provincia : null,
      page: 1
    };
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams
    });
  }
}
