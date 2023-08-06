-- Creación base de datos
CREATE DATABASE BasicStore;

USE BasicStore	
CREATE TABLE Clientes (
	NITCliente VARCHAR(20) PRIMARY KEY,
    NombreCliente VARCHAR(100) NOT NULL,
    Direccion VARCHAR(200),
    Telefono VARCHAR(20),
    Email VARCHAR(100)
);

CREATE TABLE Articulos (
    ArticuloID INT PRIMARY KEY IDENTITY(1,1),
    NombreArticulo VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(200),
    Precio DECIMAL(10, 2) NOT NULL,
    Stock INT NOT NULL DEFAULT 0
);

CREATE TABLE Usuarios (
    UsuarioID INT PRIMARY KEY IDENTITY(1,1),
    NombreUsuario VARCHAR(50) NOT NULL,
    Contraseña VARCHAR(100) NOT NULL,
    CONSTRAINT UQ_Usuarios_NombreUsuario UNIQUE (NombreUsuario)
);

CREATE TABLE EncabezadoFactura (
    FacturaID INT PRIMARY KEY IDENTITY(1,1),
    FechaFactura DATE NOT NULL,
    NITCliente VARCHAR(20) NOT NULL,
    TotalFactura DECIMAL(10, 2) NOT NULL,
    CONSTRAINT FK_EncabezadoFactura_Cliente FOREIGN KEY (NITCliente) REFERENCES Clientes (NITCliente)
);

CREATE TABLE DetalleFactura (
    DetalleID INT PRIMARY KEY IDENTITY(1,1),
    FacturaID INT NOT NULL,
    ArticuloID INT NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10, 2) NOT NULL,
    Subtotal DECIMAL(10, 2) NOT NULL,
    CONSTRAINT FK_DetalleFactura_EncabezadoFactura FOREIGN KEY (FacturaID) REFERENCES EncabezadoFactura (FacturaID),
    CONSTRAINT FK_DetalleFactura_MaestroArticulos FOREIGN KEY (ArticuloID) REFERENCES Articulos (ArticuloID)
);

-- Indices
CREATE INDEX IX_Clientes_NombreCliente ON Clientes (NombreCliente);
CREATE INDEX IX_Articulos_NombreArticulo ON Articulos (NombreArticulo);
CREATE INDEX IX_Usuarios_NombreUsuario ON Usuarios (NombreUsuario);
CREATE INDEX IX_DetalleFactura_FacturaID ON DetalleFactura (FacturaID);
CREATE INDEX IX_DetalleFactura_ArticuloID ON DetalleFactura (ArticuloID);


-- CREACION PROCEDIMIENTOS ALMACENADOS
-- SP CRUD CLIENTES
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE sp_Clientes_CRUD
    @Operacion CHAR(1), -- 'C' para Crear, 'R' para Leer, 'U' para Actualizar, 'D' para Eliminar
    @NITCliente VARCHAR(50),
    @NombreCliente VARCHAR(100),
    @Direccion VARCHAR(200),
    @Telefono VARCHAR(20),
    @Email VARCHAR(100)
AS
BEGIN
    IF @Operacion = 'C'
    BEGIN
        INSERT INTO Clientes (NITCliente,NombreCliente, Direccion, Telefono, Email)
        VALUES (@NITCliente,@NombreCliente, @Direccion, @Telefono, @Email)
		SELECT SCOPE_IDENTITY();
    END
    ELSE IF @Operacion = 'R'
    BEGIN
        IF @NITCliente!=''
            SELECT * FROM Clientes WHERE NITCliente = @NITCliente
        ELSE
            SELECT * FROM Clientes
    END
    ELSE IF @Operacion = 'U'
    BEGIN
        UPDATE TOP(1) Clientes
        SET NombreCliente = @NombreCliente, Direccion = @Direccion, Telefono = @Telefono, Email = @Email
        WHERE NITCliente = @NITCliente
    END
    ELSE IF @Operacion = 'D'
    BEGIN
        DELETE TOP(1) FROM Clientes WHERE NITCliente = @NITCliente
    END
END


-- SP CRUD ARTICULOS
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE sp_Articulos_CRUD
    @Operacion CHAR(1), -- 'C' para Crear, 'R' para Leer, 'U' para Actualizar, 'D' para Eliminar
    @ArticuloID INT,
    @NombreArticulo VARCHAR(100),
    @Descripcion VARCHAR(200),
    @Precio DECIMAL(10, 2),
    @Stock INT
AS
BEGIN
    IF @Operacion = 'C'
    BEGIN
        INSERT INTO Articulos (NombreArticulo, Descripcion, Precio, Stock)
        VALUES (@NombreArticulo, @Descripcion, @Precio, @Stock)
    END
    ELSE IF @Operacion = 'R'
    BEGIN
        IF @ArticuloID IS NOT NULL
            SELECT * FROM Articulos WHERE ArticuloID = @ArticuloID
        ELSE
            SELECT * FROM Articulos
    END
    ELSE IF @Operacion = 'U'
    BEGIN
        UPDATE TOP(1) Articulos
        SET NombreArticulo = @NombreArticulo, Descripcion = @Descripcion, Precio = @Precio, Stock = @Stock
        WHERE ArticuloID = @ArticuloID
    END
    ELSE IF @Operacion = 'D'
    BEGIN
        DELETE TOP(1) FROM Articulos WHERE ArticuloID = @ArticuloID
    END
END

