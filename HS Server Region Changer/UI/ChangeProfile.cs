using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Net;

namespace HS_Server_Region_Changer
{
    public partial class ChangeProfile : Form
    {
        [DllImport("KERNEL32.DLL")]
        public static extern uint GetPrivateProfileString(
      string lpAppName,
      string lpKeyName,
      string lpDefault,
      StringBuilder lpReturnedString,
      uint nSize,
      string lpFileName);

        public ChangeProfile()
        {
            InitializeComponent();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            comboBox1.Items.Add("Uplay");

            textBox1.Text = Properties.Settings.Default.profile_name[Properties.Settings.Default.combobox1_selected_index];
            textBox2.Text = Properties.Settings.Default.profile_gamesettings[Properties.Settings.Default.combobox1_selected_index];
            textBox3.Text = Properties.Settings.Default.profile_exe[Properties.Settings.Default.combobox1_selected_index];

            for (int i = 0; i <= 1; i++)
            {
                if (comboBox1.Items[i].ToString() == Properties.Settings.Default.profile_platform[Properties.Settings.Default.combobox1_selected_index])
                {
                    comboBox1.SelectedIndex = i;
                    break;
                }
            }

        }
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
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && comboBox1.Text != "")
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
                        Properties.Settings.Default.profile_name[Properties.Settings.Default.combobox1_selected_index] = textBox1.Text;
                        Properties.Settings.Default.profile_gamesettings[Properties.Settings.Default.combobox1_selected_index] = textBox2.Text;
                        Properties.Settings.Default.profile_exe[Properties.Settings.Default.combobox1_selected_index] = textBox3.Text;
                        Properties.Settings.Default.profile_platform[Properties.Settings.Default.combobox1_selected_index] = comboBox1.Text;
                        Properties.Settings.Default.Save();
                        MessageBox.Show("変更しました。");
                        this.Close();
                    }
            }
            else
            {
                MessageBox.Show("未入力の項目があります。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}
