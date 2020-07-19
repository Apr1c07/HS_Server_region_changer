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
using HS_Server_Region_Changer.Core;

namespace HS_Server_Region_Changer
{
    public partial class Main : Form
    {
        [DllImport("KERNEL32.DLL")]
        public static extern uint GetPrivateProfileString(
           string lpAppName,
           string lpKeyName,
           string lpDefault,
           StringBuilder lpReturnedString,
           uint nSize,
           string lpFileName);

        [DllImport("KERNEL32.DLL")]
        private static extern uint WritePrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpString,
            string lpFileName);

        private Timer t;
        public bool refresh_checkRun = true;
        public int pre_index;
        public int before_index;

        public Main()
        {
            InitializeComponent();
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            toolStripComboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            get_time();
            timer1.Interval = 1000;
            timer1.Enabled = true;

            if (Properties.Settings.Default.quit_apps == true)
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }

            if (Properties.Settings.Default.gamesettings_set_check == false)
            {
                Properties.Settings.Default.profile_name = new System.Collections.Specialized.StringCollection();
                Properties.Settings.Default.profile_gamesettings = new System.Collections.Specialized.StringCollection();
                Properties.Settings.Default.profile_exe = new System.Collections.Specialized.StringCollection();
                Properties.Settings.Default.profile_platform = new System.Collections.Specialized.StringCollection();
                ini_gamesetting_dr();
            }
            else
            {
                combobox1_refresh(1);
                //read_server();
            }
        }

        private void ini_gamesetting_dr()
        {
            DialogResult dr = MessageBox.Show("初期設定をします。プロファイルを作成してください。" + Environment.NewLine +
                  Environment.NewLine, "初期設定");

            CreateProfile createProfile_ini = new CreateProfile();
            createProfile_ini.FormClosed += this.createProfile_ini_FormClosed;
            createProfile_ini.Show();
            createProfile_ini.TopMost = true;
        }
        private void read_server()
        {
            if (System.IO.File.Exists(Properties.Settings.Default.profile_gamesettings[Properties.Settings.Default.combobox1_selected_index]))
            {
                StringBuilder DataCenterHint = new StringBuilder(1024);
                GetPrivateProfileString(
                    "ONLINE",
                    "DataCenterHint",
                    "0",
                    DataCenterHint,
                    Convert.ToUInt32(DataCenterHint.Capacity),
                    Properties.Settings.Default.profile_gamesettings[Properties.Settings.Default.combobox1_selected_index]);

                for (int i = 0; i <= comboBox1.Items.Count - 1; i++)
                {
                    if (((KeyValuePair<string, string>)comboBox1.Items[i]).Value.ToString() == DataCenterHint.ToString())
                    {
                        comboBox1.SelectedIndex = i;
                    }
                }
            }
            else
            {
                MessageBox.Show("指定されたGamesettings.iniファイルが存在しません。");
            }


        }//現在のサーバーをインデックスに設定
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process p = System.Diagnostics.Process.Start(Properties.Settings.Default.profile_exe[toolStripComboBox1.SelectedIndex]);
                if (checkBox1.Checked == true) this.Close();
            }
            catch
            {
                MessageBox.Show("ゲーム実行ファイルが見つかりませんでした。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcessesByName("Hyperscape");
            foreach (System.Diagnostics.Process p in ps) p.Kill();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            WritePrivateProfileString(
           "ONLINE",
           "DataCenterHint",
           comboBox1.SelectedValue.ToString(),
           Properties.Settings.Default.profile_gamesettings[Properties.Settings.Default.combobox1_selected_index]);

            StringBuilder DataCenterHint = new StringBuilder(1024);
            GetPrivateProfileString(
                "ONLINE",
                "DataCenterHint",
                "0",
                DataCenterHint,
                Convert.ToUInt32(DataCenterHint.Capacity),
                Properties.Settings.Default.profile_gamesettings[Properties.Settings.Default.combobox1_selected_index]);

            toolStripStatusLabel1.ForeColor = Color.Red;
            toolStripStatusLabel1.Text = "接続サーバーを「" + DataCenterHint.ToString() + "」に変更しました。";
            Properties.Settings.Default.username_selected = toolStripComboBox1.SelectedItem.ToString();
            Properties.Settings.Default.Save();

            refresh_checkRun = false;
            time_start();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            CheckRunningGame MyClassObj = new CheckRunningGame(this);
            MyClassObj.check_running();
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            About form2 = new About();
            form2.Show();
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            before_index = toolStripComboBox1.Items.Count;
            CreateProfile createProfile = new CreateProfile();
            createProfile.FormClosed += this.CreateProfile_FormClosed;
            createProfile.Show();
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pre_index = toolStripComboBox1.SelectedIndex;
            ChangeProfile changeProfile = new ChangeProfile();
            changeProfile.FormClosed += this.ChangeProfile_FormClosed;
            changeProfile.Show();
        }
        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripComboBox1.Items.Clear();
            toolStripComboBox1.Text = "";

            if (Properties.Settings.Default.profile_name.Count != 0)
            {
                Properties.Settings.Default.profile_name.RemoveAt(Properties.Settings.Default.combobox1_selected_index);
                Properties.Settings.Default.profile_gamesettings.RemoveAt(Properties.Settings.Default.combobox1_selected_index);
                Properties.Settings.Default.profile_exe.RemoveAt(Properties.Settings.Default.combobox1_selected_index);
                Properties.Settings.Default.profile_platform.RemoveAt(Properties.Settings.Default.combobox1_selected_index);
                Properties.Settings.Default.Save();

                combobox1_refresh(3);
            }
            else
            {
                MessageBox.Show("プロファイルが存在しません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void createProfile_ini_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Properties.Settings.Default.profile_name.Count != 0)
            {
                Properties.Settings.Default.gamesettings_set_check = true;
                Properties.Settings.Default.Save();

                StringBuilder DataCenterHint = new StringBuilder(1024);
                GetPrivateProfileString(
                    "ONLINE",
                    "DataCenterHint",
                    "0",
                    DataCenterHint,
                    Convert.ToUInt32(DataCenterHint.Capacity),
                    Properties.Settings.Default.profile_gamesettings[0]);

                toolStripComboBox1.Items.Add(Properties.Settings.Default.profile_name[0]);
                toolStripComboBox1.SelectedIndex = 0;
            }
            else
            {
                removeToolStripMenuItem.Enabled = false;
                editToolStripMenuItem.Enabled = false;
                button3.Enabled = false;
            }
        }
        private void CreateProfile_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (before_index < Properties.Settings.Default.profile_name.Count) pre_index = Properties.Settings.Default.profile_name.Count - 1;
            combobox1_refresh(0);
        }
        private void ChangeProfile_FormClosed(object sender, FormClosedEventArgs e)
        {
            combobox1_refresh(0);
        }
        private void get_time()
        {
            (var authGroup, var key, var value, var now) = GetTime.getTime();
            comboBox1.DataSource = authGroup;
            comboBox1.DisplayMember = key;
            comboBox1.ValueMember = value;

            label3.Text = now.ToString("MM/dd HH:mm");
        }
        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.combobox1_selected_index = toolStripComboBox1.SelectedIndex;
            Properties.Settings.Default.Save();
            read_server();
            toolStripStatusLabel1.ForeColor = Color.Red;
            toolStripStatusLabel1.Text = "プロファイルを「" + Properties.Settings.Default.profile_name[Properties.Settings.Default.combobox1_selected_index] + "」に設定しました。";
            refresh_checkRun = false;
            time_start();
            this.ActiveControl = null;//コントロールの選択状態"青"を解除
        }
        private void Main_Load(object sender, EventArgs e)
        {

        }
        private void combobox1_refresh(int option)//0:新規データをインデックスに　　1:前回選択したプロファイルをインデックスに 　　 3:プロファイル削除後
        {
            toolStripComboBox1.Items.Clear();
            toolStripComboBox1.Text = "";

            for (int i = 0; i <= Properties.Settings.Default.profile_name.Count - 1; i++)
            {
                toolStripComboBox1.Items.Add(Properties.Settings.Default.profile_name[i]);
            }

            switch (option)
            {
                case 0:
                    if (Properties.Settings.Default.profile_name.Count != 0)
                    {
                        removeToolStripMenuItem.Enabled = true;
                        editToolStripMenuItem.Enabled = true;
                        button3.Enabled = true;
                        //toolStripComboBox1.SelectedIndex = Properties.Settings.Default.profile_name.Count - 1;
                        toolStripComboBox1.SelectedIndex = pre_index;
                    }
                    else
                    {
                        removeToolStripMenuItem.Enabled = false;
                        editToolStripMenuItem.Enabled = false;
                        button3.Enabled = false;
                    }
                    break;

                case 1:
                    removeToolStripMenuItem.Enabled = true;
                    editToolStripMenuItem.Enabled = true;
                    button3.Enabled = true;
                    for (int i = 0; i <= Properties.Settings.Default.profile_name.Count - 1; i++)
                    {
                        if (Properties.Settings.Default.profile_name[i] == Properties.Settings.Default.username_selected)
                        {
                            toolStripComboBox1.SelectedIndex = i;
                        }
                        else
                        {
                            toolStripComboBox1.SelectedIndex = 0;
                        }
                    }
                    break;

                case 3:
                    if (Properties.Settings.Default.profile_name.Count != 0)
                    {
                        for (int i = 0; i <= Properties.Settings.Default.profile_name.Count - 1; i++)
                        {
                            if (Properties.Settings.Default.profile_name[i] == Properties.Settings.Default.username_selected)
                            {
                                toolStripComboBox1.SelectedIndex = i;
                            }
                            else
                            {
                                toolStripComboBox1.SelectedIndex = 0;
                            }
                        }
                    }
                    else
                    {
                        Properties.Settings.Default.gamesettings_set_check = false;
                        Properties.Settings.Default.Save();
                        comboBox1.SelectedIndex = 0;
                        removeToolStripMenuItem.Enabled = false;
                        editToolStripMenuItem.Enabled = false;
                    }
                    break;
            }
        }
        private void time_start()
        {
            t = new Timer();
            t.Tick += new EventHandler(MyEvent);
            t.Interval = 3000; // ミリ秒単位で指定
            t.Start();
        }
        private void MyEvent(object sender, EventArgs e)
        {
            t.Stop();
            refresh_checkRun = true;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.quit_apps = checkBox1.Checked;
            Properties.Settings.Default.Save();
        }
    }
}
