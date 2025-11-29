export interface AsignacionDto {
  aseguradoId: number;
  seguroId: number;
}

export interface AsignacionDetalle {
  aseguradoSeguroId: number;
  aseguradoId: number;
  cedulaAsegurado: string;
  nombreAsegurado: string;
  seguroId: number;
  codigoSeguro: string;
  nombreSeguro: string;
  fechaAsignacion: Date;
}