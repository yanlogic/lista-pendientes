using Windows.Security.Credentials;
using Windows.Storage;

namespace lista_pendientes
{
    internal class configuracion
    {
        private const string resourceName = "ListaPendientes.IA";
        private const string userName = "api_key";

        public static void GuardarClave(string clave)
        {
            BorrarClave();
            if (clave == "") return;

            PasswordVault boveda = new PasswordVault();
            boveda.Add(new PasswordCredential(resourceName, userName, clave));
        }

        public static string LeerClave()
        {
            try
            {
                PasswordVault boveda = new PasswordVault();
                PasswordCredential credencial = boveda.Retrieve(resourceName, userName);
                credencial.RetrievePassword();
                return credencial.Password;
            }
            catch
            {
                return "";
            }
        }

        public static void BorrarClave()
        {
            try
            {
                PasswordVault boveda = new PasswordVault();
                PasswordCredential credencial = boveda.Retrieve(resourceName, userName);
                boveda.Remove(credencial);
            }
            catch
            {
            }
        }
        public static string LeerModelo()
        {
            object? valor = ApplicationData.Current.LocalSettings.Values["modelo"];
            return valor as string ?? "gemini-3.7-flash";
        }

        public static void GuardarModelo(string modelo)
        {
            ApplicationData.Current.LocalSettings.Values["modelo"] = modelo;
        }

        public static bool HayClave()
        {
            return LeerClave() != "";
        }
    }
}
