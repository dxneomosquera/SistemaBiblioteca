-- 1. Crear y usar la base de datos
CREATE DATABASE SistemaBiblioteca;
GO

USE SistemaBiblioteca;
GO

-- 2. Tabla Autores
CREATE TABLE Autores (
    IdAutor INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL,
    Apellido NVARCHAR(50) NOT NULL
);

-- 3. Tabla Editoriales
CREATE TABLE Editoriales (
    IdEditorial INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL UNIQUE
);

-- 4. Tabla Libros
CREATE TABLE Libros (
    IdLibro INT IDENTITY(1,1) PRIMARY KEY,
    Codigo_ISBN NVARCHAR(20) NOT NULL UNIQUE,
    Titulo NVARCHAR(150) NOT NULL,
    Existencias INT NOT NULL CHECK (Existencias >= 0),
    IdAutor INT NOT NULL,
    IdEditorial INT NOT NULL,
    CONSTRAINT FK_Libros_Autores FOREIGN KEY (IdAutor) REFERENCES Autores(IdAutor),
    CONSTRAINT FK_Libros_Editoriales FOREIGN KEY (IdEditorial) REFERENCES Editoriales(IdEditorial)
);

-- 5. Tabla Usuarios
CREATE TABLE Usuarios (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    Documento NVARCHAR(20) NOT NULL UNIQUE,
    Nombre NVARCHAR(50) NOT NULL,
    Apellido NVARCHAR(50) NOT NULL,
    Telefono NVARCHAR(15) NULL,
    Correo NVARCHAR(100) NULL
);

-- 6. Tabla Préstamos
CREATE TABLE Prestamos (
    IdPrestamo INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL,
    IdLibro INT NOT NULL,
    FechaPrestamo DATE NOT NULL,
    FechaDevolucion DATE NULL,
    Estado NVARCHAR(20) NOT NULL CHECK (Estado IN ('Activo', 'Devuelto')),
    CONSTRAINT FK_Prestamos_Usuarios FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario),
    CONSTRAINT FK_Prestamos_Libros FOREIGN KEY (IdLibro) REFERENCES Libros(IdLibro)
);

-- 7. INSERCIÓN DE DATOS DE PRUEBA
INSERT INTO Autores (Nombre, Apellido) VALUES 
('Gabriel', 'García Márquez'),
('J.R.R.', 'Tolkien');

INSERT INTO Editoriales (Nombre) VALUES 
('Editorial Norma'),
('Minotauro');

INSERT INTO Libros (Codigo_ISBN, Titulo, Existencias, IdAutor, IdEditorial) VALUES 
('978-3-16-148410-0', 'Cien años de soledad', 5, 1, 1),
('978-0-261-10238-5', 'El Señor de los Anillos', 3, 2, 2);

INSERT INTO Usuarios (Documento, Nombre, Apellido, Telefono, Correo) VALUES 
('10203040', 'Carlos', 'Pérez', '3001234567', 'carlos@email.com'),
('50607080', 'Ana', 'Gómez', '3109876543', 'ana@email.com');

INSERT INTO Prestamos (IdUsuario, IdLibro, FechaPrestamo, FechaDevolucion, Estado) VALUES 
(1, 1, GETDATE(), NULL, 'Activo'),
(2, 2, GETDATE(), GETDATE(), 'Devuelto');
GO