import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AdoptanteDto } from '../models/interfaces';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdoptantesService {
  private baseUrl = 'https://localhost:7165/api/Adoptantes';

  constructor(private http: HttpClient) { }

  getPerfil(): Observable<AdoptanteDto> {
    return this.http.get<AdoptanteDto>(`${this.baseUrl}/me`);
  }

  actualizarPerfil(datos: AdoptanteDto): Observable<AdoptanteDto> {
    return this.http.put<AdoptanteDto>(`${this.baseUrl}/me`, datos);
  }
}
