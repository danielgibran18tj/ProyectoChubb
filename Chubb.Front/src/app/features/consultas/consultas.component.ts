import { Component } from '@angular/core';
import { ConsultaPorCedula, ConsultaPorCodigoSeguro } from '../../core/models/consulta.model';
import { AsignacionService } from '../../core/services/asignacion.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-consultas',
  imports: [CommonModule, FormsModule],
  templateUrl: './consultas.component.html',
  styleUrl: './consultas.component.css'
})
export class ConsultasComponent {
  cedulaBuscar = '';
  resultadoCedula: ConsultaPorCedula | null = null;

  // Búsqueda por código de seguro
  codigoSeguroBuscar = '';
  resultadoCodigoSeguro: ConsultaPorCodigoSeguro | null = null;

  loading = false;
  mensaje: { tipo: 'success' | 'danger' | 'info', texto: string } | null = null;
  tipoConsulta: 'cedula' | 'codigo' = 'cedula';

  constructor(private asignacionService: AsignacionService) { }

  cambiarTipoConsulta(tipo: 'cedula' | 'codigo') {
    this.tipoConsulta = tipo;
    this.limpiarResultados();
  }

  buscarPorCedula() {
    if (!this.cedulaBuscar.trim()) {
      this.mostrarMensaje('danger', 'Debe ingresar una cédula');
      return;
    }

    this.loading = true;
    this.resultadoCedula = null;
    this.mensaje = null;

    this.asignacionService.consultarPorCedula(this.cedulaBuscar).subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.resultadoCedula = response.data;
          if (this.resultadoCedula.seguros.length === 0) {
            this.mostrarMensaje('info', 'El asegurado no tiene seguros asignados');
          } else {
            this.mostrarMensaje('success', 'Consulta realizada exitosamente');
          }
        }
        this.loading = false;
      },
      error: (error) => {
        this.mostrarMensaje('danger', error.error?.message || 'No se encontró información para esta cédula');
        this.loading = false;
      }
    });
  }

  buscarPorCodigoSeguro() {
    if (!this.codigoSeguroBuscar.trim()) {
      this.mostrarMensaje('danger', 'Debe ingresar un código de seguro');
      return;
    }

    this.loading = true;
    this.resultadoCodigoSeguro = null;
    this.mensaje = null;

    this.asignacionService.consultarPorCodigoSeguro(this.codigoSeguroBuscar).subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.resultadoCodigoSeguro = response.data;
          if (this.resultadoCodigoSeguro.asegurados.length === 0) {
            this.mostrarMensaje('info', 'El seguro no tiene asegurados asignados');
          } else {
            this.mostrarMensaje('success', 'Consulta realizada exitosamente');
          }
        }
        this.loading = false;
      },
      error: (error) => {
        this.mostrarMensaje('danger', error.error?.message || 'No se encontró información para este código');
        this.loading = false;
      }
    });
  }

  limpiarResultados() {
    this.resultadoCedula = null;
    this.resultadoCodigoSeguro = null;
    this.cedulaBuscar = '';
    this.codigoSeguroBuscar = '';
    this.mensaje = null;
  }

  mostrarMensaje(tipo: 'success' | 'danger' | 'info', texto: string) {
    this.mensaje = { tipo, texto };
    setTimeout(() => {
      if (this.mensaje?.tipo !== 'success') {
        this.mensaje = null;
      }
    }, 5000);
  }

}
