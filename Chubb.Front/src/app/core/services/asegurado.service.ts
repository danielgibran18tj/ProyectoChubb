import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { ActualizarAseguradoDto, Asegurado, CrearAseguradoDto } from '../models/asegurado.model';
import { ApiResponse } from '../models/api-response.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AseguradoService {

  private apiUrl = `${environment.apiUrl}/Asegurados`;

  constructor(private http: HttpClient) { }

  obtenerTodos(): Observable<ApiResponse<Asegurado[]>> {
    return this.http.get<ApiResponse<Asegurado[]>>(this.apiUrl);
  }

  obtenerPorId(id: number): Observable<ApiResponse<Asegurado>> {
    return this.http.get<ApiResponse<Asegurado>>(`${this.apiUrl}/${id}`);
  }

  crear(asegurado: CrearAseguradoDto): Observable<ApiResponse<Asegurado>> {
    return this.http.post<ApiResponse<Asegurado>>(this.apiUrl, asegurado);
  }

  actualizar(id: number, asegurado: ActualizarAseguradoDto): Observable<ApiResponse<Asegurado>> {
    return this.http.put<ApiResponse<Asegurado>>(`${this.apiUrl}/${id}`, asegurado);
  }

  eliminar(id: number): Observable<ApiResponse<void>> {
    return this.http.delete<ApiResponse<void>>(`${this.apiUrl}/${id}`);
  }

}
