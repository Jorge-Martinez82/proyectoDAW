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
