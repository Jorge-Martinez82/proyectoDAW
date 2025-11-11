import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AnimalDto, AnimalesResponse } from '../../models/interfaces';

@Injectable({
  providedIn: 'root'
})
export class AnimalesService {
  private apiUrl = `https://localhost:7165/api/Animales`;

  constructor(private http: HttpClient) { }

  getAnimales(
    pageNumber: number = 1,
    pageSize: number = 12,
    tipo?: string,
    provincia?: string,
    protectoraId?: number
  ): Observable<AnimalesResponse> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (tipo && tipo !== 'todos') {
      params = params.set('tipo', tipo);
    }

    if (provincia && provincia !== 'todos') {
      params = params.set('provincia', provincia);
    }

    if (protectoraId) {
      params = params.set('protectoraId', protectoraId.toString());
    }

    return this.http.get<AnimalesResponse>(this.apiUrl, { params });
  }

  getAnimalById(uuid: string): Observable<AnimalDto> {
    return this.http.get<AnimalDto>(`${this.apiUrl}/${uuid}`);
  }
}
