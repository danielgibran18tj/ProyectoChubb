export interface Asegurado {
  aseguradoId: number;
  cedula: string;
  nombreCompleto: string;
  telefono: string;
  edad: number;
}

export interface CrearAseguradoDto {
  cedula: string;
  nombreCompleto: string;
  telefono: string;
  edad: number;
}

export interface ActualizarAseguradoDto {
  aseguradoId: number;
  cedula: string;
  nombreCompleto: string;
  telefono: string;
  edad: number;
}

