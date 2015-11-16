using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Spiffdog.SqlLocalDbManager
{
    public partial class Form1 : Form
    {
        FileDialog _dialog;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            DoProcess("create");
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            DoProcess("start");
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            DoProcess("stop");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DoProcess("delete");
        }

        private void btnGetInfo_Click(object sender, EventArgs e)
        {
            DoProcess("info");
        }

        private void DoProcess(string activity)
        {
            try
            {
                if (string.IsNullOrEmpty(Global.SqlLocalDbPath))
                {
                    _dialog = new OpenFileDialog();
                    _dialog.Title = "Locate SqlLocalDB.exe";
                    _dialog.FileName = "SqlLocalDB.exe";
                    _dialog.Filter = "Executable files (*.exe)|*.exe|All files (*.*)|*.*";
                    _dialog.InitialDirectory = Global.SqlLocalDbDefaultPath;
                    _dialog.FilterIndex = 1;
                    if (_dialog.ShowDialog() == DialogResult.OK)
                    {
                        Global.SqlLocalDbPath = "\"" + _dialog.FileName + "\"";
                    }
                }

                var info = new ProcessStartInfo(Global.SqlLocalDbPath, " " + activity + " " + txtInstanceName.Text);
                var p = new Process();
                p.StartInfo = info;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.Start();
                p.WaitForExit();
                var reader = p.StandardOutput;
                var output = reader.ReadToEnd();
                reader.Close();

                if (output.Length == 0)
                {
                    reader = p.StandardError;
                    var error = reader.ReadToEnd();
                    reader.Close();
                    txtResult.Text = error;
                } else {
                    txtResult.Text = output;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}