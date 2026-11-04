# Sistema de Gestión de Biblioteca

## Descripción
Aplicación de escritorio desarrollada en C# (Windows Forms) con conexión a SQL Server mediante ADO.NET. El sistema automatiza y administra los procesos operativos de una biblioteca, garantizando la integridad referencial y la gestión fluida del inventario bibliográfico.

## Arquitectura del Sistema
El proyecto implementa los principios de la Programación Orientada a Objetos (POO) y separa las responsabilidades lógicas y de almacenamiento en capas:
- **Capa de Presentación:** Formularios de Windows Forms para la captura de datos, manejo de eventos y validaciones iniciales de usuario.
- **Capa de Acceso a Datos:** Clase especializada (`ConexionBD.cs`) que administra la comunicación con SQL Server y la ejecución de consultas parametrizadas para evitar inyecciones SQL.
- **Capa de Base de Datos:** Motor relacional Microsoft SQL Server que asegura la persistencia, normalización y las reglas de integridad del modelo.

## Módulos Implementados (CRUD)
1. **Libros:** Gestión bibliográfica con relaciones foráneas a autores y editoriales.
2. **Autores y Editoriales:** Administración de catálogos base con restricciones de unicidad.
3. **Usuarios:** Registro de miembros habilitados con validación de documento único.
4. **Préstamos:** Registro de operaciones de salida y devolución afectando el estado dinámico (Activo/Devuelto).
5. **Reportes:** Visualización consolidada en tiempo real de los préstamos activos.

## Instrucciones de Configuración y Ejecución
1. Clonar el repositorio en el equipo local.
2. Abrir SQL Server Management Studio (SSMS) y ejecutar el archivo adjunto `Script_SistemaBiblioteca.sql` para generar la base de datos, las tablas y los datos de prueba iniciales.
3. Abrir la solución `SistemaBiblioteca.sln` utilizando Visual Studio 2022.
4. Si es necesario, ajustar la cadena de conexión en el archivo `ConexionBD.cs` según la instancia local de SQL Server del evaluador (ej. `(localdb)\MSSQLLocalDB` o `.\SQLEXPRESS`).
5. Compilar la solución para restaurar paquetes y ejecutar el proyecto (F5).