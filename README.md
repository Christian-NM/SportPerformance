# SportPerformance - Gestión de Entrenamientos

Aplicación de escritorio desarrollada en **C#** y **WPF** bajo la arquitectura **MVVM (Model-View-ViewModel)**. Diseñada para el seguimiento exhaustivo de ejercicios, sesiones de entrenamiento y registro de series (pesos y repeticiones).

## 🚀 Características Principales
* **Arquitectura Profesional:** Implementación estricta de MVVM para una separación clara entre la lógica de negocio y la interfaz de usuario.
* **Persistencia de Datos:** Uso de **SQLite** para un almacenamiento ligero, rápido y local sin dependencias de servidores externos.
* **Gestión CRUD Completa:** * Catálogo de ejercicios clasificados por grupo muscular.
    * Historial de sesiones de entrenamiento por fecha.
    * Registro de series vinculadas a ejercicios y entrenamientos específicos.
* **Interfaz Moderna:** Diseño basado en XAML con navegación fluida entre módulos.

## 🛠️ Tecnologías Utilizadas
* **Lenguaje:** C# (.NET Core / .NET 6-8)
* **Framework UI:** WPF (Windows Presentation Foundation)
* **Base de Datos:** SQLite con `System.Data.SQLite`
* **Patrón de Diseño:** MVVM
* **Herramientas:** Visual Studio 2022, Git

## 📂 Estructura del Proyecto
* `Models/`: Definición de las entidades de datos (Ejercicio, Entrenamiento, Serie).
* `ViewModels/`: Lógica de la aplicación y comandos (`RelayCommand`).
* `Views/`: Definición visual en XAML.
* `Helpers/`: Clases de utilidad para la conexión a base de datos (`DatabaseHelper`).

## ⚙️ Instalación y Uso
1. Clonar el repositorio: `git clone https://github.com/TU_USUARIO/SportPerformance.git`
2. Abrir el archivo `.sln` en **Visual Studio**.
3. Restaurar los paquetes NuGet (clic derecho en la solución -> Restaurar paquetes NuGet).
4. Ejecutar el proyecto (F5). La base de datos se inicializará automáticamente en la primera ejecución.

---
*Proyecto desarrollado como parte de un proceso de formación técnica y portafolio profesional.*