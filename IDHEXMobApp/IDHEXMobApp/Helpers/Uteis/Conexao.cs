namespace IDHEXMobApp.Helpers.Uteis
{
    public static class Conexao
    {
        public static bool CheckConnectivity()
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet || current == NetworkAccess.ConstrainedInternet)
            {
                return true; 
            }
            else
            {
                return false; 
            }
        }
    }
}
