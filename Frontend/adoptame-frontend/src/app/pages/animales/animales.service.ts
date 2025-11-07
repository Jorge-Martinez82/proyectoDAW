import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AnimalesService {
  private apiUrl = 'https://localhost:7165/api/Animales';

  constructor(private http: HttpClient) { }

  getAnimales(): Observable<any> {
    return this.http.get(`${this.apiUrl}`); 
  }
}
