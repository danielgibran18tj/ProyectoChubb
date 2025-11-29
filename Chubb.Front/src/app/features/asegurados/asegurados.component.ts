import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Seguro } from '../../core/models/seguro.model';
import { ActualizarAseguradoDto, Asegurado, CrearAseguradoDto } from '../../core/models/asegurado.model';
import { AsignacionService } from '../../core/services/asignacion.service';
import { SeguroService } from '../../core/services/seguro.service';
import { AseguradoService } from '../../core/services/asegurado.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-asegurados',
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './asegurados.component.html',
  styleUrl: './asegurados.component.css'
})
export class AseguradosComponent {

  asegurados: Asegurado[] = [];
  seguros: Seguro[] = [];
  aseguradoForm: FormGroup;
  loading = false;
  mostrarFormulario = false;
  modoEdicion = false;
  aseguradoSeleccionado: Asegurado | null = null;
  mensaje: { tipo: 'success' | 'danger', texto: string } | null = null;

  // Modal de asignación
  mostrarModalAsignacion = false;
  aseguradoParaAsignar: Asegurado | null = null;
  seguroSeleccionadoId: number | null = null;

  constructor(
    private fb: FormBuilder,
    private aseguradoService: AseguradoService,
    private seguroService: SeguroService,
    private asignacionService: AsignacionService
  ) {
    this.aseguradoForm = this.fb.group({
      cedula: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],
      nombreCompleto: ['', [Validators.required, Validators.maxLength(200)]],
      telefono: ['', [Validators.required, Validators.pattern(/^(\+593)?\d{9,10}$/)]],
      edad: ['', [Validators.required, Validators.min(0), Validators.max(120)]]
    });
  }

  ngOnInit() {
    this.cargarAsegurados();
    this.cargarSeguros();
  }

  cargarAsegurados() {
    this.loading = true;
    this.aseguradoService.obtenerTodos().subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.asegurados = response.data;
        }
        this.loading = false;
      },
      error: (error) => {
        this.mostrarMensaje('danger', 'Error al cargar asegurados');
        this.loading = false;
        console.error(error);
      }
    });
  }

  cargarSeguros() {
    this.seguroService.obtenerTodos().subscribe({
      next: (response) => {
        if (response.success && response.data) {
          this.seguros = response.data;
        }
      },
      error: (error) => {
        console.error('Error al cargar seguros:', error);
      }
    });
  }

  nuevoAsegurado() {
    this.modoEdicion = false;
    this.aseguradoSeleccionado = null;
    this.aseguradoForm.reset();
    this.mostrarFormulario = true;
  }

  editarAsegurado(asegurado: Asegurado) {
    this.modoEdicion = true;
    this.aseguradoSeleccionado = asegurado;
    this.aseguradoForm.patchValue({
      cedula: asegurado.cedula,
      nombreCompleto: asegurado.nombreCompleto,
      telefono: asegurado.telefono,
      edad: asegurado.edad
    });
    this.mostrarFormulario = true;
  }

  guardarAsegurado() {
    if (this.aseguradoForm.invalid) {
      Object.keys(this.aseguradoForm.controls).forEach(key => {
        this.aseguradoForm.get(key)?.markAsTouched();
      });
      return;
    }

    this.loading = true;

    if (this.modoEdicion && this.aseguradoSeleccionado) {
      const aseguradoActualizado: ActualizarAseguradoDto = {
        aseguradoId: this.aseguradoSeleccionado.aseguradoId,
        ...this.aseguradoForm.value
      };

      this.aseguradoService.actualizar(this.aseguradoSeleccionado.aseguradoId, aseguradoActualizado).subscribe({
        next: (response) => {
          if (response.success) {
            this.mostrarMensaje('success', 'Asegurado actualizado exitosamente');
            this.cargarAsegurados();
            this.cancelar();
          }
          this.loading = false;
        },
        error: (error) => {
          this.mostrarMensaje('danger', error.error?.message || 'Error al actualizar asegurado');
          this.loading = false;
        }
      });
    } else {
      const nuevoAsegurado: CrearAseguradoDto = this.aseguradoForm.value;

      this.aseguradoService.crear(nuevoAsegurado).subscribe({
        next: (response) => {
          if (response.success) {
            this.mostrarMensaje('success', 'Asegurado creado exitosamente');
            this.cargarAsegurados();
            this.cancelar();
          }
          this.loading = false;
        },
        error: (error) => {
          this.mostrarMensaje('danger', error.error?.message || 'Error al crear asegurado');
          this.loading = false;
        }
      });
    }
  }

  eliminarAsegurado(asegurado: Asegurado) {
    if (confirm(`¿Está seguro de eliminar al asegurado "${asegurado.nombreCompleto}"?`)) {
      this.loading = true;
      this.aseguradoService.eliminar(asegurado.aseguradoId).subscribe({
        next: (response) => {
          if (response.success) {
            this.mostrarMensaje('success', 'Asegurado eliminado exitosamente');
            this.cargarAsegurados();
          }
          this.loading = false;
        },
        error: (error) => {
          this.mostrarMensaje('danger', 'Error al eliminar asegurado');
          this.loading = false;
        }
      });
    }
  }

  abrirModalAsignacion(asegurado: Asegurado) {
    this.aseguradoParaAsignar = asegurado;
    this.seguroSeleccionadoId = null;
    this.mostrarModalAsignacion = true;
  }

  cerrarModalAsignacion() {
    this.mostrarModalAsignacion = false;
    this.aseguradoParaAsignar = null;
    this.seguroSeleccionadoId = null;
  }

  asignarSeguro() {
    if (!this.aseguradoParaAsignar || !this.seguroSeleccionadoId) {
      this.mostrarMensaje('danger', 'Debe seleccionar un seguro');
      return;
    }

    this.loading = true;
    this.asignacionService.asignarSeguro({
      aseguradoId: this.aseguradoParaAsignar.aseguradoId,
      seguroId: this.seguroSeleccionadoId
    }).subscribe({
      next: (response) => {
        if (response.success) {
          this.mostrarMensaje('success', 'Seguro asignado exitosamente');
          this.cerrarModalAsignacion();
        }
        this.loading = false;
      },
      error: (error) => {
        this.mostrarMensaje('danger', error.error?.message || 'Error al asignar seguro');
        this.loading = false;
      }
    });
  }

  cancelar() {
    this.mostrarFormulario = false;
    this.aseguradoForm.reset();
    this.aseguradoSeleccionado = null;
    this.modoEdicion = false;
  }

  mostrarMensaje(tipo: 'success' | 'danger', texto: string) {
    this.mensaje = { tipo, texto };
    setTimeout(() => {
      this.mensaje = null;
    }, 5000);
  }

  get f() {
    return this.aseguradoForm.controls;
  }

}
