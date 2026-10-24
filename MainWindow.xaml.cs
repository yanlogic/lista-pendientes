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

            if (tareaElegida == null)
            {
                TituloPanel.Text = "Selecciona una tarea";
                TipoPanel.Text = "";
                PanelAcademico.Visibility = Visibility.Collapsed;
            }
            else
            {
                TituloPanel.Text = tareaElegida.Nombre;
                Tipo_tarea tipo = detector_tipo.Detectar(tareaElegida.Nombre);
                TipoPanel.Text = "Tipo: " + tipo;

                if (tipo == Tipo_tarea.academica)
                {
                    PanelAcademico.Visibility = Visibility.Visible;
                    CajaTema.Text = tareaElegida.Tema;
                }
                else
                {
                    PanelAcademico.Visibility = Visibility.Collapsed;
                }
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
    }
}
