using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HS_Server_Region_Changer.Core;

namespace HS_Server_Region_Changer.Core
{
    public class CheckRunningGame
    {
        private Main main_f;

        public CheckRunningGame(Main main_F)
        {
            main_f = main_F;
            check_running();
        }
        public void check_running()
        {
               System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcessesByName("Hyperscape");

            if (ps.Length != 0)
            {
                if (main_f.toolStripComboBox1.Items.Count == 0)
                {
                    main_f.toolStripStatusLabel1.ForeColor = System.Drawing.Color.Red;
                    main_f.toolStripStatusLabel1.Text = "プロファイルを作成して下さい。";
                    main_f.button1.Enabled = false;
                    main_f.button2.Enabled = true;
                    main_f.button3.Enabled = false;
                    main_f.comboBox1.Enabled = false;
                    main_f.toolStripComboBox1.Enabled = false;
                }
                else
                {
                    if (main_f.refresh_checkRun)
                    {
                        main_f.toolStripStatusLabel1.ForeColor = System.Drawing.Color.Red;
                        main_f.toolStripStatusLabel1.Text = "注意：現在Hyper Scapeが起動中です。変更する際は必ずゲームを終了して下さい。";
                    }
                    main_f.button1.Enabled = false;
                    main_f.button2.Enabled = true;
                    main_f.button3.Enabled = false;
                    main_f.comboBox1.Enabled = true;
                    main_f.toolStripComboBox1.Enabled = true;
                }
            }
            else
            {
                if (main_f.toolStripComboBox1.Items.Count == 0)
                {
                    main_f.toolStripStatusLabel1.ForeColor = System.Drawing.Color.Red;
                    main_f.toolStripStatusLabel1.Text = "プロファイルを作成して下さい。";
                    main_f.button1.Enabled = false;
                    main_f.button2.Enabled = false;
                    main_f.button3.Enabled = false;
                    main_f.comboBox1.Enabled = false;
                    main_f.toolStripComboBox1.Enabled = false;
                }
                else
                {
                    if (main_f.refresh_checkRun)
                    {
                        main_f.toolStripStatusLabel1.ForeColor = System.Drawing.Color.Red;
                        main_f.toolStripStatusLabel1.Text = "現在Hyper Scapeは起動していません。";
                    }
                    main_f.button1.Enabled = true;
                    main_f.button2.Enabled = false;
                    main_f.button3.Enabled = true;
                    main_f.comboBox1.Enabled = true;
                    main_f.toolStripComboBox1.Enabled = true;
                }
            }
        }
    }
}
