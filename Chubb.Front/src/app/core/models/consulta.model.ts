import { Asegurado } from "./asegurado.model";
import { Seguro } from "./seguro.model";

export interface ConsultaPorCedula {
  asegurado: Asegurado;
  seguros: SeguroAsignado[];
}

export interface ConsultaPorCodigoSeguro {
  seguro: Seguro;
  asegurados: AseguradoAsignado[];
}

export interface SeguroAsignado {
  seguroId: number;
  codigoSeguro: string;
  nombreSeguro: string;
  sumaAsegurada: number;
  prima: number;
  fechaAsignacion: Date;
}

export interface AseguradoAsignado {
  aseguradoId: number;
  cedula: string;
  nombreCompleto: string;
  telefono: string;
  edad: number;
  fechaAsignacion: Date;
}