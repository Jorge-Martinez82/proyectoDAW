import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AdoptanteDto } from '../models/interfaces';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../config';

@Injectable({
  providedIn: 'root'
})
export class PerfilService {
  private baseUrl = `${API_BASE_URL}Adoptantes`;

  constructor(private http: HttpClient) { }

  // obtiene el perfil del adoptante autenticado
  getPerfil(): Observable<AdoptanteDto> {
    return this.http.get<AdoptanteDto>(`${this.baseUrl}/me`);
  }

  // actualiza el perfil del adoptante autenticado
  actualizarPerfil(datos: AdoptanteDto): Observable<AdoptanteDto> {
    return this.http.put<AdoptanteDto>(`${this.baseUrl}/me`, datos);
  }
}
