USE SistemaBiblioteca;
GO

-- 4. Crear tabla Usuarios
CREATE TABLE Usuarios (
    IdUsuario INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Documento NVARCHAR(20) NOT NULL UNIQUE, 
    Telefono NVARCHAR(20),
    Correo NVARCHAR(100)
);
GO

-- 5. Crear tabla Libros con claves foráneas
CREATE TABLE Libros (
    IdLibro INT PRIMARY KEY IDENTITY(1,1),
    Codigo_ISBN NVARCHAR(50) NOT NULL UNIQUE, 
    Titulo NVARCHAR(200) NOT NULL,
    Existencias INT NOT NULL DEFAULT 1,
    IdAutor INT NOT NULL,
    IdEditorial INT NOT NULL,
    FOREIGN KEY (IdAutor) REFERENCES Autores(IdAutor),
    FOREIGN KEY (IdEditorial) REFERENCES Editoriales(IdEditorial)
);
GO

-- 6. Crear tabla Prestamos con claves foráneas
CREATE TABLE Prestamos (
    IdPrestamo INT PRIMARY KEY IDENTITY(1,1),
    IdUsuario INT NOT NULL,
    IdLibro INT NOT NULL,
    FechaPrestamo DATE NOT NULL,
    FechaDevolucion DATE,
    Estado NVARCHAR(20) DEFAULT 'Prestado', 
    FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario),
    FOREIGN KEY (IdLibro) REFERENCES Libros(IdLibro)
);
GO

-- 7. Inserción de datos de prueba
INSERT INTO Editoriales (Nombre, Pais) VALUES ('Oveja Negra', 'Colombia'), ('Norma', 'Colombia');
INSERT INTO Autores (Nombre, Apellido) VALUES ('Gabriel', 'Garcia Marquez'), ('Mario', 'Mendoza');
INSERT INTO Usuarios (Nombre, Apellido, Documento, Telefono, Correo) VALUES ('Carlos', 'Ramirez', '10203040', '3000000000', 'carlos@email.com');
INSERT INTO Libros (Codigo_ISBN, Titulo, Existencias, IdAutor, IdEditorial) VALUES ('ISBN-1001', 'Cien años de soledad', 5, 1, 1), ('ISBN-1002', 'Satanas', 3, 2, 2);
GO