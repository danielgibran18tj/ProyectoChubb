import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { AsignacionDetalle, AsignacionDto } from '../models/asignacion.model';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/api-response.model';
import { ConsultaPorCedula, ConsultaPorCodigoSeguro } from '../models/consulta.model';

@Injectable({
  providedIn: 'root'
})
export class AsignacionService {

  private apiUrl = `${environment.apiUrl}`;

  constructor(private http: HttpClient) { }

  asignarSeguro(asignacion: AsignacionDto): Observable<ApiResponse<AsignacionDetalle>> {
    return this.http.post<ApiResponse<AsignacionDetalle>>(`${this.apiUrl}/Asignaciones`, asignacion);
  }

  consultarPorCedula(cedula: string): Observable<ApiResponse<ConsultaPorCedula>> {
    return this.http.get<ApiResponse<ConsultaPorCedula>>(`${this.apiUrl}/Consultas/por-cedula/${cedula}`);
  }

  consultarPorCodigoSeguro(codigoSeguro: string): Observable<ApiResponse<ConsultaPorCodigoSeguro>> {
    return this.http.get<ApiResponse<ConsultaPorCodigoSeguro>>(`${this.apiUrl}/Consultas/por-codigo-seguro/${codigoSeguro}`);
  }

}
