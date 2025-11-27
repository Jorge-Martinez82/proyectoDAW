import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProtectorasResponse, ProtectoraDto } from '../models/interfaces';
import { API_BASE_URL } from '../config';

@Injectable({
  providedIn: 'root'
})
export class ProtectorasService {
  private apiUrl = `${API_BASE_URL}Protectoras`;

  constructor(private http: HttpClient) { }

  // obtiene lista de protectoras con filtros y paginacion
  getProtectoras(
    pageNumber: number = 1,
    pageSize: number = 12,
    provincia: string = 'todos'
  ): Observable<ProtectorasResponse> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (provincia && provincia !== 'todos') {
      params = params.set('provincia', provincia);
    }

    return this.http.get<ProtectorasResponse>(this.apiUrl, { params });
  }

  // obtiene una protectora por su uuid
  getProtectoraById(uuid: string): Observable<ProtectoraDto> {
    return this.http.get<ProtectoraDto>(`${this.apiUrl}/${uuid}`);
  }

  // obtiene el perfil de la protectora autenticada
  getPerfilProtectora(): Observable<ProtectoraDto> {
    return this.http.get<ProtectoraDto>(`${this.apiUrl}/me`);
  }

  // actualiza el perfil de la protectora autenticada
  updateProtectora(datos: ProtectoraDto): Observable<ProtectoraDto> {
    return this.http.put<ProtectoraDto>(`${this.apiUrl}/me`, datos);
  }
}
