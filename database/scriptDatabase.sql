USE [master]
GO
/****** Object:  Database [ChubbSeguroDB]    Script Date: 2025-11-29 2:33:29 PM ******/
CREATE DATABASE [ChubbSeguroDB]
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ChubbSeguroDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ChubbSeguroDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ChubbSeguroDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ChubbSeguroDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ChubbSeguroDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ChubbSeguroDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [ChubbSeguroDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ChubbSeguroDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ChubbSeguroDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ChubbSeguroDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ChubbSeguroDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ChubbSeguroDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ChubbSeguroDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ChubbSeguroDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ChubbSeguroDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ChubbSeguroDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ChubbSeguroDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ChubbSeguroDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ChubbSeguroDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ChubbSeguroDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ChubbSeguroDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ChubbSeguroDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ChubbSeguroDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ChubbSeguroDB] SET RECOVERY FULL 
GO
ALTER DATABASE [ChubbSeguroDB] SET  MULTI_USER 
GO
ALTER DATABASE [ChubbSeguroDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ChubbSeguroDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ChubbSeguroDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ChubbSeguroDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ChubbSeguroDB] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'ChubbSeguroDB', N'ON'
GO
USE [ChubbSeguroDB]
GO
/****** Object:  Table [dbo].[Asegurados]    Script Date: 2025-11-29 2:33:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Asegurados](
	[AseguradoId] [int] IDENTITY(1,1) NOT NULL,
	[Cedula] [nvarchar](20) NOT NULL,
	[NombreCompleto] [nvarchar](200) NOT NULL,
	[Telefono] [nvarchar](20) NOT NULL,
	[Edad] [int] NOT NULL,
	[FechaCreacion] [datetime2](7) NULL,
	[FechaModificacion] [datetime2](7) NULL,
	[Activo] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[AseguradoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Cedula] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AseguradosSeguros]    Script Date: 2025-11-29 2:33:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AseguradosSeguros](
	[AseguradoSeguroId] [int] IDENTITY(1,1) NOT NULL,
	[AseguradoId] [int] NOT NULL,
	[SeguroId] [int] NOT NULL,
	[FechaAsignacion] [datetime2](7) NULL,
	[Activo] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[AseguradoSeguroId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_AseguradosSeguros] UNIQUE NONCLUSTERED 
(
	[AseguradoId] ASC,
	[SeguroId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CargasMasivas]    Script Date: 2025-11-29 2:33:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CargasMasivas](
	[CargaMasivaId] [int] IDENTITY(1,1) NOT NULL,
	[NombreArchivo] [nvarchar](500) NOT NULL,
	[TotalRegistros] [int] NOT NULL,
	[RegistrosExitosos] [int] NOT NULL,
	[RegistrosFallidos] [int] NOT NULL,
	[FechaCarga] [datetime2](7) NULL,
	[UsuarioCarga] [nvarchar](100) NULL,
	[Observaciones] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[CargaMasivaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Seguros]    Script Date: 2025-11-29 2:33:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Seguros](
	[SeguroId] [int] IDENTITY(1,1) NOT NULL,
	[CodigoSeguro] [nvarchar](50) NOT NULL,
	[NombreSeguro] [nvarchar](200) NOT NULL,
	[SumaAsegurada] [decimal](18, 2) NOT NULL,
	[Prima] [decimal](18, 2) NOT NULL,
	[FechaCreacion] [datetime2](7) NULL,
	[FechaModificacion] [datetime2](7) NULL,
	[Activo] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[SeguroId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[CodigoSeguro] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Asegurados] ADD  DEFAULT (getdate()) FOR [FechaCreacion]
GO
ALTER TABLE [dbo].[Asegurados] ADD  DEFAULT (getdate()) FOR [FechaModificacion]
GO
ALTER TABLE [dbo].[Asegurados] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[AseguradosSeguros] ADD  DEFAULT (getdate()) FOR [FechaAsignacion]
GO
ALTER TABLE [dbo].[AseguradosSeguros] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[CargasMasivas] ADD  DEFAULT (getdate()) FOR [FechaCarga]
GO
ALTER TABLE [dbo].[Seguros] ADD  DEFAULT (getdate()) FOR [FechaCreacion]
GO
ALTER TABLE [dbo].[Seguros] ADD  DEFAULT (getdate()) FOR [FechaModificacion]
GO
ALTER TABLE [dbo].[Seguros] ADD  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[AseguradosSeguros]  WITH CHECK ADD  CONSTRAINT [FK_AseguradosSeguros_Asegurados] FOREIGN KEY([AseguradoId])
REFERENCES [dbo].[Asegurados] ([AseguradoId])
GO
ALTER TABLE [dbo].[AseguradosSeguros] CHECK CONSTRAINT [FK_AseguradosSeguros_Asegurados]
GO
ALTER TABLE [dbo].[AseguradosSeguros]  WITH CHECK ADD  CONSTRAINT [FK_AseguradosSeguros_Seguros] FOREIGN KEY([SeguroId])
REFERENCES [dbo].[Seguros] ([SeguroId])
GO
ALTER TABLE [dbo].[AseguradosSeguros] CHECK CONSTRAINT [FK_AseguradosSeguros_Seguros]
GO
ALTER TABLE [dbo].[Asegurados]  WITH CHECK ADD CHECK  (([Edad]>=(0) AND [Edad]<=(120)))
GO
ALTER TABLE [dbo].[Seguros]  WITH CHECK ADD CHECK  (([Prima]>(0)))
GO
ALTER TABLE [dbo].[Seguros]  WITH CHECK ADD CHECK  (([SumaAsegurada]>(0)))
GO
ALTER TABLE [dbo].[Seguros]  WITH CHECK ADD  CONSTRAINT [CK_Seguros_SumaAsegurada] CHECK  (([SumaAsegurada]>=[Prima]))
GO
ALTER TABLE [dbo].[Seguros] CHECK CONSTRAINT [CK_Seguros_SumaAsegurada]
GO
/****** Object:  StoredProcedure [dbo].[sp_ActualizarAsegurado]    Script Date: 2025-11-29 2:33:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SP: Actualizar Asegurado
CREATE   PROCEDURE [dbo].[sp_ActualizarAsegurado]
    @AseguradoId INT,
    @Cedula NVARCHAR(20),
    @NombreCompleto NVARCHAR(200),
    @Telefono NVARCHAR(20),
    @Edad INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM dbo.Asegurados WHERE Cedula = @Cedula AND AseguradoId != @AseguradoId AND Activo = 1)
        BEGIN
            RAISERROR('La cédula ya existe en otro registro', 16, 1);
            RETURN;
        END

        UPDATE dbo.Asegurados
        SET Cedula = @Cedula,
            NombreCompleto = @NombreCompleto,
            Telefono = @Telefono,
            Edad = @Edad,
            FechaModificacion = GETDATE()
        WHERE AseguradoId = @AseguradoId AND Activo = 1;

        IF @@ROWCOUNT = 0
        BEGIN
            RAISERROR('Asegurado no encontrado', 16, 1);
        END
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ActualizarSeguro]    Script Date: 2025-11-29 2:33:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SP: Actualizar Seguro
CREATE   PROCEDURE [dbo].[sp_ActualizarSeguro]
    @SeguroId INT,
    @CodigoSeguro NVARCHAR(50),
    @NombreSeguro NVARCHAR(200),
    @SumaAsegurada DECIMAL(18,2),
    @Prima DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM dbo.Seguros WHERE CodigoSeguro = @CodigoSeguro AND SeguroId != @SeguroId AND Activo = 1)
        BEGIN
            RAISERROR('El código de seguro ya existe en otro registro', 16, 1);
            RETURN;
        END

        UPDATE dbo.Seguros
        SET CodigoSeguro = @CodigoSeguro,
            NombreSeguro = @NombreSeguro,
            SumaAsegurada = @SumaAsegurada,
            Prima = @Prima,
            FechaModificacion = GETDATE()
        WHERE SeguroId = @SeguroId AND Activo = 1;

        IF @@ROWCOUNT = 0
        BEGIN
            RAISERROR('Seguro no encontrado', 16, 1);
        END
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_AsignarSeguroAsegurado]    Script Date: 2025-11-29 2:33:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SP: Asignar Seguro a Asegurado
CREATE   PROCEDURE [dbo].[sp_AsignarSeguroAsegurado]
    @AseguradoId INT,
    @SeguroId INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        -- Verificar que existan ambos registros
        IF NOT EXISTS (SELECT 1 FROM dbo.Asegurados WHERE AseguradoId = @AseguradoId AND Activo = 1)
        BEGIN
            RAISERROR('Asegurado no encontrado', 16, 1);
            RETURN;
        END

        IF NOT EXISTS (SELECT 1 FROM dbo.Seguros WHERE SeguroId = @SeguroId AND Activo = 1)
        BEGIN
            RAISERROR('Seguro no encontrado', 16, 1);
            RETURN;
        END

        -- Verificar si ya existe la relación
        IF EXISTS (SELECT 1 FROM dbo.AseguradosSeguros 
                   WHERE AseguradoId = @AseguradoId AND SeguroId = @SeguroId AND Activo = 1)
        BEGIN
            RAISERROR('La asignación ya existe', 16, 1);
            RETURN;
        END

        INSERT INTO dbo.AseguradosSeguros (AseguradoId, SeguroId)
        VALUES (@AseguradoId, @SeguroId);

        SELECT SCOPE_IDENTITY() AS AseguradoSeguroId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ConsultarAseguradosPorCodigoSeguro]    Script Date: 2025-11-29 2:33:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SP: Consultar Asegurados por Código de Seguro
CREATE   PROCEDURE [dbo].[sp_ConsultarAseguradosPorCodigoSeguro]
    @CodigoSeguro NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        s.SeguroId,
        s.CodigoSeguro,
        s.NombreSeguro,
        s.SumaAsegurada,
        s.Prima,
        a.AseguradoId,
        a.Cedula,
        a.NombreCompleto,
        a.Telefono,
        a.Edad,
        aseg.FechaAsignacion
    FROM dbo.Seguros s
    LEFT JOIN dbo.AseguradosSeguros aseg ON s.SeguroId = aseg.SeguroId AND aseg.Activo = 1
    LEFT JOIN dbo.Asegurados a ON aseg.AseguradoId = a.AseguradoId AND a.Activo = 1
    WHERE s.CodigoSeguro = @CodigoSeguro 
      AND s.Activo = 1
    ORDER BY aseg.FechaAsignacion DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ConsultarSegurosPorCedula]    Script Date: 2025-11-29 2:33:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SP: Consultar Seguros por Cédula
CREATE   PROCEDURE [dbo].[sp_ConsultarSegurosPorCedula]
    @Cedula NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        a.AseguradoId,
        a.Cedula,
        a.NombreCompleto,
        a.Telefono,
        a.Edad,
        s.SeguroId,
        s.CodigoSeguro,
        s.NombreSeguro,
        s.SumaAsegurada,
        s.Prima,
        aseg.FechaAsignacion
    FROM dbo.Asegurados a
    LEFT JOIN dbo.AseguradosSeguros aseg ON a.AseguradoId = aseg.AseguradoId AND aseg.Activo = 1
    LEFT JOIN dbo.Seguros s ON aseg.SeguroId = s.SeguroId AND s.Activo = 1
    WHERE a.Cedula = @Cedula 
      AND a.Activo = 1 
    ORDER BY aseg.FechaAsignacion DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CrearAsegurado]    Script Date: 2025-11-29 2:33:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SP: Crear Asegurado
CREATE   PROCEDURE [dbo].[sp_CrearAsegurado]
    @Cedula NVARCHAR(20),
    @NombreCompleto NVARCHAR(200),
    @Telefono NVARCHAR(20),
    @Edad INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM dbo.Asegurados WHERE Cedula = @Cedula AND Activo = 1)
        BEGIN
            RAISERROR('La cédula ya existe', 16, 1);
            RETURN;
        END

        INSERT INTO dbo.Asegurados (Cedula, NombreCompleto, Telefono, Edad)
        VALUES (@Cedula, @NombreCompleto, @Telefono, @Edad);

        SELECT SCOPE_IDENTITY() AS AseguradoId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CrearSeguro]    Script Date: 2025-11-29 2:33:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_CrearSeguro]
    @CodigoSeguro NVARCHAR(50),
    @NombreSeguro NVARCHAR(200),
    @SumaAsegurada DECIMAL(18,2),
    @Prima DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM dbo.Seguros WHERE CodigoSeguro = @CodigoSeguro AND Activo = 1)
        BEGIN
            RAISERROR('El código de seguro ya existe', 16, 1);
            RETURN;
        END

        INSERT INTO dbo.Seguros (CodigoSeguro, NombreSeguro, SumaAsegurada, Prima)
        VALUES (@CodigoSeguro, @NombreSeguro, @SumaAsegurada, @Prima);

        SELECT SCOPE_IDENTITY() AS SeguroId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_EliminarAsegurado]    Script Date: 2025-11-29 2:33:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SP: Eliminar Asegurado (Lógico)
CREATE   PROCEDURE [dbo].[sp_EliminarAsegurado]
    @AseguradoId INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE dbo.Asegurados
        SET Activo = 0,
            FechaModificacion = GETDATE()
        WHERE AseguradoId = @AseguradoId;

        -- También desactivar las relaciones
        UPDATE dbo.AseguradosSeguros
        SET Activo = 0
        WHERE AseguradoId = @AseguradoId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_EliminarSeguro]    Script Date: 2025-11-29 2:33:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- SP: Eliminar Seguro (Lógico)
CREATE   PROCEDURE [dbo].[sp_EliminarSeguro]
    @SeguroId INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE dbo.Seguros
        SET Activo = 0,
            FechaModificacion = GETDATE()
        WHERE SeguroId = @SeguroId;

        -- También desactivar las relaciones
        UPDATE dbo.AseguradosSeguros
        SET Activo = 0
        WHERE SeguroId = @SeguroId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO
USE [master]
GO
ALTER DATABASE [ChubbSeguroDB] SET  READ_WRITE 
GO



-- Insertar seguros de ejemplo
INSERT INTO [ChubbSeguroDB].dbo.Seguros (CodigoSeguro, NombreSeguro, SumaAsegurada, Prima)
VALUES 
    ('SEG-001', 'Seguro Joven', 50000.00, 500.00),
    ('SEG-002', 'Seguro Adulto', 100000.00, 1200.00),
    ('SEG-003', 'Seguro Senior', 75000.00, 1500.00);
GO
