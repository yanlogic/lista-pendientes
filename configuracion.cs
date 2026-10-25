using Windows.Security.Credentials;

namespace lista_pendientes
{
    internal class configuracion
    {
        private const string resourceName = "ListaPendientes.Tavily";
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
    }
}
