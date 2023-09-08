using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.CodeDom.Compiler;
using S7.Net;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Security.Cryptography.X509Certificates;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace kiemtrakytu
{
    public partial class Form1 : Form
    {
        public string stri_cmd, str, x, y, z;
        public bool failcombine;
        int dem = 0;
        int cmd_count = 0;
        public string tmp;
        #region S7 Connect
        Timer timer = new Timer();
        Plc plc_s7_1200;
        CpuType CPU_Type = CpuType.S71200;
        string Ip = "192.168.0.1";
        short Rack = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            plc_s7_1200 = new Plc(CPU_Type, Ip, Rack, Slot);
            plc_s7_1200.Open();

            if (plc_s7_1200.IsConnected)
            {
                timer.Interval = Time_Update;
                timer.Start();
                timer.Tick += Timer_Tick;
            }
        }

        short Slot = 1;
        short Time_Update = 100;
        #endregion



        #region Read & write to S7
        public class DB1
        {
            public double x { get; set; }
            public double y { get; set; }
            public double z { get; set; }
            public double d { get; set; }
        }

        

        #region Read S7
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (plc_s7_1200.IsConnected)
                progressBar1.Value = 100;
            else
                progressBar1.Value = 0;

            DB1 db1 = new DB1();
            plc_s7_1200.ReadClass(db1, 1);

            TB_Read_x.Text = Convert.ToSingle(db1.x).ToString();
            TB_Read_y.Text = Convert.ToSingle(db1.y).ToString();
            TB_Read_z.Text = Convert.ToSingle(db1.z).ToString();
            //TB_Read_d.Text = Convert.ToSingle(db1.d).ToString();
            //Console.WriteLine("done");
        }
        #endregion

        #region write S7
        private void button1_Click_1(object sender, EventArgs e)
        {

            if (dem == 0)
            {
                stri_cmd = cmd_input.Text;
                int temp = 0;
                int temp1 = 0;
                while (temp != -1)
                {
                    temp = cmd_input.Text.IndexOf(";", temp1 + 1);
                    cmd_count += 1;
                    temp1 = temp;

                }
                //System.Console.WriteLine(cmd_count-1);
            }

            string stri_temp = stri_cmd;
            string[] cmd_ar = stri_temp.Split(';');
            string stri_new = cmd_ar[dem];
            //System.Console.WriteLine(stri_new);
            cmd_run.Text = "";
            if (check(stri_new))
            {
                for (int i = dem; i < cmd_ar.Length; i++) cmd_run.Text = cmd_run.Text + cmd_ar[i] + System.Environment.NewLine;
                stri_new = stri_new.Trim(';');
                string[] str_arr = stri_new.Split(' ');
                for (int i = 0; i < str_arr.Length; i++)
                {

                    str_arr[i] = str_arr[i].Trim();
                    Regex trimmer = new Regex(@"\s\s+");
                    str_arr[i] = trimmer.Replace(str_arr[i], " ");

                    if (str_arr[i].Contains('x')) tmp = str_arr[i].Substring(str_arr[i].IndexOf('x'), 1);
                    if (str_arr[i].Contains('y')) tmp = str_arr[i].Substring(str_arr[i].IndexOf('y'), 1);
                    if (str_arr[i].Contains('z')) tmp = str_arr[i].Substring(str_arr[i].IndexOf('z'), 1);

                    switch (tmp)
                    {
                        case "x":
                            x = str_arr[i].Trim('x');
                            break;
                        case "y":
                            y = str_arr[i].Trim('y');
                            break;
                        case "z":
                            z = str_arr[i].Trim('z');
                            break;
                    }
                    write_PLC();
                    System.Console.WriteLine(str_arr[i]);
                }
                dem++;
                x = "";
                y = "";
                z = "";
                //System.Console.WriteLine(dem);
                if (dem == cmd_count - 1)
                {
                    MessageBox.Show("Complete command");
                    button1.Enabled = false;
                }
                else
                {
                    //stri_cmd = stri_cmd.Remove(0,pos_cmd+1);
                    //System.Console.WriteLine(stri_cmd);
                    //System.Console.WriteLine(stri_cmd.Length);

                }

            }

            //System.Console.WriteLine(stri_new);

        }
        #endregion

        #endregion

        #region Kiểm tra ky tự
        /*private void build_Click(object sender, EventArgs e)
        {
            int dem = 1;
            
            chuoi = cmd_input.Text;
            chuoi = chuoi.ToLower();
            chuoi = chuoi.Trim();
            //TH1: Kiểm tra kết thúc bằng dầu ";"
            if (chuoi.EndsWith(";"))
            {
                // loại trừ ký tự trống
                Regex trimmer = new Regex(@"\s\s+");
                chuoi = trimmer.Replace(chuoi, " ");

                int solenh = 0;
                int temp = 0;
                int temp1 = 0;
                while (temp != -1)
                {
                    temp = chuoi.IndexOf(";", temp1 + 1);
                    solenh += 1;
                    temp1 = temp;
                }
                solenh -= 1;
                //System.Console.WriteLine(solenh);

                string[] lenh = chuoi.Split(';');

                for (int i = 0; i < solenh; i++)
                {
                    string s = lenh[i];
                    s = s.Trim();
                    string[] lenhh = s.Split();
                    //System.Console.Write(s);

                    //stri_cmd = cmd_run.Text + s + System.Environment.NewLine; 

                    //System.Console.WriteLine(lenhh[i].Length);

                    for (int j = 0; j < lenhh.Length; j++)
                    {
                        //TH2: Kiểm tra ký tự có phù hợp không
                        string[] chuoikt = { "q", "w", "e", "r", "t", "u", "i", "o", "p", "a", "f", "q", "g", "h", "j", "k", "l", "c", "v", "b", "n", ":", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "=", "[", "]", "{", "}", "<", ">", "?" };
                        for (int k = 0; k < chuoikt.Length; k++)
                        {
                            if (lenhh[j].Contains(chuoikt[k]) == true)
                            {
                                failcombine2 = true;
                                MessageBox.Show("Err chua ky tu khong hop le");
                            }
                        }

                        //System.Console.WriteLine(lenhh[j]);
                        string[] chuoiso = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

                        //TH3: Kiểm tra lệnh có chứa toạ độ số không
                        if ((lenhh[j].Contains(chuoiso[0]) || lenhh[j].Contains(chuoiso[1]) || lenhh[j].Contains(chuoiso[2]) || lenhh[j].Contains(chuoiso[3]) || lenhh[j].Contains(chuoiso[4]) || lenhh[j].Contains(chuoiso[5]) || lenhh[j].Contains(chuoiso[6]) || lenhh[j].Contains(chuoiso[7]) || lenhh[j].Contains(chuoiso[8]) || lenhh[j].Contains(chuoiso[9])) == true)
                        {
                            //System.Console.WriteLine(lenhh[j]); 
                        }
                        else
                        {
                            failcombine3 = true;
                            MessageBox.Show("Err khong chua so");
                        }

                        //TH4: Kiểm tra toạ độ có bắt đầu bằng ký tự hợp lệ không
                        char[] char1 = lenhh[j].ToCharArray();
                        //System.Console.WriteLine(char1[0]);  
                        if (char1[0] == 'x' || char1[0] == 'y' || char1[0] == 'z' || char1[0] == 's' || char1[0] == 'd' || char1[0] == 'm')
                        {

                        }
                        else
                        {
                            failcombine4 = true;
                            MessageBox.Show("Err bat dau khong hop le");
                        }
                    }
                    //TH5: Lặp kí tự
                    if ((s.IndexOf("x") != s.LastIndexOf("x")) || (s.IndexOf("y") != s.LastIndexOf("y")) || (s.IndexOf("z") != s.LastIndexOf("z")) || (s.IndexOf("m") != s.LastIndexOf("m")) || (s.IndexOf("s") != s.LastIndexOf("s")) || (s.IndexOf("d") != s.LastIndexOf("d")))
                    {
                        failcombine5 = true;
                        MessageBox.Show("Err lap ky tu");
                    }
                }
            }
            else
            {
                failcombine1 = true;
                MessageBox.Show("Loi ;");
            }

            if ((failcombine1 || failcombine2 || failcombine3 || failcombine4 || failcombine5 || failcombine6) == true)
            {
                //MessageBox.Show("error combine");
                failcombine1 = false;
                failcombine2 = false;
                failcombine3 = false;
                failcombine4 = false;
                failcombine5 = false;
                failcombine6 = false;
            }
            else
            {
                MessageBox.Show("finish");
            }
        }*/
        #endregion

        public bool check(string cmd)
        {

            string chuoi;
            bool fail1, fail2, fail3, fail4, fail5, fail6;

            fail1 = false; fail2 = false; fail3 = false; fail4 = false; fail5 = false; fail6 = false;

            chuoi = cmd.ToLower();
            chuoi = chuoi.Trim();
            //TH1: Kiểm tra kết thúc bằng dầu ";"
            //if (chuoi.EndsWith(";"))
            //{
            // loại trừ ký tự trống
            Regex trimmer = new Regex(@"\s\s+");
            chuoi = trimmer.Replace(chuoi, " ");

            int solenh = 0;
            int temp = 0;
            int temp1 = 0;
            while (temp != -1)
            {
                temp = chuoi.IndexOf(";", temp1 + 1);
                solenh += 1;
                temp1 = temp;
            }
            solenh -= 1;
            //System.Console.WriteLine(solenh);

            string[] lenh = chuoi.Split(';');

            for (int i = 0; i < solenh; i++)
            {
                string s = lenh[i];
                s = s.Trim();
                string[] lenhh = s.Split();
                //System.Console.Write(s);

                //stri_cmd = cmd_run.Text + s + System.Environment.NewLine; 

                //System.Console.WriteLine(lenhh[i].Length);

                for (int j = 0; j < lenhh.Length; j++)
                {
                    //TH2: Kiểm tra ký tự có phù hợp không
                    string[] chuoikt = { "q", "w", "e", "r", "t", "u", "i", "o", "p", "a", "f", "q", "g", "h", "j", "k", "l", "c", "v", "b", "n", ":", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "=", "[", "]", "{", "}", "<", ">", "?" };
                    for (int k = 0; k < chuoikt.Length; k++)
                    {
                        if (lenhh[j].Contains(chuoikt[k]) == true)
                        {
                            fail2 = true;
                            MessageBox.Show("Err chua ky tu khong hop le");
                            failcombine = true;
                        }
                    }

                    //System.Console.WriteLine(lenhh[j]);
                    string[] chuoiso = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

                    //TH3: Kiểm tra lệnh có chứa toạ độ số không
                    if ((lenhh[j].Contains(chuoiso[0]) || lenhh[j].Contains(chuoiso[1]) || lenhh[j].Contains(chuoiso[2]) || lenhh[j].Contains(chuoiso[3]) || lenhh[j].Contains(chuoiso[4]) || lenhh[j].Contains(chuoiso[5]) || lenhh[j].Contains(chuoiso[6]) || lenhh[j].Contains(chuoiso[7]) || lenhh[j].Contains(chuoiso[8]) || lenhh[j].Contains(chuoiso[9])) == true)
                    {
                        //System.Console.WriteLine(lenhh[j]); 
                    }
                    else
                    {
                        fail3 = true;
                        MessageBox.Show("Err khong chua so");
                        failcombine = true;                    }

                    //TH4: Kiểm tra toạ độ có bắt đầu bằng ký tự hợp lệ không
                    char[] char1 = lenhh[j].ToCharArray();
                    //System.Console.WriteLine(char1[0]);  
                    if (char1[0] == 'x' || char1[0] == 'y' || char1[0] == 'z' || char1[0] == 's' || char1[0] == 'd' || char1[0] == 'm')
                    {

                    }
                    else
                    {
                        fail4 = true;
                        MessageBox.Show("Err bat dau khong hop le");
                        failcombine = true;
                    }
                }
                //TH5: Lặp kí tự
                if ((s.IndexOf("x") != s.LastIndexOf("x")) || (s.IndexOf("y") != s.LastIndexOf("y")) || (s.IndexOf("z") != s.LastIndexOf("z")) || (s.IndexOf("m") != s.LastIndexOf("m")) || (s.IndexOf("s") != s.LastIndexOf("s")) || (s.IndexOf("d") != s.LastIndexOf("d")))
                {
                    fail5 = true;
                    MessageBox.Show("Err lap ky tu");
                    failcombine = true;
                }
            }
            /*}
            else
            {
                fail1 = true;
                MessageBox.Show("Loi ;");
                failcombine = true;
            }
            */
            if ((fail1 || fail2 || fail3 || fail4 || fail5 || fail6) == true)
            {
                //MessageBox.Show("error combine");
                fail1 = false;
                fail2 = false;
                fail3 = false;
                fail4 = false;
                fail5 = false;
                fail6 = false;
                return false;
            }
            else
            {
                MessageBox.Show("finish");
                return true;
            }
        }

        public void write_PLC()
        {
            DB1 db1 = new DB1();
            TB_Write_x.Text = x;
            TB_Write_y.Text = y;
            TB_Write_z.Text = z;
            if (TB_Write_x.Text != "")
                db1.x = Convert.ToSingle(TB_Write_x.Text);

            if (TB_Write_y.Text != "")
                db1.y = Convert.ToSingle(TB_Write_y.Text);

            if (TB_Write_z.Text != "")
                db1.z = Convert.ToSingle(TB_Write_z.Text);

            plc_s7_1200.WriteClass(db1, 1);

        }
    }


}

