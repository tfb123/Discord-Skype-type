using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketSharp;

namespace Client
{
    public partial class Form1 : Form
    {
        WebSocket client;
        string userName = "Milan";
        Dictionary<string, Color> users = new Dictionary<string, Color>();

        delegate void AddUser(string name,Color color);

        delegate void AddEntry(string entry);



        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new WebSocket($"ws://localhost:7777/Dsicord?name={userName}");
            client.OnMessage += Client_OnMessage;
            client.Connect(); 
        }

        private void Client_OnMessage(object sender, MessageEventArgs e)
        {
            var code = e.Data.Split('|')[0];
            var data = e.Data.Split('|')[1];
            switch (code)
            {
                case "USERS":
                    GetAllUsers(data);
                    break;
                case "MESSAGE":
                    var add = new AddEntry(AddChatEntry);
                    panelChat.Invoke(add,new object[] { data });
                    break;
                case "LEFT":
                    panelUsers.Invoke(new MethodInvoker(()=> { RemoveUser(data); }));
                    MessageBox.Show(data);
                    break;
            }
        }

        private void RemoveUser(string data)
        {
            string id = data;
            for (int i=0;i<panelUsers.Controls.Count;i++)
            {
                if (panelUsers.Controls[i].Name == id)
                {
                    panelUsers.Controls.RemoveAt(i);
                }
            }
        }

        void AddChatEntry(string messageString)
        {
            var name = messageString.Split(':')[0];
            var message = messageString.Split(':')[1];

            var color = (users.ContainsKey(name)) ? users[name] : Color.White;

            var entry = new ChatEntry(name, message, color);
            panelChat.Controls.Add(entry);
        }

        private void GetAllUsers(string userString)
        {
            foreach (var user in userString.Split(' '))
            {
                if (string.IsNullOrEmpty(user)) continue;

                var color = GenerateColor();

                var add = new AddUser(AddUserToForm);
                panelUsers.Invoke(add, new object[] { user, color });
            }
        }

        private object GenerateColor()
        {
            Color color;
            var rnd = new Random();
            do
            {
                color = Color.FromArgb(rnd.Next(25, 255), rnd.Next(25, 255), rnd.Next(25, 255));

            } while (users.ContainsValue(color));
            return color;
        }

        private void AddUserToForm(string name, Color color)
        {
            var button = new IconButton()
            {
                Text = name.Split(':')[0],
                Name = name.Split(':')[1],
                ForeColor = color,
                TextAlign = ContentAlignment.MiddleLeft,

                IconChar = IconChar.UserCircle,
                IconColor = color,
                IconSize = 25,
                ImageAlign = ContentAlignment.MiddleLeft,

                TextImageRelation = TextImageRelation.ImageBeforeText,

                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Width = panelUsers.Width,
                Dock = DockStyle.Top,

                Height = 60
            };
            panelUsers.Controls.Add(button);
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            client.Close();
            Application.Exit();
        }

        private void txtChat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                client.Send($"{userName}:{txtChat.Text}");
                txtChat.Clear();
            }
        }

        private void panelChat_ControlAdded(object sender, ControlEventArgs e)
        {
            panelChat.ScrollControlIntoView(e.Control);
        }

        private void txtChat_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
