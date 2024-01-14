using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class ChatEntry : UserControl
    {
        public ChatEntry(string name,string message , Color color)
        {
            InitializeComponent();

            lblName.Text = name;
            lblName.ForeColor = color;
            lblMessage.Text = message;
        }



        private void ChatEntry_Load(object sender, EventArgs e)
        {

        }
    }
}
