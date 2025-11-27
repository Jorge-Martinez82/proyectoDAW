import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AnimalDto, AnimalesResponse } from '../models/interfaces';
import { API_BASE_URL } from '../config';

@Injectable({
  providedIn: 'root'
})
export class AnimalesService {
  private apiUrl = `${API_BASE_URL}Animales`;

  constructor(private http: HttpClient) { }

  // obtiene lista de animales con filtros y paginacion
  getAnimales(
    pageNumber: number = 1,
    pageSize: number = 12,
    tipo?: string,
    provincia?: string,
    protectoraUuid?: string | null
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

    if (protectoraUuid) {
      params = params.set('protectoraUuid', protectoraUuid); 
    }

    return this.http.get<AnimalesResponse>(this.apiUrl, { params });
  }

  // obtiene un animal por su uuid
  getAnimalById(uuid: string): Observable<AnimalDto> {
    return this.http.get<AnimalDto>(`${this.apiUrl}/${uuid}`);
  }

  // elimina un animal por su uuid
  deleteAnimal(uuid: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${uuid}`);
  }

  // crea un nuevo animal
  createAnimal(dto: AnimalDto): Observable<AnimalDto> {
    return this.http.post<AnimalDto>(this.apiUrl, dto);
  }
}
