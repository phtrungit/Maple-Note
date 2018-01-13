using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuickNote
{
   
    public partial class DetailNote : MetroFramework.Forms.MetroForm
    {
        public delegate void SendDetailNote(string tag, string content);
        public SendDetailNote Sender_DetailNote;
        public DetailNote()
        {
            InitializeComponent();
            Sender_DetailNote = new SendDetailNote(GetMessage);
        }

        private void GetMessage(string tag, string content)
        {
            txtTag.Text = tag;
            txtContent.Text = content;
        }

        private void DetailNote_Load(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtContent_Click(object sender, EventArgs e)
        {

        }

        private void txtTag_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
