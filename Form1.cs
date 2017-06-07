using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace MinecraftModUpdater
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnSelectFolder_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxPath.Text = folderBrowserDialog1.SelectedPath;
            }
            
            
        }

        //select zip
        private void BtnSelectZip_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Zip bestanden (*.zip)|*.zip";
            DialogResult result = openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            textBoxZip.Text = openFileDialog1.FileName;
        }


        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {

            if (File.Exists(textBoxZip.Text) 
                && Directory.Exists(textBoxPath.Text) 
                && Directory.Exists(Path.Combine(textBoxPath.Text, "mods")) 
                && Directory.Exists(Path.Combine(textBoxPath.Text, "config"))
                )
            {
                this.Enabled = false;
                Form2 win2 = new Form2(textBoxPath.Text, textBoxZip.Text)
                {
                    Owner = this
                };
                win2.Show();
                
            }
            else MessageBox.Show("De update zip of de map kon niet gevonden worden.", "Fout!");
        }
    }
}
