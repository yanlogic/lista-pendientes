using lista_pendientes;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace listas_pendientes
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private ObservableCollection<Tarea> tareas = new ObservableCollection<Tarea>();
        private ObservableCollection<Tarea> completadas = new ObservableCollection<Tarea>();

        private List<Fuente> fuentesEjemplo = new List<Fuente>
        {
            new Fuente { Titulo = "SciELO", Enlace = new Uri("https://scielo.org") },
            new Fuente { Titulo = "Redalyc", Enlace = new Uri("https://www.redalyc.org") },
            new Fuente { Titulo = "Dialnet", Enlace = new Uri("https://dialnet.unirioja.es") },
            new Fuente { Titulo = "Google Académico", Enlace = new Uri("https://scholar.google.com") }
        };

        public MainWindow()
        {
            InitializeComponent();
            ListaTareas.ItemsSource = tareas;
            TareasCompletadas.ItemsSource = completadas;
            foreach (Tarea t in almacen_tareas.Cargar())
            {
                if (t.Completada)
                    completadas.Add(t);
                else
                    tareas.Add(t);
            }
            ListaFuentes.ItemsSource = fuentesEjemplo;
            ListaRecetas.ItemsSource = ideasCocina;
        }

        private void BotonAgregar_Click(object sender, RoutedEventArgs e)
        {
            string texto = CajaTarea.Text.Trim();
            if (texto == "") return;

            tareas.Add(new Tarea { Nombre = texto });
            CajaTarea.Text = "";
            GuardarTodo();
        }

        private void BotonEliminar_Click(object sender, RoutedEventArgs e)
        {
            Button boton = (Button)sender;
            Tarea tarea = (Tarea)boton.DataContext;
            tareas.Remove(tarea);
            GuardarTodo();
        }

        private void Fila_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Grid fila = (Grid)sender;
            Button BotonEliminar = (Button)fila.Children[1];

            if (BotonEliminar != null)
            {
                BotonEliminar.Opacity = 1;
            }
        }

        private void Fila_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Grid fila = (Grid)sender;
            Button BotonEliminar = (Button)fila.Children[1];

            if (BotonEliminar != null)
            {
                BotonEliminar.Opacity = 0;
            }
        }

        private void ListaTareas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Tarea? tareaElegida = ListaTareas.SelectedItem as Tarea;

            PanelAcademico.Visibility = Visibility.Collapsed;
            PanelAireLibre.Visibility = Visibility.Collapsed;
            PanelCocina.Visibility = Visibility.Collapsed;

            if (tareaElegida == null)
            {
                TituloPanel.Text = "Selecciona una tarea";
                TipoPanel.Text = "";
                return;
            }

            TituloPanel.Text = tareaElegida.Nombre;
            Tipo_tarea tipo = detector_tipo.Detectar(tareaElegida.Nombre);
            TipoPanel.Text = "Tipo: " + tipo;

            if (tipo == Tipo_tarea.academica)
            {
                PanelAcademico.Visibility = Visibility.Visible;
                CajaTema.Text = tareaElegida.Tema;
                ListaFuentes.ItemsSource = fuentesEjemplo;
                EstadoFuentes.Text = "";
            }
            else if (tipo == Tipo_tarea.aireLibre)
            {
                PanelAireLibre.Visibility = Visibility.Visible;
                TipAireLibre.Text = tipsAireLibre[azar.Next(tipsAireLibre.Length)];
            }
            else if (tipo == Tipo_tarea.cocina)
            {
                PanelCocina.Visibility = Visibility.Visible;
                string busqueda = Uri.EscapeDataString("recetas " + tareaElegida.Nombre);
                EnlaceRecetas.NavigateUri = new Uri("https://www.google.com/search?q=" + busqueda);
            }
        }

        private void CajaTema_TextChanged(object sender, TextChangedEventArgs e)
        {
            Tarea? tareaElegida = ListaTareas.SelectedItem as Tarea;
            if (tareaElegida == null) return;

            tareaElegida.Tema = CajaTema.Text;
            GuardarTodo();
        }

        private void BotonCompletar_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            Tarea tarea = (Tarea)checkBox.DataContext;

            if (tarea.Completada)
            {
                tareas.Remove(tarea);
                completadas.Add(tarea);
            }
            GuardarTodo();
        }

        private void GuardarTodo()
        {
            List<Tarea> todas = new List<Tarea>();
            todas.AddRange(tareas);
            todas.AddRange(completadas);
            almacen_tareas.Guardar(todas);
        }

        private Random azar = new Random();

        private string[] tipsAireLibre =
        {
            "Si el piso está muy caliente para tu mano, también lo está para las patitas de tu perro.",
            "Lleva agua para ti y para tu mascota, aunque el paseo sea corto.",
            "Los paseos con olfateo cansan más a un perro que correr: déjalo oler con calma.",
            "Salir a caminar 20 minutos ya mejora el ánimo y la concentración."
        };

        private string[] ideasCocina =
        {
            "Arroz con pollo",
            "Tallarines verdes",
            "Tortilla de verduras",
            "Lomo saltado",
            "Sopa de verduras con fideos"
        };

        private async void BotonConfiguracion_Click(object sender, RoutedEventArgs e)
        {
            TextBlock explicacion = new TextBlock
            {
                Text = "Opcional. Sin clave la app funciona igual. Con una clave gratuita de Google AI Studio podrá corregir tildes y sugerir recetas. Los textos de tus tareas se enviarían a Google.",
                TextWrapping = TextWrapping.Wrap
            };

            HyperlinkButton enlace = new HyperlinkButton
            {
                Content = "Conseguir una clave gratis",
                NavigateUri = new Uri("https://aistudio.google.com/apikey")
            };

            PasswordBox cajaClave = new PasswordBox
            {
                PlaceholderText = "Pega aquí tu clave",
                Password = configuracion.LeerClave()
            };

            TextBox cajaModelo = new TextBox
            {
                Header = "Modelo",
                Text = configuracion.LeerModelo()
            };

            StackPanel contenido = new StackPanel { Spacing = 8 };
            contenido.Children.Add(explicacion);
            contenido.Children.Add(enlace);
            contenido.Children.Add(cajaClave);
            contenido.Children.Add(cajaModelo);

            ContentDialog dialogo = new ContentDialog
            {
                Title = "IA (opcional)",
                Content = contenido,
                PrimaryButtonText = "Guardar",
                CloseButtonText = "Cancelar",
                XamlRoot = this.Content.XamlRoot
            };

            ContentDialogResult resultado = await dialogo.ShowAsync();

            if (resultado == ContentDialogResult.Primary)
            {
                configuracion.GuardarClave(cajaClave.Password.Trim());

                string modelo = cajaModelo.Text.Trim();
                configuracion.GuardarModelo(modelo == "" ? "gemini-3.7-flash" : modelo);
            }
        }

        private async void BotonBuscarFuentes_Click(object sender, RoutedEventArgs e)
        {
            Tarea? tareaElegida = ListaTareas.SelectedItem as Tarea;
            if (tareaElegida == null) return;

            string tema = CajaTema.Text.Trim();
            if (tema == "") tema = tareaElegida.Nombre;

            BotonBuscarFuentes.IsEnabled = false;
            EstadoFuentes.Text = "Buscando...";

            try
            {
                List<Fuente> encontradas = await buscar_fuentes.Buscar(tema);
                ListaFuentes.ItemsSource = encontradas;
                EstadoFuentes.Text = encontradas.Count == 0 ? "No se encontraron fuentes para ese tema." : "";
            }
            catch
            {
                EstadoFuentes.Text = "No se pudo buscar. Revisa tu conexión a internet.";
            }

            BotonBuscarFuentes.IsEnabled = true;
        }
    }
}
