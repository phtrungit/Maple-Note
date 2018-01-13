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
    public partial class AddNoteForm : MetroFramework.Forms.MetroForm
    {
        private String _tag=null;
        private String _content=null;
        public SendNote Sender;
        public AddNoteForm()
        {
            InitializeComponent();
        }
        public AddNoteForm(SendNote sender)
        {
            InitializeComponent();
            this.Sender = sender;
        }
        public String getTag()
        {
            return _tag;
        }
        public String getContent()
        {
            return _content;
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void AddNoteForm_Load(object sender, EventArgs e)
        {
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Sender(txtTag.Text, txtContent.Text);
            this.Close();
        }

        private void txtTag_Click(object sender, EventArgs e)
        {

        }
    }
}
