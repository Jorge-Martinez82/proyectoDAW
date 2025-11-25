import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SolicitudDto, CrearSolicitudRequest } from '../models/interfaces';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class SolicitudesService {
  private apiUrl = 'https://localhost:7165/api/Solicitudes';

  constructor(private http: HttpClient, private auth: AuthService) {}

  private authHeaders(): HttpHeaders | undefined {
    const token = this.auth.getToken();
    return token ? new HttpHeaders({ 'Authorization': `Bearer ${token}` }) : undefined;
  }

  crearSolicitud(data: CrearSolicitudRequest): Observable<SolicitudDto> {
    return this.http.post<SolicitudDto>(this.apiUrl, data, { headers: this.authHeaders() });
  }

  getSolicitudesAdoptante(pageNumber: number = 1, pageSize: number = 10): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/adoptante?pageNumber=${pageNumber}&pageSize=${pageSize}`, { headers: this.authHeaders() });
  }

  getSolicitudesProtectora(pageNumber: number = 1, pageSize: number = 10): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/protectora?pageNumber=${pageNumber}&pageSize=${pageSize}`, { headers: this.authHeaders() });
  }

  actualizarEstado(id: number, estado: string): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}/estado`, { estado }, { headers: this.authHeaders() });
  }
}
