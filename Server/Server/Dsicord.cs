using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;


namespace Server
{
    class Dsicord : WebSocketBehavior
    {
        List<string> users = new List<string>() { "Celovek1:123", "Celovek2:521" };

        protected override void OnOpen()
        {
            if (!Context.QueryString.Contains("Name"))
            {
                Send("Error|You must have a name to connect");
                Sessions.CloseSession(ID);
                return;
            }
            var name = Context.QueryString["name"];
            if (string.IsNullOrEmpty(name) || users.Contains(name))
            {
                Send("Error|Change name");
                Sessions.CloseSession(ID);
                return;
            }

            Send($"USERS|{UsersToString()}");
            users.Add(name+":"+ID);

            Sessions.Broadcast($"USERS|{name}:{ID}");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            Sessions.Broadcast($"MESSAGE|{e.Data}");
        }

        private string UsersToString()
        {
            var text = "";
            foreach (var user in users)
            {
                text += user + " ";
            }
            return text;

        }

        protected override void OnClose(CloseEventArgs e)
        {
            Sessions.Broadcast($"LEFT|{ID}");
            Console.WriteLine($"{DateTime.Now.ToString("t")} server stopped: {e.Reason}");
        }
    }
}
