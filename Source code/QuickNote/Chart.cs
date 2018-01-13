using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
namespace QuickNote
{
    public partial class Chart : MetroFramework.Forms.MetroForm
    {
        public delegate void SendDataChart(List<String>tag, List<int> num_notes);
        public SendDataChart Sender_DataChart;
        public Chart()
        {
            InitializeComponent();
            Sender_DataChart = new SendDataChart(GetMessage);
        }

       

        private void Chart_Load(object sender, EventArgs e)
        {
            //GetMessage();

        }

        private void chartNote_Click(object sender, EventArgs e)
        {

        }
        private void GetMessage(List<String> tag, List<int> num_notes)
        {
            for (int i = 0; i < tag.Count; i++)
            {
                chartNote.Series["Tag"].Points.AddXY(tag[i]+" {"+num_notes[i].ToString()+"}", num_notes[i]);
            }
        }
    }
}
