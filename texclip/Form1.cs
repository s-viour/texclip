using System.IO;
using System.Diagnostics;


namespace texclip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void LoadResultPicture()
        {
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
                pictureBox1.Image = null;
            }
            using (var bmpTmp = new Bitmap(Path.GetTempPath() + "texsnip\\result.png"))
            {
                pictureBox1.Image = new Bitmap(bmpTmp);
            }
            
            pictureBox1.Update();
            Clipboard.SetImage(pictureBox1.Image);
        }

        private void SubmitCompile(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Enter)
            {
                return;
            }
            Process texsnip = new();
            texsnip.StartInfo.FileName = "texsnip.exe";
            texsnip.StartInfo.RedirectStandardInput = true;
            texsnip.StartInfo.CreateNoWindow = true;
            try
            {
                texsnip.Start();
            }
            catch (System.ComponentModel.Win32Exception)
            {
                MessageBox.Show("could not find texsnip.exe in the path!");
                throw;
            }
            StreamWriter inputWriter = texsnip.StandardInput;
            inputWriter.Write(textBox1.Text);
            inputWriter.Close();

            texsnip.WaitForExit();
            LoadResultPicture();
        }
    }
}