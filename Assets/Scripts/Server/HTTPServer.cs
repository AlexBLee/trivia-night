using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class HTTPServer : MonoBehaviour
{
    private HttpListener _listener;
    private Thread _httpThread;

    private void Start()
    {
        _httpThread = new Thread(StartHttpServer);
        _httpThread.Start();
    }

    private void StartHttpServer()
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add($"http://{GetLocalIpv4Address()}:8081/");
        _listener.Start();
        Debug.Log("HTTP server started");

        Task listenTask = HandleConnection();
        listenTask.GetAwaiter().GetResult();

        _listener.Close();
    }

    private string GetLocalIpv4Address()
    {
        return Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(f =>
                f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            .ToString();
    }

    private async Task HandleConnection()
    {
        while (true)
        {
            var context = await _listener.GetContextAsync();
            var response = context.Response;

            string localPath = context.Request.Url.AbsolutePath.Trim('/');
            if (string.IsNullOrEmpty(localPath))
            {
                localPath = "index.html";
            }

            string filePath = Path.Combine(Application.streamingAssetsPath, "dist/" + localPath);

            if (File.Exists(filePath))
            {
                byte[] buffer = await File.ReadAllBytesAsync(filePath);
                response.ContentType = GetExtensionType(Path.GetExtension(filePath));
                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            }
            else
            {
                byte[] buffer = Encoding.UTF8.GetBytes("<h1>404 Not Found</h1>");
                response.StatusCode = 404;
                await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            }

            response.OutputStream.Close();
        }
    }

    string GetExtensionType(string ext)
    {
        switch (ext.ToLower())
        {
            case ".html": return "text/html";
            case ".css": return "text/css";
            case ".js": return "application/javascript";
            case ".png": return "image/png";
            case ".jpg":
            case ".jpeg": return "image/jpeg";
            default: return "application/octet-stream";
        }
    }
}
