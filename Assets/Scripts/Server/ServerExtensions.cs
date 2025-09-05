using System.Linq;
using System.Net;

public static class ServerExtensions
{
    public static string GetLocalIpv4Address()
    {
        return Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(f =>
                f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            .ToString();
    }
}
