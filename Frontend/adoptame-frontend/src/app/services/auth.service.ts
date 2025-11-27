import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, BehaviorSubject, tap } from 'rxjs';
import { LoginRequest, LoginResponse, RegistroRequest, UsuarioDto } from '../models/interfaces';
import { API_BASE_URL } from '../config';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);

  private apiUrl = `${API_BASE_URL}Usuarios`;
  private currentUserSubject = new BehaviorSubject<UsuarioDto | null>(this.getUserFromStorage());
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor() { }

  // inicia sesion y guarda token y usuario
  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, credentials)
      .pipe(
        tap(response => {
          localStorage.setItem('token', response.token);
          localStorage.setItem('currentUser', JSON.stringify(response.usuario));
          this.currentUserSubject.next(response.usuario);
        })
      );
  }

  // registra un nuevo usuario
  registro(data: RegistroRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, data);
  }

  // cierra sesion y limpia almacenamiento
  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);
  }

  // verifica si hay token guardado
  isLoggedIn(): boolean {
    return localStorage.getItem('token') !== null;
  }

  // obtiene el usuario actual
  getCurrentUser(): UsuarioDto | null {
    return this.currentUserSubject.value;
  }

  // obtiene el token actual
  getToken(): string | null {
    return localStorage.getItem('token');
  }

  // obtiene el rol del usuario
  getUserRole(): 'Protectora' | 'Adoptante' | null {
    const user: any = this.getCurrentUser();
    if (!user) return null;
    return user.rol || user.tipoUsuario || null;
  }

  // lee el usuario guardado en localstorage
  private getUserFromStorage(): UsuarioDto | null {
    const userJson = localStorage.getItem('currentUser');
    return userJson ? JSON.parse(userJson) : null;
  }
}
