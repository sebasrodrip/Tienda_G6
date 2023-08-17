-- Crear la base de datos
CREATE DATABASE Tienda_G6;
GO

-- Usar la base de datos recién creada
USE Tienda_G6;
GO

-- Crear tabla Categoria
CREATE TABLE Categoria (
    IdCategoria bigint IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Descripcion varchar(30) NOT NULL,
    Estado bit NOT NULL
);

-- Crear tabla Articulo
CREATE TABLE Articulo (
    IdArticulo bigint IDENTITY(1,1) NOT NULL PRIMARY KEY,
    IdCategoria bigint NOT NULL,
    Descripcion varchar(20) NOT NULL,
    Detalle varchar(20) NOT NULL,
    Precio float NOT NULL,
    Existencia int NOT NULL,
    Estado bit NOT NULL,
    FOREIGN KEY (IdCategoria) REFERENCES Categoria (IdCategoria) -- Clave foránea a la tabla Categoria
);

-- Crear tabla Rol
CREATE TABLE Rol (
    IdRol int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    NombreRol varchar(20) NOT NULL
);

-- Insertar el rol "cliente"
INSERT INTO Rol (NombreRol)
VALUES ('ADMINISTRADOR');

-- Insertar el rol "administrador"
INSERT INTO Rol (NombreRol)
VALUES ('NORMAL');

-- Insertar el rol "normal"
INSERT INTO Rol (NombreRol)
VALUES ('CLIENTE');

-- Crear tabla Usuario
CREATE TABLE Usuario (
    IdUsuario bigint IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Identificacion varchar(20) NOT NULL,
    Nombre varchar(100) NOT NULL,
    Email varchar(50) NOT NULL,
    Contrasenna varchar(MAX) NOT NULL,
    Estado bit NOT NULL,
    IdRol int NOT NULL,
	ClaveTemporal bit NULL,
	Caducidad datetime NULL,
    FOREIGN KEY (IdRol) REFERENCES Rol (IdRol) -- Clave foránea a la tabla Rol
);

select * from Usuario

ALTER TABLE Usuario
ADD CONSTRAINT UQ_Identificacion_Usuario UNIQUE (Identificacion);

ALTER TABLE Usuario
ADD CONSTRAINT UQ_Email_Usuario UNIQUE (Email);

-- Crear tabla Credito
CREATE TABLE Credito (
    IdCredito int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Limite float NOT NULL
);

-- Insertar un crédito de 300 mil colones
INSERT INTO Credito (Limite)
VALUES (300000);

-- Insertar un crédito de 500 mil colones
INSERT INTO Credito (Limite)
VALUES (500000);

-- Insertar un crédito de 1 millón de colones
INSERT INTO Credito (Limite)
VALUES (1000000);

-- Crear tabla Cliente
CREATE TABLE Cliente (
    IdCliente bigint IDENTITY(1,1) NOT NULL PRIMARY KEY,
    IdUsuario bigint NOT NULL,
    IdCredito int NOT NULL DEFAULT 1,
    Nombre varchar(20) NOT NULL,
    Apellidos varchar(20) NOT NULL,
    Email varchar(20) NOT NULL,
    Telefono varchar(15) NOT NULL,
    FOREIGN KEY (IdUsuario) REFERENCES Usuario (IdUsuario), -- Clave foránea a la tabla Usuario
    FOREIGN KEY (IdCredito) REFERENCES Credito (IdCredito) -- Clave foránea a la tabla Credito
);

-- Crear tabla Factura
CREATE TABLE Factura (
    IdFactura bigint IDENTITY(1,1) NOT NULL PRIMARY KEY,
    IdCliente bigint NOT NULL,
    Fecha date NOT NULL,
    Total float NOT NULL,
    Estado int NOT NULL,
    FOREIGN KEY (IdCliente) REFERENCES Cliente (IdCliente) -- Clave foránea a la tabla Cliente
);

-- Crear tabla Venta
CREATE TABLE Venta (
    IdVenta bigint IDENTITY(1,1) NOT NULL PRIMARY KEY,
    IdFactura bigint NOT NULL,
    IdArticulo bigint NOT NULL,
    Precio float NOT NULL,
    Cantidad int NOT NULL,
    FOREIGN KEY (IdFactura) REFERENCES Factura (IdFactura), -- Clave foránea a la tabla Factura
    FOREIGN KEY (IdArticulo) REFERENCES Articulo (IdArticulo) -- Clave foránea a la tabla Articulo
);

-- Crear tabla Bitacora
CREATE TABLE Bitacora (
    IdBitacora bigint IDENTITY(1,1) NOT NULL PRIMARY KEY,
    FechaHora datetime NOT NULL,
    MensajeError varchar(5000) NOT NULL,
    Origen varchar(5000) NOT NULL,
    IdUsuario bigint NOT NULL,
    DireccionIP varchar(50) NOT NULL,
    FOREIGN KEY (IdUsuario) REFERENCES Usuario (IdUsuario) -- Clave foránea a la tabla Usuario
);

-- Procedimientos Almacenados
CREATE OR ALTER PROCEDURE [dbo].[RegistrarBitacora] 
	@MensajeError	VARCHAR(5000), 
	@Origen			VARCHAR(5000), 
	@IdUsuario		BIGINT, 
	@DireccionIP	VARCHAR(50)
AS
BEGIN

	INSERT INTO dbo.Bitacora(FechaHora,MensajeError,Origen,IdUsuario,DireccionIP)
    VALUES (GETDATE(),@MensajeError, @Origen, @IdUsuario, @DireccionIP)

END
GO

CREATE OR ALTER PROCEDURE [dbo].[RegistrarUsuario] 
	@Email VARCHAR(50),
    @Contrasenna VARCHAR(25),
    @Identificacion VARCHAR(20),
    @Nombre VARCHAR(100),
    @Estado BIT,
    @IdRol INT
AS
BEGIN
	
	INSERT INTO dbo.Usuario(Email,Contrasenna,Identificacion,Nombre,Estado,IdRol,ClaveTemporal,Caducidad)
    VALUES (@Email,@Contrasenna,@Identificacion,@Nombre,@Estado,@IdRol,0,GETDATE())

END
GO