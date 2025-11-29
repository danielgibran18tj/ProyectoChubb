export interface Seguro {
  seguroId: number;
  codigoSeguro: string;
  nombreSeguro: string;
  sumaAsegurada: number;
  prima: number;
}

export interface CrearSeguroDto {
  codigoSeguro: string;
  nombreSeguro: string;
  sumaAsegurada: number;
  prima: number;
}

export interface ActualizarSeguroDto {
  seguroId: number;
  codigoSeguro: string;
  nombreSeguro: string;
  sumaAsegurada: number;
  prima: number;
}
