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

  crearSolicitud(data: CrearSolicitudRequest): Observable<SolicitudDto> {
    const token = this.auth.getToken();
    const headers = token ? new HttpHeaders({ 'Authorization': `Bearer ${token}` }) : undefined;
    return this.http.post<SolicitudDto>(this.apiUrl, data, { headers });
  }

  getSolicitudesAdoptante(pageNumber: number = 1, pageSize: number = 10): Observable<any> {
    const token = this.auth.getToken();
    const headers = token ? new HttpHeaders({ 'Authorization': `Bearer ${token}` }) : undefined;
    return this.http.get<any>(`${this.apiUrl}/adoptante?pageNumber=${pageNumber}&pageSize=${pageSize}`, { headers });
  }

  getSolicitudesProtectora(pageNumber: number = 1, pageSize: number = 10): Observable<any> {
    const token = this.auth.getToken();
    const headers = token ? new HttpHeaders({ 'Authorization': `Bearer ${token}` }) : undefined;
    return this.http.get<any>(`${this.apiUrl}/protectora?pageNumber=${pageNumber}&pageSize=${pageSize}`, { headers });
  }

  actualizarEstado(id: number, estado: string): Observable<void> {
    const token = this.auth.getToken();
    const headers = token ? new HttpHeaders({ 'Authorization': `Bearer ${token}` }) : undefined;
    return this.http.put<void>(`${this.apiUrl}/${id}/estado`, { estado }, { headers });
  }
}
