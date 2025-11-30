using Business.Interfaces;
using Business.Validators;
using Data.Interfaces;
using Models.DTOs;
using Models.Entities;
using Models.Responses;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CargaMasivaService : ICargaMasivaService
    {
        private readonly IAseguradoRepository _aseguradoRepository;
        private readonly ISeguroRepository _seguroRepository;
        private readonly IAseguradoSeguroRepository _aseguradoSeguroRepository;
        private readonly AseguradoValidator _validator;

        public CargaMasivaService(
            IAseguradoRepository aseguradoRepository,
            ISeguroRepository seguroRepository,
            IAseguradoSeguroRepository aseguradoSeguroRepository)
        {
            _aseguradoRepository = aseguradoRepository;
            _seguroRepository = seguroRepository;
            _aseguradoSeguroRepository = aseguradoSeguroRepository;
            _validator = new AseguradoValidator();
        }

        private async Task<ApiResponse<ResultadoCargaMasivaDto>> ProcesarCargaMasivaAsync(
            List<AseguradoCargaDto> asegurados,
            string nombreArchivo)
        {
            try
            {
                var resultado = new ResultadoCargaMasivaDto
                {
                    TotalRegistros = asegurados.Count
                };

                var aseguradosValidos = new List<Asegurado>();

                // Validar cada asegurado
                foreach (var dto in asegurados)
                {
                    var validacion = _validator.ValidarCreacion(new CrearAseguradoDto
                    {
                        Cedula = dto.Cedula,
                        NombreCompleto = dto.NombreCompleto,
                        Telefono = dto.Telefono,
                        Edad = dto.Edad
                    });

                    if (!validacion.IsValid)
                    {
                        resultado.RegistrosFallidos++;
                        resultado.Errores.Add(new ErrorCargaDto
                        {
                            NumeroLinea = dto.NumeroLinea,
                            Cedula = dto.Cedula,
                            MensajeError = string.Join(", ", validacion.GetErrorMessages())
                        });
                        continue;
                    }

                    // Verificar si la cédula ya existe
                    if (await _aseguradoRepository.ExisteCedulaAsync(dto.Cedula))
                    {
                        resultado.RegistrosFallidos++;
                        resultado.Errores.Add(new ErrorCargaDto
                        {
                            NumeroLinea = dto.NumeroLinea,
                            Cedula = dto.Cedula,
                            MensajeError = "La cédula ya está registrada"
                        });
                        continue;
                    }

                    aseguradosValidos.Add(new Asegurado
                    {
                        Cedula = dto.Cedula,
                        NombreCompleto = dto.NombreCompleto,
                        Telefono = dto.Telefono,
                        Edad = dto.Edad
                    });
                }

                // Insertar asegurados válidos
                if (aseguradosValidos.Any())
                {
                    resultado.RegistrosExitosos = await _aseguradoRepository.CrearMasivoAsync(aseguradosValidos);

                    var aseg = await _aseguradoRepository.ObtenerTodosAsync();

                    foreach (var item in aseguradosValidos)
                    {
                        item.AseguradoId = aseg.FirstOrDefault(a => a.Cedula == item.Cedula)!.AseguradoId;
                    }

                    // Asignar seguros automáticamente según edad
                    await AsignarSegurosAutomaticoAsync(aseguradosValidos);
                }

                resultado.Mensaje = $"Procesamiento completado. {resultado.RegistrosExitosos} registros exitosos, {resultado.RegistrosFallidos} fallidos.";
                return ApiResponse<ResultadoCargaMasivaDto>.SuccessResponse(resultado, resultado.Mensaje);
            }
            catch (Exception ex)
            {
                return ApiResponse<ResultadoCargaMasivaDto>.ErrorResponse($"Error al procesar la carga masiva: {ex.Message}");
            }
        }


        public async Task<ApiResponse<ResultadoCargaMasivaDto>> LeerArchivoTextoAsync(Stream fileStream, string nombreArchivo)
        {
            var asegurados = new List<AseguradoCargaDto>();
            var numeroLinea = 0;

            using (var reader = new StreamReader(fileStream, Encoding.UTF8))
            {
                // Saltar encabezado si existe
                string? primeraLinea = await reader.ReadLineAsync();
                numeroLinea++;

                string? linea;
                while ((linea = await reader.ReadLineAsync()) != null)
                {
                    numeroLinea++;
                    if (string.IsNullOrWhiteSpace(linea)) continue;

                    // Formato esperado: Cedula|NombreCompleto|Telefono|Edad
                    var campos = linea.Split('|');
                    if (campos.Length != 4) continue;

                    asegurados.Add(new AseguradoCargaDto
                    {
                        Cedula = campos[0].Trim(),
                        NombreCompleto = campos[1].Trim(),
                        Telefono = campos[2].Trim(),
                        Edad = int.TryParse(campos[3].Trim(), out var edad) ? edad : 0,
                        NumeroLinea = numeroLinea
                    });
                }
            }

            if (!asegurados.Any())
            {
                return ApiResponse<ResultadoCargaMasivaDto>.ErrorResponse("El archivo no contiene registros válidos");
            }

            var response = await ProcesarCargaMasivaAsync(asegurados, nombreArchivo);

            return response;
        }


        public async Task<ApiResponse<ResultadoCargaMasivaDto>> LeerArchivoExcelAsync(Stream fileStream, string nombreArchivo)
        {
            var asegurados = new List<AseguradoCargaDto>();

            try
            {
                IWorkbook workbook;

                // Determinar el tipo de archivo por extensión
                if (nombreArchivo.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    workbook = new XSSFWorkbook(fileStream);
                }
                else if (nombreArchivo.EndsWith(".xls", StringComparison.OrdinalIgnoreCase))
                {
                    workbook = new HSSFWorkbook(fileStream);
                }
                else
                {
                    return ApiResponse<ResultadoCargaMasivaDto>.ErrorResponse("Formato de archivo no soportado. Use .xlsx o .xls");
                }

                using (workbook)
                {
                    var sheet = workbook.GetSheetAt(0); // Primera hoja

                    if (sheet == null)
                    {
                        return ApiResponse<ResultadoCargaMasivaDto>.ErrorResponse("El archivo Excel no contiene hojas");
                    }

                    var rowCount = sheet.LastRowNum;

                    if (rowCount <= 0)
                    {
                        return ApiResponse<ResultadoCargaMasivaDto>.ErrorResponse("El archivo no contiene datos");
                    }

                    // Empezar desde la fila 1 (índice base 0, asumiendo que la fila 0 es encabezado)
                    for (int rowIndex = 1; rowIndex <= rowCount; rowIndex++)
                    {
                        var row = sheet.GetRow(rowIndex);

                        if (row == null) continue;

                        // Verificar si la fila está vacía
                        if (row.GetCell(0) == null && row.GetCell(1) == null &&
                            row.GetCell(2) == null && row.GetCell(3) == null)
                        {
                            continue;
                        }

                        // Formato esperado: Columna0=Cedula, Columna1=NombreCompleto, Columna2=Telefono, Columna3=Edad
                        var cedula = GetCellValue(row.GetCell(0));
                        var nombreCompleto = GetCellValue(row.GetCell(1));
                        var telefono = GetCellValue(row.GetCell(2));
                        var edadTexto = GetCellValue(row.GetCell(3));

                        // Validar que al menos tenga cédula
                        if (string.IsNullOrWhiteSpace(cedula))
                        {
                            continue;
                        }

                        asegurados.Add(new AseguradoCargaDto
                        {
                            Cedula = cedula,
                            NombreCompleto = nombreCompleto,
                            Telefono = telefono,
                            Edad = int.TryParse(edadTexto, out var edad) ? edad : 0,
                            NumeroLinea = rowIndex + 1 // número de línea en Excel
                        });
                    }
                }

                if (!asegurados.Any())
                {
                    return ApiResponse<ResultadoCargaMasivaDto>.ErrorResponse("El archivo no contiene registros válidos");
                }

                var response = await ProcesarCargaMasivaAsync(asegurados, nombreArchivo);
                return response;
            }
            catch (Exception ex)
            {
                return ApiResponse<ResultadoCargaMasivaDto>.ErrorResponse($"Error al procesar el archivo Excel: {ex.Message}");
            }
        }

        private string GetCellValue(ICell cell)
        {
            if (cell == null) return string.Empty;

            return cell.CellType switch
            {
                CellType.String => cell.StringCellValue?.Trim() ?? string.Empty,
                CellType.Numeric => cell.NumericCellValue.ToString().Trim(),
                CellType.Boolean => cell.BooleanCellValue.ToString().Trim(),
                CellType.Formula => cell.StringCellValue?.Trim() ?? string.Empty,
                _ => string.Empty
            };
        }

        private async Task AsignarSegurosAutomaticoAsync(List<Asegurado> asegurados)
        {
            try
            {
                var seguros = await _seguroRepository.ObtenerTodosAsync();
                var listaSeguros = seguros.ToList();

                if (!listaSeguros.Any()) return;

                // Lógica de asignación por edad 
                // Producto 1: menores de 20 años
                // Producto 2: 20 a 30 años
                // Producto 3: mayores de 30 años

                foreach (var asegurado in asegurados)
                {
                    Seguro? seguroAsignado = null;

                    if (asegurado.Edad < 20 && listaSeguros.Count > 0)
                        seguroAsignado = listaSeguros[0];
                    else if (asegurado.Edad >= 20 && asegurado.Edad <= 30 && listaSeguros.Count > 1)
                        seguroAsignado = listaSeguros[1];
                    else if (asegurado.Edad > 30 && listaSeguros.Count > 2)
                        seguroAsignado = listaSeguros[2];
                    else if (listaSeguros.Count > 0)
                        seguroAsignado = listaSeguros[0]; // Seguro por defecto

                    if (seguroAsignado != null)
                    {
                        try
                        {
                            await _aseguradoSeguroRepository.AsignarSeguroAsync(asegurado.AseguradoId, seguroAsignado.SeguroId);
                        }
                        catch
                        {
                            // Ignorar errores de asignación individual
                        }
                    }
                }
            }
            catch
            {
                // No fallar el proceso si la asignación automática falla
            }
        }

    }
}
