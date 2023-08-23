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

INSERT INTO Categoria (Descripcion, Estado)
VALUES ('Bespoke', 1);

INSERT INTO Categoria (Descripcion, Estado)
VALUES ('Aspiradoras', 1);

INSERT INTO Categoria (Descripcion, Estado)
VALUES ('Refrigeradoras', 1);

INSERT INTO Categoria (Descripcion, Estado)
VALUES ('Lavadoras y Secadoras', 1);

INSERT INTO Categoria (Descripcion, Estado)
VALUES ('Cocinas', 1);

INSERT INTO Categoria (Descripcion, Estado)
VALUES ('Lavavajillas', 1);

INSERT INTO Categoria (Descripcion, Estado)
VALUES ('Microondas', 1);

INSERT INTO Categoria (Descripcion, Estado)
VALUES ('Electrodomesticos Pequeños', 1);

SELECT * FROM Categoria

-- Crear tabla Articulo
CREATE TABLE Articulo (
    IdArticulo bigint IDENTITY(1,1) NOT NULL PRIMARY KEY,
    IdCategoria bigint NOT NULL,
    Descripcion varchar(50) NOT NULL,
    Detalle varchar(50) NOT NULL,
    Precio decimal(18,2) NOT NULL,
    Existencia int NOT NULL,
    Estado bit NOT NULL,
    FOREIGN KEY (IdCategoria) REFERENCES Categoria (IdCategoria) -- Clave foránea a la tabla Categoria
);



INSERT INTO Articulo (IdCategoria, Descripcion, Detalle, Precio, Existencia, Estado)
VALUES (3, 'Refrigeradora GE', 'GBE21DGKBB',  1449.00, 1, 1);

INSERT INTO Articulo (IdCategoria, Descripcion, Detalle, Precio, Existencia, Estado)
VALUES (3, 'Refrigeradora Samsung', 'RH68B8841S9EF',  1199.00, 1, 1);

INSERT INTO Articulo (IdCategoria, Descripcion, Detalle, Precio, Existencia, Estado)
VALUES (3, 'Refrigeradora LG', 'LRSOS2706D',  2332.00, 1, 1);

INSERT INTO Articulo (IdCategoria, Descripcion, Detalle, Precio, Existencia, Estado)
VALUES (2, 'Aspiradora Samsung', 'VS28C9784QK',  2332.00, 1, 1);

INSERT INTO Articulo (IdCategoria, Descripcion, Detalle, Precio, Existencia, Estado)
VALUES (3, 'Refrigeradora Bespoke Samsung', 'RB38A7B6D22EF',  999.00, 1, 1);

SELECT * FROM Articulo;


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
    IdArticulo bigint NOT NULL,
    Nombre varchar(20) NOT NULL,
    Apellidos varchar(20) NOT NULL,
    Email varchar(20) NOT NULL,
    Telefono varchar(15) NOT NULL,
    PrecioPago decimal(18,2) NOT NULL,
    FechaPago datetime NOT NULL,
    FOREIGN KEY (IdArticulo) REFERENCES Articulo (IdArticulo),
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

-- Crear tabla carrito
CREATE TABLE Carrito (
    IdCarrito bigint IDENTITY(1,1) NOT NULL PRIMARY KEY,
    IdCliente bigint NOT NULL,
    IdArticulo bigint NOT NULL,
    Fecha datetime NOT NULL,
    FOREIGN KEY (IdCliente) REFERENCES Cliente (IdCliente), -- Clave foránea a la tabla Cliente
    FOREIGN KEY (IdArticulo) REFERENCES Articulo (IdArticulo) -- Clave foránea a la tabla Articulo
);

-- Procedimientos Almacenados
GO
CREATE PROCEDURE [dbo].[RegistrarBitacora] 
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