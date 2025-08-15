using System;
using System.IO;
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
        _listener.Prefixes.Add("http://192.168.1.77:8081/");
        _listener.Start();
        Debug.Log("HTTP server started");

        Task listenTask = HandleConnection();
        listenTask.GetAwaiter().GetResult();

        _listener.Close();
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

            string filePath = Path.Combine(Application.streamingAssetsPath, localPath);

            if (File.Exists(filePath))
            {
                byte[] buffer = File.ReadAllBytes(filePath);
                response.ContentType = "text/html";
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
}
