# SportPerformance

Aplicación de escritorio WPF para la gestión integral de entrenamientos deportivos, desarrollada bajo la arquitectura MVVM (Model-View-ViewModel) para garantizar la separación de responsabilidades y la mantenibilidad del código.

## Arquitectura y Tecnologías
* **Framework:** .NET 8.0 (WPF)
* **Patrón de Diseño:** MVVM (DataBinding, ICommand, INotifyPropertyChanged)
* **Base de Datos:** SQLite (Relacional, Local)
* **Acceso a Datos:** ADO.NET puro (`System.Data.SQLite`)

## Módulos Principales
* **Dashboard:** Panel de control con métricas agregadas mediante consultas escalares.
* **Catálogo de Ejercicios:** Mantenimiento (CRUD) del diccionario de ejercicios.
* **Historial de Sesiones:** Registro cronológico de entrenamientos.
* **Gestión de Series:** Módulo transaccional que establece la relación entre ejercicios y sesiones, implementando integridad referencial y eliminación en cascada (`ON DELETE CASCADE`).

## Estructura Relacional
1. `EJERCICIOS` (id_ejercicio [PK], nombre, musculo)
2. `ENTRENAMIENTOS` (id_entrenamiento [PK], fecha, comentario)
3. `SERIES` (id_serie [PK], id_ejercicio [FK], id_entrenamiento [FK], peso, repeticiones)