-- SP CRUD USUARIOS
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_Usuarios_CRUD]
    @Operacion CHAR(1), -- 'C' para Crear, 'R' para Leer, 'U' para Actualizar, 'D' para Eliminar, 'A' para leer por el usuario.
    @UsuarioID INT,
    @NombreUsuario VARCHAR(50),
    @Contraseña VARCHAR(100)
AS
BEGIN
    IF @Operacion = 'C'
    BEGIN
        INSERT INTO Usuarios (NombreUsuario, Contraseña)
        VALUES (@NombreUsuario, @Contraseña)
    END
    ELSE IF @Operacion = 'R'
    BEGIN
        IF @UsuarioID IS NOT NULL
            SELECT * FROM Usuarios WHERE UsuarioID = @UsuarioID
        ELSE
            SELECT * FROM Usuarios
    END
    ELSE IF @Operacion = 'U'
    BEGIN
        UPDATE TOP(1) Usuarios
        SET NombreUsuario = @NombreUsuario, Contraseña = @Contraseña
        WHERE UsuarioID = @UsuarioID
    END
    ELSE IF @Operacion = 'D'
    BEGIN
        DELETE TOP(1) FROM Usuarios WHERE UsuarioID = @UsuarioID
    END
	ELSE IF @Operacion = 'A'
    BEGIN
            SELECT * FROM Usuarios WHERE NombreUsuario = @NombreUsuario
    END
END

-- SP FACTURA
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE sp_EncabezadoFactura
    @Operacion CHAR(1), -- 'C' para Crear, 'R' para Leer, 'U' para Actualizar (solo NIT), 'D' para Eliminar
    @FacturaID INT,
    @FechaFactura DATE,
    @ClienteID INT,
    @NITCliente VARCHAR(20)
AS
BEGIN
    IF @Operacion = 'C'
    BEGIN
        -- Crear nueva factura
        INSERT INTO EncabezadoFactura (FechaFactura, NITCliente, TotalFactura)
        VALUES (@FechaFactura, @NITCliente, 0); -- El TotalFactura se inicializa en 0

        -- Obtener el ID de la factura recién creada
        SET @FacturaID = SCOPE_IDENTITY();
    END
    ELSE IF @Operacion = 'R'
    BEGIN
        -- Leer información de una factura y sus detalles asociados
        SELECT ef.FacturaID, ef.FechaFactura, ef.NITCliente, ef.TotalFactura,
               df.DetalleID, df.ArticuloID, a.NombreArticulo, df.Cantidad, df.PrecioUnitario, df.Subtotal
        FROM EncabezadoFactura ef
        INNER JOIN Clientes c ON ef.NITCliente = c.NITCliente
        LEFT JOIN DetalleFactura df ON ef.FacturaID = df.FacturaID
        LEFT JOIN Articulos a ON df.ArticuloID = a.ArticuloID
        WHERE ef.FacturaID = @FacturaID;
    END
    ELSE IF @Operacion = 'U'
    BEGIN
        -- Actualizar el NIT del cliente en la tabla EncabezadoFactura
        UPDATE TOP(1) EncabezadoFactura
        SET NITCliente = @NITCliente
        WHERE FacturaID = @FacturaID;
    END
    ELSE IF @Operacion = 'D'
    BEGIN
        -- Eliminar la factura y sus detalles asociados
        DELETE FROM DetalleFactura WHERE FacturaID = @FacturaID;
        DELETE TOP(1) FROM EncabezadoFactura WHERE FacturaID = @FacturaID;
    END
END


-- SP DETALLE FACTURA
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE sp_DetalleFactura
    @Operacion CHAR(1), -- 'C' para Crear, 'R' para Leer, 'U' para Actualizar, 'D' para Eliminar
    @DetalleID INT,
    @FacturaID INT,
    @ArticuloID INT,
    @Cantidad INT,
    @PrecioUnitario DECIMAL(10, 2),
    @Subtotal DECIMAL(10, 2)
AS
BEGIN
    IF @Operacion = 'C'
    BEGIN
        -- Crear nuevo detalle
        INSERT INTO DetalleFactura (FacturaID, ArticuloID, Cantidad, PrecioUnitario, Subtotal)
        VALUES (@FacturaID, @ArticuloID, @Cantidad, @PrecioUnitario, @Subtotal)
		
		-- Actualizar el total de la factura sumando el subtotal del nuevo detalle
        UPDATE EncabezadoFactura
        SET TotalFactura = TotalFactura + @Subtotal
        WHERE FacturaID = @FacturaID;
    END
    ELSE IF @Operacion = 'R'
    BEGIN
        -- Leer detalles de una factura específica
        SELECT * FROM DetalleFactura WHERE FacturaID = @FacturaID;
    END
    ELSE IF @Operacion = 'U'
    BEGIN
        -- No permitir actualizar el detalle, ya que debe ser inmutable
        -- Si es necesario modificar el detalle, se debería eliminar y crear uno nuevo.
        RAISERROR('No se permite actualizar el detalle de la factura. Si es necesario, elimine y cree uno nuevo.', 16, 1);
        RETURN;
    END
    ELSE IF @Operacion = 'D'
    BEGIN
        -- Obtener el subtotal del detalle a eliminar
        DECLARE @SubtotalDetalle DECIMAL(10, 2);
        SELECT @SubtotalDetalle = Subtotal FROM DetalleFactura WHERE DetalleID = @DetalleID;

        -- Eliminar el detalle específico
        DELETE FROM DetalleFactura WHERE DetalleID = @DetalleID;

        -- Actualizar el total de la factura restando el subtotal del detalle eliminado
        UPDATE EncabezadoFactura
        SET TotalFactura = TotalFactura - @SubtotalDetalle
        WHERE FacturaID = @FacturaID;
    END
END
