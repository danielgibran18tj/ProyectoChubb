import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/api-response.model';
import { ResultadoCargaMasiva } from '../models/carga-masiva.model';

@Injectable({
  providedIn: 'root'
})
export class CargaMasivaService {

  private apiUrl = `${environment.apiUrl}/CargaMasiva`;

  constructor(private http: HttpClient) { }

  subirArchivo(archivo: File): Observable<ApiResponse<ResultadoCargaMasiva>> {
    const formData = new FormData();
    formData.append('archivo', archivo, archivo.name);

    return this.http.post<ApiResponse<ResultadoCargaMasiva>>(`${this.apiUrl}/subir-archivo`, formData);
  }

}
