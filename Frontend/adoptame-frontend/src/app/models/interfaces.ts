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
