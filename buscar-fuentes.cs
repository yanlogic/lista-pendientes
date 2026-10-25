using listas_pendientes;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;

namespace lista_pendientes
{
    public class buscar_fuentes
    {
        private static readonly HttpClient cliente = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
        public static async Task<List<Fuente>> Buscar(string tema)
        {
            string url = "https://api.crossref.org/works?rows=5&select=title,URL&query="
                         + Uri.EscapeDataString(tema);

            string json = await cliente.GetStringAsync(url);

            using JsonDocument documento = JsonDocument.Parse(json);
            JsonElement items = documento.RootElement.GetProperty("message").GetProperty("items");

            List<Fuente> resultado = new List<Fuente>();

            foreach (JsonElement item in items.EnumerateArray())
            {
                if (!item.TryGetProperty("title", out JsonElement titulos)) continue;
                if (titulos.GetArrayLength() == 0) continue;
                if (!item.TryGetProperty("URL", out JsonElement enlace)) continue;

                string titulo = titulos[0].GetString() ?? "";
                string? direccion = enlace.GetString();
                if (titulo == "" || direccion == null) continue;

                resultado.Add(new Fuente { Titulo = titulo, Enlace = new Uri(direccion) });
            }

            return resultado;
        }
    }
}
