using System.Net.NetworkInformation;

namespace DentalPlus.InternetConnection
{
    public class InternetConnection
    {
        public bool VerificarConexaoInternet()
        {
            try
            {
                using (var ping = new Ping())
                {
                    PingReply reply = ping.Send("8.8.8.8", 1000); // Ping no servidor DNS público do Google
                    return reply.Status == IPStatus.Success;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}