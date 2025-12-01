-- =============================================
-- Script de Inicialización para Docker
-- Este script invoca el scriptDatabase.sql existente
-- =============================================

USE master;
GO

-- Esperar a que SQL Server esté completamente listo
WAITFOR DELAY '00:00:05';
GO

PRINT 'Iniciando creación de base de datos desde Docker...';
GO

-- Verificar si la base de datos ya existe
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'SistemaSeguroDB')
BEGIN
    PRINT 'Base de datos no existe, procediendo con la creación...';
    
    :r /docker-entrypoint-initdb.d/scriptDatabase.sql
    
    PRINT 'Base de datos inicializada correctamente desde scriptDatabase.sql';
END
ELSE
BEGIN
    PRINT 'Base de datos SistemaSeguroDB ya existe, omitiendo inicialización';
END
GO

PRINT 'Proceso de inicialización completado';
GO