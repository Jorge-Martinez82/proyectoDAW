import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProtectorasService } from '../../services/protectoras.service';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TarjetaProtectoras } from '../../components/tarjeta-protectoras/tarjeta-protectoras';
import { NgxPaginationModule } from 'ngx-pagination';
import { ProtectoraDto, ProtectorasResponse } from '../../models/interfaces';
import { FormularioBusqueda, FiltrosBusqueda } from '../../components/formulario-busqueda/formulario-busqueda';
import { delay } from 'rxjs/operators';
import { Spinner } from '../../components/spinner/spinner';

@Component({
  selector: 'app-protectoras',
  imports: [CommonModule, FormsModule, TarjetaProtectoras, NgxPaginationModule, FormularioBusqueda, Spinner],
  templateUrl: './protectoras.html',
  styleUrl: './protectoras.css'
})
export class Protectoras implements OnInit {
  protectoras: ProtectoraDto[] = [];
  loading = true;
  error: string | null = null;

  currentPage = 1;
  pageSize = 12;
  totalCount = 0;

  constructor(
    private protectorasService: ProtectorasService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit() {
    this.route.queryParams.subscribe((params) => {
      const provincia = params['provincia'] || 'todos';
      this.currentPage = Number(params['page']) || 1;

      this.cargarProtectoras(provincia);
    });
  }

  cargarProtectoras(provincia: string = 'todos') {
    this.loading = true;

    this.protectorasService.getProtectoras(
      this.currentPage,
      this.pageSize,
      provincia
    )
      .pipe(
        delay(1000)
      )
      .subscribe({
        next: (response: ProtectorasResponse) => {
          this.protectoras = response.data || [];
          this.totalCount = response.totalCount || 0;
          this.loading = false;
        },
        error: (err) => {
          this.error = 'Error al cargar las protectoras';
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
        provincia: filtros.provincia !== 'todos' ? filtros.provincia : null,
        page: 1
      }
    });
  }
}
