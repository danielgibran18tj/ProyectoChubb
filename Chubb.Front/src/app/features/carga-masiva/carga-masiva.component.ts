import { Component } from '@angular/core';
import { ResultadoCargaMasiva } from '../../core/models/carga-masiva.model';
import { CargaMasivaService } from '../../core/services/carga-masiva.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-carga-masiva',
  imports: [CommonModule],
  templateUrl: './carga-masiva.component.html',
  styleUrl: './carga-masiva.component.css'
})
export class CargaMasivaComponent {
  archivoSeleccionado: File | null = null;
  loading = false;
  resultado: ResultadoCargaMasiva | null = null;
  mensaje: { tipo: 'success' | 'danger' | 'warning', texto: string } | null = null;

  constructor(private cargaMasivaService: CargaMasivaService) { }

  onFileSelected(event: any) {
    const file = event.target.files[0];

    if (file) {
      // Validar extensión
      if (!file.name.endsWith('.txt')) {
        this.mostrarMensaje('danger', 'Solo se permiten archivos .txt');
        this.archivoSeleccionado = null;
        event.target.value = '';
        return;
      }

      // Validar tamaño (máximo 5MB)
      if (file.size > 5 * 1024 * 1024) {
        this.mostrarMensaje('danger', 'El archivo no debe superar los 5MB');
        this.archivoSeleccionado = null;
        event.target.value = '';
        return;
      }

      this.archivoSeleccionado = file;
      this.resultado = null;
      this.mensaje = null;
    }
  }

  procesarArchivo() {
    if (!this.archivoSeleccionado) {
      this.mostrarMensaje('danger', 'Debe seleccionar un archivo');
      return;
    }

    this.loading = true;
    this.resultado = null;

    this.cargaMasivaService.subirArchivo(this.archivoSeleccionado).subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.resultado = response.data;

          if (this.resultado.registrosFallidos === 0) {
            this.mostrarMensaje('success', 'Carga completada exitosamente. Todos los registros fueron procesados.');
          } else if (this.resultado.registrosExitosos === 0) {
            this.mostrarMensaje('danger', 'La carga falló. No se procesó ningún registro.');
          } else {
            this.mostrarMensaje('warning', 'Carga completada con algunos errores.');
          }
        }
        this.loading = false;
      },
      error: (error) => {
        this.mostrarMensaje('danger', error.error?.message || 'Error al procesar el archivo');
        this.loading = false;
      }
    });
  }

  limpiar() {
    this.archivoSeleccionado = null;
    this.resultado = null;
    this.mensaje = null;

    // Limpiar el input file
    const fileInput = document.getElementById('fileInput') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }
  }

  descargarPlantilla() {
    const contenido = 'Cedula|NombreCompleto|Telefono|Edad\n0912345678|Juan Pérez García|0987654321|25\n0923456789|María López Fernández|0998765432|35\n0934567890|Carlos Ramírez Torres|0976543210|18';
    const blob = new Blob([contenido], { type: 'text/plain' });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = 'plantilla_asegurados.txt';
    link.click();
    window.URL.revokeObjectURL(url);
  }

  mostrarMensaje(tipo: 'success' | 'danger' | 'warning', texto: string) {
    this.mensaje = { tipo, texto };
  }

}
