import { Injectable } from '@angular/core';
import { ApiResponse } from '../models/api-response.model';
import { Observable } from 'rxjs';
import { ActualizarSeguroDto, CrearSeguroDto, Seguro } from '../models/seguro.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SeguroService {

  private apiUrl = `${environment.apiUrl}/Seguros`;

  constructor(private http: HttpClient) { }

  obtenerTodos(): Observable<ApiResponse<Seguro[]>> {
    return this.http.get<ApiResponse<Seguro[]>>(this.apiUrl);
  }

  obtenerPorId(id: number): Observable<ApiResponse<Seguro>> {
    return this.http.get<ApiResponse<Seguro>>(`${this.apiUrl}/${id}`);
  }

  crear(seguro: CrearSeguroDto): Observable<ApiResponse<Seguro>> {
    return this.http.post<ApiResponse<Seguro>>(this.apiUrl, seguro);
  }

  actualizar(id: number, seguro: ActualizarSeguroDto): Observable<ApiResponse<Seguro>> {
    return this.http.put<ApiResponse<Seguro>>(`${this.apiUrl}/${id}`, seguro);
  }

  eliminar(id: number): Observable<ApiResponse<void>> {
    return this.http.delete<ApiResponse<void>>(`${this.apiUrl}/${id}`);
  }

}
