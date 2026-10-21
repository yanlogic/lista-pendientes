# Lista de pendientes con panel inteligente

Aplicación de escritorio para Windows (WinUI 3, C#) que guarda tareas pendientes y, al seleccionar una, muestra a la derecha un panel de ayuda según el tipo de tarea.

## Capturas

![Captura 1 de la Versión 1](capturas/v1_1.png)
![Captura 2 de la Versión 1](capturas/v1_2.png)
![Captura 3 de la Versión 1](capturas/v1_3.png)

## Problema
Las apps de pendientes solo guardan una lista; no ayudan a hacer la tarea.

## Estado actual
**Versión 1 terminada.** Lista de pendientes y de completadas, con guardado automático.

## Qué hace la Versión 1
- Agregar una tarea escribiendo su nombre.
- Marcar una tarea con el check: pasa a la columna "Tareas Completadas".
- Eliminar una tarea con la X, que solo aparece al pasar el mouse sobre la fila.
- Seleccionar una tarea (clic en su nombre) y ver su título en el panel derecho.
- Guardar automáticamente después de cada cambio y cargar las tareas al abrir la app.

## Versiones planeadas
| Versión | Qué incluye |
|---|---|
| 1 | Lo descrito arriba (terminada) |
| 2 | Detectar el tipo de tarea por palabras clave + panel académico (apps sugeridas, tema, fuentes) |
| 3 | Búsqueda de fuentes por API, elegir modelo de IA con API key propia, paneles de aire libre y cocina |

## Cómo está organizado el código
| Archivo | Qué contiene |
|---|---|
| `MainWindow.xaml` | El diseño: 3 columnas (completadas, pendientes con caja y botón, panel derecho) |
| `MainWindow.xaml.cs` | Lo que hace cada botón y evento de la ventana |
| `Tarea.cs` | La clase `Tarea`: guarda el nombre y si está completada |
| `almacen_tareas.cs` | Clase estática con `Guardar` y `Cargar` (archivo JSON) |

### Funciones principales
| Función | Qué hace |
|---|---|
| `BotonAgregar_Click` | Crea una tarea con el texto de la caja y la agrega a pendientes |
| `BotonCompletar_Click` | Pasa la tarea marcada de pendientes a completadas |
| `BotonEliminar_Click` | Quita la tarea de su fila |
| `Fila_PointerEntered` / `Fila_PointerExited` | Muestran y ocultan la X al pasar el mouse |
| `ListaTareas_SelectionChanged` | Muestra en el panel derecho el nombre de la tarea elegida |
| `GuardarTodo` | Junta pendientes y completadas y las guarda |
| `almacen_tareas.Guardar` / `Cargar` | Escriben y leen `tareas.json` en la carpeta local de la app |

## Tecnologías
- C# y WinUI 3 (Windows App SDK)
- ObservableCollection para que la lista se actualice sola
- JSON (System.Text.Json) para guardar las tareas
- Visual Studio Community

## Cómo ejecutarlo
1. Instala Visual Studio con la carga de trabajo **Desarrollo de escritorio con .NET** y los **Windows App SDK C# Templates**.
2. Clona este repositorio y abre el archivo `.sln`.
3. Presiona el botón verde **Iniciar**.

## Autora
Yanta (GitHub: [yanlogic](https://github.com/yanlogic))
