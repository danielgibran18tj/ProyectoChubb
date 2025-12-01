import { Component } from '@angular/core';
import { ActualizarSeguroDto, CrearSeguroDto, Seguro } from '../../core/models/seguro.model';
import { SeguroService } from '../../core/services/seguro.service';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-seguros',
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './seguros.component.html',
  styleUrl: './seguros.component.css'
})
export class SegurosComponent {

  seguros: Seguro[] = [];
  seguroForm: FormGroup;
  loading = false;
  mostrarFormulario = false;
  modoEdicion = false;
  seguroSeleccionado: Seguro | null = null;
  mensaje: { tipo: 'success' | 'danger', texto: string } | null = null;
  pageSize: number = 10;
  paginaActual: number = 1;
  segurosPaginados: any[] = [];

  constructor(
    private fb: FormBuilder,
    private seguroService: SeguroService
  ) {
    this.seguroForm = this.fb.group({
      codigoSeguro: ['', [Validators.required, Validators.maxLength(50)]],
      nombreSeguro: ['', [Validators.required, Validators.maxLength(200)]],
      sumaAsegurada: ['', [Validators.required, Validators.min(1)]],
      prima: ['', [Validators.required, Validators.min(1)]]
    });
  }

  ngOnInit() {
    this.cargarSeguros();
  }

  cargarSeguros() {
    this.loading = true;
    this.seguroService.obtenerTodos().subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.seguros = response.data;
        }
        this.actualizarPaginacion();
        this.loading = false;
      },
      error: (error) => {
        this.mostrarMensaje('danger', 'Error al cargar seguros');
        this.loading = false;
        console.error(error);
      }
    });
  }

  nuevoSeguro() {
    this.modoEdicion = false;
    this.seguroSeleccionado = null;
    this.seguroForm.reset();
    this.mostrarFormulario = true;
  }

  editarSeguro(seguro: Seguro) {
    this.modoEdicion = true;
    this.seguroSeleccionado = seguro;
    this.seguroForm.patchValue({
      codigoSeguro: seguro.codigoSeguro,
      nombreSeguro: seguro.nombreSeguro,
      sumaAsegurada: seguro.sumaAsegurada,
      prima: seguro.prima
    });
    this.mostrarFormulario = true;
  }

  guardarSeguro() {
    if (this.seguroForm.invalid) {
      Object.keys(this.seguroForm.controls).forEach(key => {
        this.seguroForm.get(key)?.markAsTouched();
      });
      return;
    }

    this.loading = true;

    if (this.modoEdicion && this.seguroSeleccionado) {
      const seguroActualizado: ActualizarSeguroDto = {
        seguroId: this.seguroSeleccionado.seguroId,
        ...this.seguroForm.value
      };

      this.seguroService.actualizar(this.seguroSeleccionado.seguroId, seguroActualizado).subscribe({
        next: (response) => {
          if (response.success) {
            this.mostrarMensaje('success', 'Seguro actualizado exitosamente');
            this.cargarSeguros();
            this.cancelar();
          }
          this.loading = false;
        },
        error: (error) => {
          this.mostrarMensaje('danger', error.error?.message || 'Error al actualizar seguro');
          this.loading = false;
        }
      });
    } else {
      const nuevoSeguro: CrearSeguroDto = this.seguroForm.value;

      this.seguroService.crear(nuevoSeguro).subscribe({
        next: (response) => {
          if (response.success) {
            this.mostrarMensaje('success', 'Seguro creado exitosamente');
            this.cargarSeguros();
            this.cancelar();
          }
          this.loading = false;
        },
        error: (error) => {
          this.mostrarMensaje('danger', error.error?.message || 'Error al crear seguro');
          this.loading = false;
        }
      });
    }
  }

  eliminarSeguro(seguro: Seguro) {
    if (confirm(`¿Está seguro de eliminar el seguro "${seguro.nombreSeguro}"?`)) {
      this.loading = true;
      this.seguroService.eliminar(seguro.seguroId).subscribe({
        next: (response) => {
          if (response.success) {
            this.mostrarMensaje('success', 'Seguro eliminado exitosamente');
            this.cargarSeguros();
          }
          this.loading = false;
        },
        error: (error) => {
          this.mostrarMensaje('danger', 'Error al eliminar seguro');
          this.loading = false;
        }
      });
    }
  }

  cancelar() {
    this.mostrarFormulario = false;
    this.seguroForm.reset();
    this.seguroSeleccionado = null;
    this.modoEdicion = false;
  }

  mostrarMensaje(tipo: 'success' | 'danger', texto: string) {
    this.mensaje = { tipo, texto };
    setTimeout(() => {
      this.mensaje = null;
    }, 5000);
  }

  get f() {
    return this.seguroForm.controls;
  }

  get totalPaginas(): number {
    return Math.ceil(this.seguros.length / this.pageSize);
  }

  get indiceInicio(): number {
    return (this.paginaActual - 1) * this.pageSize;
  }

  get indiceFin(): number {
    return Math.min(this.indiceInicio + this.pageSize, this.seguros.length);
  }

  get paginas(): number[] {
    const paginas: number[] = [];
    const maxPaginas = 5; // Mostrar máximo 5 números de página

    let inicio = Math.max(1, this.paginaActual - Math.floor(maxPaginas / 2));
    let fin = Math.min(this.totalPaginas, inicio + maxPaginas - 1);

    if (fin - inicio < maxPaginas - 1) {
      inicio = Math.max(1, fin - maxPaginas + 1);
    }

    for (let i = inicio; i <= fin; i++) {
      paginas.push(i);
    }

    return paginas;
  }

  irAPagina(pagina: number): void {
    if (pagina >= 1 && pagina <= this.totalPaginas) {
      this.paginaActual = pagina;
      this.actualizarPaginacion();
    }
  }

  cambiarTamanioPagina(): void {
    this.paginaActual = 1;
    this.actualizarPaginacion();
  }

  actualizarPaginacion(): void {
    const inicio = this.indiceInicio;
    const fin = this.indiceFin;
    this.segurosPaginados = this.seguros.slice(inicio, fin);
  }

}
