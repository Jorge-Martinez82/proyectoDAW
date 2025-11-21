import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProtectorasResponse, ProtectoraDto } from '../../models/interfaces';

@Injectable({
  providedIn: 'root'
})
export class ProtectorasService {
  private apiUrl = `https://localhost:7165/api/Protectoras`;

  constructor(private http: HttpClient) { }

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

  getProtectoraById(uuid: string): Observable<ProtectoraDto> {
    return this.http.get<ProtectoraDto>(`${this.apiUrl}/${uuid}`);
  }

  getPerfilProtectora(): Observable<ProtectoraDto> {
    return this.http.get<ProtectoraDto>(`${this.apiUrl}/me`);
  }

  updateProtectora(datos: ProtectoraDto): Observable<ProtectoraDto> {
    return this.http.put<ProtectoraDto>(`${this.apiUrl}/me`, datos);
  }
}
