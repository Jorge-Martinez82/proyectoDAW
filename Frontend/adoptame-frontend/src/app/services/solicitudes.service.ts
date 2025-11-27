import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SolicitudDto, CrearSolicitudRequest } from '../models/interfaces';
import { AuthService } from './auth.service';
import { API_BASE_URL } from '../config';

@Injectable({
  providedIn: 'root'
})
export class SolicitudesService {
  private apiUrl = `${API_BASE_URL}Solicitudes`;

  constructor(private http: HttpClient, private auth: AuthService) {}

  // construye encabezados con token si existe
  private authHeaders(): HttpHeaders | undefined {
    const token = this.auth.getToken();
    return token ? new HttpHeaders({ 'Authorization': `Bearer ${token}` }) : undefined;
  }

  // crea una nueva solicitud de adopcion
  crearSolicitud(data: CrearSolicitudRequest): Observable<SolicitudDto> {
    return this.http.post<SolicitudDto>(this.apiUrl, data, { headers: this.authHeaders() });
  }

  // obtiene solicitudes del adoptante autenticado
  getSolicitudesAdoptante(pageNumber: number = 1, pageSize: number = 10): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/adoptante?pageNumber=${pageNumber}&pageSize=${pageSize}`, { headers: this.authHeaders() });
  }

  // obtiene solicitudes de la protectora autenticada
  getSolicitudesProtectora(pageNumber: number = 1, pageSize: number = 10): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/protectora?pageNumber=${pageNumber}&pageSize=${pageSize}`, { headers: this.authHeaders() });
  }

  // actualiza el estado de una solicitud
  actualizarEstado(id: number, estado: string): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}/estado`, { estado }, { headers: this.authHeaders() });
  }
}
