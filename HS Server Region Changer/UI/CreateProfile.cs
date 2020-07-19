using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace HS_Server_Region_Changer
{
    public partial class CreateProfile : Form
    {
        [DllImport("KERNEL32.DLL")]
        public static extern uint GetPrivateProfileString(
        string lpAppName,
        string lpKeyName,
        string lpDefault,
        StringBuilder lpReturnedString,
        uint nSize,
        string lpFileName);
        public CreateProfile()
        {
            InitializeComponent();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            platformList_ini();
        }

        private void platformList_ini()
        {
            comboBox1.Items.Add("Uplay");
            comboBox1.SelectedIndex = 0;
        }//サーバー一覧

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                var directory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\My Games\\Hyperscape";
                string[] directoryCount = Directory.GetDirectories(directory, "*", SearchOption.TopDirectoryOnly);

                if (directoryCount.Length == 1)
                {
                    openFileDialog1.InitialDirectory = directoryCount[0];
                }
                else
                {
                    openFileDialog1.InitialDirectory = directory;
                }

                openFileDialog1.FileName = "GameSettings";
                openFileDialog1.Filter = "INI ファイル (.ini)|*.ini";

                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    textBox2.Text = openFileDialog1.FileName;
                }
            }
            else
            {
                string str = textBox2.Text;
                str = str.Remove(str.IndexOf("Hyperscape") + "Hyperscape".Length);

                openFileDialog1.InitialDirectory = str;
                openFileDialog1.FileName = "GameSettings";
                openFileDialog1.Filter = "INI ファイル (.ini)|*.ini";

                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    textBox2.Text = openFileDialog1.FileName;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool check_same_name = false;

            if (Properties.Settings.Default.profile_name.Count != 0)
            {
                for (int i = 0; i <= Properties.Settings.Default.profile_name.Count - 1; i++)
                {
                    if (textBox1.Text == Properties.Settings.Default.profile_name[i])
                    {
                        check_same_name = true;
                    }
                }
            }

            if (Properties.Settings.Default.profile_name.Count <= 10)
            {
                if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && comboBox1.Text != "")
                {
                    if (check_same_name == false)
                    {
                        StringBuilder DataCenterHint = new StringBuilder(1024);
                        GetPrivateProfileString(
                            "ONLINE",
                            "DataCenterHint",
                            "0",
                            DataCenterHint,
                            Convert.ToUInt32(DataCenterHint.Capacity),
                            textBox2.Text);

                        if (DataCenterHint.ToString() == "0")
                        {
                            MessageBox.Show("サーバーリージョンを読み込めません。GameSettings.iniの場所を確認して下さい。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {                         
                            Properties.Settings.Default.profile_name.Add(textBox1.Text);
                            Properties.Settings.Default.profile_gamesettings.Add(textBox2.Text);
                            Properties.Settings.Default.profile_exe.Add(textBox3.Text);
                            Properties.Settings.Default.profile_platform.Add(comboBox1.Text);
                            Properties.Settings.Default.Save();

                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("同じ名前のプロファイルが、既に存在します。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("未入力の項目があります。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("保存可能なプロファイルは10個までです。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var directory = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            openFileDialog1.InitialDirectory = directory;

            openFileDialog1.FileName = "Hyperscape";
            openFileDialog1.Filter = "exe ファイル (.exe)|*.exe";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox3.Text = openFileDialog1.FileName;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                if (comboBox1.Text == "Uplay")
                {
                    if (System.IO.File.Exists(@"C:\Program Files (x86)\Ubisoft\Ubisoft Game Launcher\games\Hyper Scape\Hyperscape.exe"))
                    {
                        textBox3.Text = @"C:\Program Files (x86)\Ubisoft\Ubisoft Game Launcher\games\Hyper Scape\Hyperscape.exe";
                    }
                    if (System.IO.File.Exists(@"D:\Program Files (x86)\Ubisoft\Ubisoft Game Launcher\games\Hyper Scape\Hyperscape.exe"))
                    {
                        textBox3.Text = @"D:\Program Files (x86)\Ubisoft\Ubisoft Game Launcher\games\Hyper Scape\Hyperscape.exe";
                    }
                    if (System.IO.File.Exists(@"E:\Program Files (x86)\Ubisoft\Ubisoft Game Launcher\games\Hyper Scape\Hyperscape.exe"))
                    {
                        textBox3.Text = @"E:\Program Files (x86)\Ubisoft\Ubisoft Game Launcher\games\Hyper Scape\Hyperscape.exe";
                    }
                    if (System.IO.File.Exists(@"F:\Program Files (x86)\Ubisoft\Ubisoft Game Launcher\games\Hyper Scape\Hyperscape.exe"))
                    {
                        textBox3.Text = @"F:\Program Files (x86)\Ubisoft\Ubisoft Game Launcher\games\Hyper Scape\Hyperscape.exe";
                    }
                }
            }

        }
    }
}
