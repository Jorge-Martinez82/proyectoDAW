export interface AnimalDto {
  uuid: string;
  nombre: string | null;
  tipo: string | null;
  raza: string | null;
  edad: number | null;
  genero: string | null;
  descripcion: string | null;
  protectoraId: number;
  imagenPrincipal: string | null;
  createdAt: Date;
}

export interface AnimalesResponse {
  data: AnimalDto[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
}

export interface ProtectoraDto {
  uuid: string;
  nombre: string;
  direccion: string | null;
  telefono: string | null;
  provincia: string | null;
  email: string | null;
  imagen: string | null;
  userId: number;
  createdAt: string;
}

export interface ProtectorasResponse {
  data: ProtectoraDto[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}
export interface SolicitudAdopcion {
  nombre: string;
  apellidos: string;
  dni: string;
  direccion: string;
  localidad: string;
  codigoPostal: string;
  provincia: string;
  mayorEdad: boolean;
  animalUuid: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  usuario: UsuarioDto;
}

export interface UsuarioDto {
  uuid: string;
  email: string;
  nombre: string;
  rol: 'Protectora' | 'Adoptante';
  protectoraId?: number;
}

export interface RegistroRequest {
  email: string;
  password: string;
  tipoUsuario: string;
  nombre?: string;
  apellidos?: string;
  direccion?: string;
  codigoPostal?: string;
  poblacion?: string;
  provincia?: string;
  telefono?: string;
  nombreProtectora?: string;
}

export interface AdoptanteDto {
  uuid: string;
  nombre: string;
  apellidos: string;
  direccion: string;
  codigoPostal: string;
  poblacion: string;
  provincia: string;
  telefono: string;
  email: string;
}
