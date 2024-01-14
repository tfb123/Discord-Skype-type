using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;


namespace Server
{
    
    class Program
    {
        static WebSocketServer server;
        static void Main(string[] args)
        {
            Console.WriteLine($"{DateTime.Now.ToString("t")} Starting server...");

            server = new WebSocketServer(7777);
            server.AddWebSocketService<Dsicord>("/Dsicord");
            server.Start();

            Console.WriteLine($"{DateTime.Now.ToString("t")} Server started");

            Console.ReadKey();
        }
    }
}
