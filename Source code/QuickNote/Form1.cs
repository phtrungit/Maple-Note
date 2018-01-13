using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuickNote
{
    public delegate void SendNote(string tag, string content);
    public partial class Form1 : MetroFramework.Forms.MetroForm    {

        String file_path = "data.dat";
        Y2KeyboardHook _keyboardHook;
        bool addNoteFrmActivated = false;

        List<Tag_Note> tag_list=new List<Tag_Note>();
        List<Note> note_list=new List<Note>();
        bool Form1IsRunning=true;
        public Form1()
        {
            bool ctrldown = false;
            InitializeComponent();

            _keyboardHook = new Y2KeyboardHook();
            _keyboardHook.Install();


            _keyboardHook.KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.LControlKey)
                {
                    ctrldown = true;
                }

                if (e.KeyCode == Keys.Z && ctrldown)
                {
                    if(!addNoteFrmActivated)
                    {
                        AddNoteEvent();
                    }
                }
            };
            _keyboardHook.KeyUp += (sender, e) =>
            {

                if (e.KeyCode == Keys.LControlKey)
                {
                    ctrldown = false;
                }
            };
        }
       
        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.Visible = true;
            ReadData();
           
            
            Refresh_lbTag();
            lbtag.SelectedIndex = 0;
            
        }

        private void Refresh_lbTag()
        {
            int last_index = lbtag.SelectedIndex;
            lbtag.Items.Clear();
            foreach (var item in tag_list)
            {
                lbtag.Items.Add("#"+item.tag_name+" {"+item.num_of_note+"}");
            }
            lbtag.SelectedIndex = last_index;
        }

        private void viewNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            WindowState = FormWindowState.Normal;
        }

       

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveData();
            Form1IsRunning = false;

            this.Close();
        }
        
       

        private void lbtag_SelectedIndexChanged(object sender, EventArgs e)
        {

           
            String selected_item = lbtag.SelectedItem.ToString();
          
            Load_LbNote(selected_item);

        }



        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddNoteEvent();
        }
        private void GetMessage(String tag,String content)
        {
            Note note = new Note();
            note.content = content;
            note.tag_name = new List<string>();
            bool flag = true;
            String[] mul_tag = tag.Split(',');
            foreach (var item in mul_tag)
            {
                item.Trim();
                for (int i = 0; i < tag_list.Count; i++)
                {
                    if(tag_list[i].tag_name==item)
                    {
                        flag = false;
                        Tag_Note tag_item = new Tag_Note();
                        tag_item.tag_name = tag_list[i].tag_name;
                        tag_item.num_of_note=tag_list[i].num_of_note+1;
                        tag_list[i] = tag_item;
                        break;
                    }
                }
                if(flag)
                {
                    Tag_Note tag_item = new Tag_Note();
                    tag_item.tag_name = tag;
                    tag_item.num_of_note = 1;
                    tag_list.Add(tag_item);
                }
                note.tag_name.Add(item);
            }

            Tag_Note all_tag = new Tag_Note();
            all_tag.tag_name = "All Tag";
            all_tag.num_of_note = tag_list[0].num_of_note+1;
            tag_list[0]= all_tag;

            note_list.Add(note);
            Refresh_lbTag();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(Form1IsRunning)
            {
                e.Cancel = true;
                this.Visible = false;
                notifyIcon1.ShowBalloonTip(3000);
            }
        }
        private void SaveData()
        {
            FileStream fs = new FileStream(file_path, FileMode.Create);//Tạo file mới tên là test.txt            
            StreamWriter sWriter = new StreamWriter(fs, Encoding.UTF8);//fs là 1 FileStream
            foreach (var item in tag_list)
            {
                String line = "*Tag:" + "{" + item.tag_name + "}{" + item.num_of_note + "}";
                sWriter.WriteLine(line);
            }
            foreach (var item in note_list)
            {
                string tag_mul="";
                foreach (var tag in item.tag_name)
                {
                    tag_mul += tag + ",";
                }
                tag_mul=tag_mul.Substring(0, tag_mul.Length - 1);
                String line = "*Note:" + "{" + tag_mul + "}" + "{" + item.content + "}";
                sWriter.WriteLine(line);
            }
            sWriter.Flush();
            fs.Close();
        }
        private void ReadData()
        {
            string[] lines = File.ReadAllLines(file_path);
            foreach (string item in lines)
            {
                if(item!="")
                {
                    if (item.StartsWith("*Tag"))
                    {
                        Tag_Note tag_note = new Tag_Note();
                        string tag_line = item;
                        tag_line = tag_line.Substring(6, tag_line.Length - 6);
                        tag_note.tag_name = tag_line.Substring(0, tag_line.IndexOf('}'));
                        tag_line = tag_line.Remove(0, tag_line.IndexOf('}') + 2);
                        tag_note.num_of_note = Int32.Parse(tag_line.Substring(0, tag_line.IndexOf('}')));
                        tag_list.Add(tag_note);
                    }
                    else
                    {
                        Note note = new Note();
                        note.tag_name = new List<string>();
                        String multag;
                        String note_line = item;
                        note_line = note_line.Substring(7, note_line.Length - 7);
                        multag = note_line.Substring(0, note_line.IndexOf('}'));
                        String[] tag_array = multag.Split(',');
                        foreach (String tag in tag_array)
                        {
                            note.tag_name.Add(tag);
                        }
                        note_line = note_line.Remove(0, note_line.IndexOf('}') + 2);
                        note.content = note_line.Substring(0, note_line.IndexOf('}'));
                        note_list.Add(note);
                    }
                }
                
            }
        }
        private void AddNoteEvent()
        {
            using (AddNoteForm addForm = new AddNoteForm(GetMessage))
            {
                this.WindowState = FormWindowState.Minimized;
                addNoteFrmActivated = true;
                addForm.ShowDialog();
            }
            this.WindowState = FormWindowState.Normal;
            addNoteFrmActivated = false;
        }

        private void addNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNoteEvent();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            
            String selected_item = lbnote.SelectedItem.ToString();
           
            String selected_tag = lbtag.SelectedItem.ToString();
            String original_tag = selected_tag;
            selected_tag = selected_tag.Substring(1, selected_tag.LastIndexOf(' ') - 1);
            foreach (var item in note_list)
            {
                string full_content = "";
                if (item.content.Length > 50 && selected_item.Length>50)
                {
                    full_content = selected_item.Substring(0, 50);
                    full_content += item.content.Substring(50, item.content.Length - 50);
                }
                else
                {
                    full_content = selected_item;
                }

                if (item.content==full_content && item.getHashCodeContent() == full_content.GetHashCode())
                {
                    
                   
                        foreach (var tag in item.tag_name)
                        {
                            for (int i = 0; i < tag_list.Count; i++)
                            {
                                if (tag == tag_list[i].tag_name)
                                {
                                    Tag_Note tag_temp = new Tag_Note();
                                    tag_temp.tag_name = tag_list[i].tag_name;
                                    tag_temp.num_of_note = tag_list[i].num_of_note - 1;
                                    tag_list[i] = tag_temp;

                                }
                            }
                        }
                    
                    note_list.Remove(item);
              
                    break;
                }
            }
            if (selected_tag == "All Tag")
            {
                Tag_Note all_tag = new Tag_Note();
                all_tag.tag_name = "All Tag";
                all_tag.num_of_note = tag_list[0].num_of_note - 1;
                tag_list[0] = all_tag;
            }

            Load_LbNote(original_tag);
            Refresh_lbTag();
        }
        private void Load_LbNote(String selected_item)
        {
            selected_item = selected_item.Substring(1, selected_item.LastIndexOf(' ') - 1);
            lbnote.Items.Clear();
            foreach (var item in note_list)
            {
                foreach (var tag in item.tag_name)
                {
                   
                    if (tag == selected_item || selected_item == "All Tag")
                    {
                        String sortContent;
                        if(item.content.Length>=50)
                        {
                            sortContent = item.content.Substring(0, 50)+"...";
                        }
                        else
                        {
                            sortContent = item.content;
                        }
                        lbnote.Items.Add(sortContent);
                    }
                    if (selected_item == "All Tag")
                        break;
                }
               

            }
        }

        private void lbnote_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDelete.Enabled = true;
        }

        private void lbnote_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           
           if(lbnote.IndexFromPoint(e.Location)!=-1)
            {
                lbnote.SelectedIndex = lbnote.IndexFromPoint(e.Location);
                String content  = lbnote.SelectedItem.ToString();
            
                String tag = "";
                foreach (var item in note_list)
                {
                    string full_content = "";
                    if(item.content.Length>50)
                    {
                        full_content = content.Substring(0, 50);
                        full_content += item.content.Substring(50, item.content.Length-50);
                    }
                    else
                    {
                        full_content=content;
                    }
                    if (item.content== full_content && item.getHashCodeContent()==full_content.GetHashCode())
                    {
                        foreach (var tag_mul in item.tag_name)
                        {
                            tag += "," + tag_mul;
                        }
                        content = full_content;
                        break;
                    }
                }
                tag = tag.Substring(1, tag.Length-1);
                DetailNote frm_detail_note = new DetailNote();
                frm_detail_note.Sender_DetailNote(tag,content);
                frm_detail_note.Show();
            }
                
        }

        private void viewStatisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Chart chart = new Chart();
            List<String>alltag=new List<String>();
            List<int> num=new List<int>() ;
         
            for (int item=1;item<tag_list.Count;item++)
            {

                alltag.Add( tag_list[item].tag_name);
                num.Add(tag_list[item].num_of_note);
                
            }
            chart.Sender_DataChart(alltag, num);
            chart.ShowDialog();
        }
    }
}
