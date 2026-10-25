using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System;

namespace lista_pendientes
{
    internal class service_ia
    {
        private static readonly HttpClient cliente = new HttpClient { Timeout = TimeSpan.FromSeconds(20) };
        public static async Task<string> Preguntar(string instruccion)
        {
            string modelo = configuracion.LeerModelo();
            string url = "https://generativelanguage.googleapis.com/v1beta/models/"
                         + modelo + ":generateContent";

            string cuerpo = JsonSerializer.Serialize(new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = instruccion } } }
                }
            });

            using HttpRequestMessage pedido = new HttpRequestMessage(HttpMethod.Post, url);
            pedido.Headers.Add("x-goog-api-key", configuracion.LeerClave());
            pedido.Content = new StringContent(cuerpo, Encoding.UTF8, "application/json");

            using HttpResponseMessage respuesta = await cliente.SendAsync(pedido);
            string json = await respuesta.Content.ReadAsStringAsync();

            if (!respuesta.IsSuccessStatusCode)
                throw new Exception("Error " + (int)respuesta.StatusCode);

            using JsonDocument documento = JsonDocument.Parse(json);
            return documento.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? "";
        }

        public static async Task<string> CorregirTildes(string texto)
        {
            string instruccion = "Corrige solo las tildes y la ortografía de este texto en español. "
                                 + "Responde únicamente con el texto corregido, sin comillas ni explicaciones: "
                                 + texto;

            string respuesta = (await Preguntar(instruccion)).Trim();

            if (respuesta == "" || respuesta.Length > texto.Length * 2 + 20)
                return texto;

            return respuesta;
        }

        public static async Task<Tipo_tarea> ClasificarTipo(string nombreTarea)
        {
            string instruccion = "Clasifica esta tarea en una sola categoría: "
                + "Academica (trabajos, estudios, exámenes), "
                + "AireLibre (pasear, ejercicio al aire libre, parques), "
                + "Cocina (cocinar, comidas) o "
                + "General (cualquier otra cosa). "
                + "Responde únicamente con una de esas cuatro palabras. Tarea: " + nombreTarea;

            string respuesta = (await Preguntar(instruccion)).Trim();

            if (Enum.TryParse(respuesta, true, out Tipo_tarea tipo) && Enum.IsDefined(tipo))
                return tipo;

            return Tipo_tarea.general;
        }
    }
}
