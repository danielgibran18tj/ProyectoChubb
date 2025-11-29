export interface ResultadoCargaMasiva {
  totalRegistros: number;
  registrosExitosos: number;
  registrosFallidos: number;
  errores: ErrorCarga[];
  mensaje: string;
}

export interface ErrorCarga {
  numeroLinea: number;
  cedula: string;
  mensajeError: string;
}
