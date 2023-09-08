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
using System.IO;

namespace kiemtrakytu
{
    public partial class Form1 : Form
    {
        public string stri_cmd, str, x, y, z, m, com;
        public bool failcombine, signFinishstep, CheckFinish, signFinish = true, reset;
        bool failcombine1, failcombine2, failcombine3, failcombine4, failcombine5, failcombine6;
        int dem = 0;
        public string tmp;
        private object openFileDialog;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
            reset = true;
        }

        #region S7 Connect
        Timer timer = new Timer();
        Plc plc_s7_1200;
        CpuType CPU_Type = CpuType.S71200;
        string Ip = "192.168.0.2";
        short Rack = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            plc_s7_1200 = new Plc(CPU_Type, Ip, Rack, Slot);
            plc_s7_1200.Open();

            if (plc_s7_1200.IsConnected)
            {
                timer.Interval = Time_Update;
                timer.Start();
                timer.Tick += Timer_Tick;

                byte[] Q01 = plc_s7_1200.ReadBytes(DataType.Output, 0, 0, 2);
                if (Q01[1].SelectBit(0))
                {
                    signFinishstep = true;
                }
                else
                {
                    signFinishstep = false;
                }
            }

            if ((signFinishstep == true) && (signFinish == false))  tranfer(); 

            if ((plc_s7_1200.IsConnected) && (reset))
            {
                plc_s7_1200.WriteBit(DataType.Output, 0, 1, 0, false);
                reset = false;
            }
        }

        short Slot = 1;
        short Time_Update = 100;

        public object Q10 { get; private set; }
        #endregion

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string filename = saveFileDialog1.FileName;
            File.WriteAllText(filename, cmd_input.Text);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                cmd_input.Text = File.ReadAllText(openFileDialog.FileName); ;

            }
        }

        private void btn_reload_Click(object sender, EventArgs e)
        {
            cmd_input.Text = "";
            cmd_run.Text = "";
            dem = 0;
            signFinish = false;

            plc_s7_1200.WriteBit(DataType.Output, 0, 1, 0, false);
        }

        #region Read & write to S7
        public class DB4
        {
            public double x { get; set; }
            public double y { get; set; }
            public double z { get; set; }
            public double m { get; set; }

        }
        
        #region Read S7
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (plc_s7_1200.IsConnected)
                progressBar1.Value = 100;
            else
                progressBar1.Value = 0;

            DB4 db1 = new DB4();
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
            signFinish = false;
            stri_cmd = cmd_input.Text;
            cmd_run.Text = "";

            string stri_temp = stri_cmd;
            string[] cmd_ar = stri_temp.Split(';');
            for (int i = 0; i < cmd_ar.Length; i++)
            {
                Regex trimmer = new Regex(@"\s\s+");
                cmd_ar[i] = trimmer.Replace(cmd_ar[i], " ");
            }
            string stri_new = cmd_ar[dem];
            for (int i = dem; i < cmd_ar.Length; i++) cmd_run.Text = cmd_run.Text + cmd_ar[i] + System.Environment.NewLine + System.Environment.NewLine;
            string[] str_arr = stri_new.Split();
            for (int i = 0; i < str_arr.Length; i++)
            {
                if (str_arr[i].Contains('x')) tmp = str_arr[i].Substring(str_arr[i].IndexOf('x'), 1);
                if (str_arr[i].Contains('y')) tmp = str_arr[i].Substring(str_arr[i].IndexOf('y'), 1);
                if (str_arr[i].Contains('z')) tmp = str_arr[i].Substring(str_arr[i].IndexOf('z'), 1);
                if (str_arr[i].Contains('m')) tmp = str_arr[i].Substring(str_arr[i].IndexOf('m'), 1);

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
                    case "m":
                        m = str_arr[i].Trim('m');
                        break;
                }
                write_PLC();
            }
            dem++;
            x = "";
            y = "";
            z = "";
            m = "";

            byte[] M0_0_On = { 1 };  // Array byte have 1 element | Number 1 => M0.0 = true

            plc_s7_1200.WriteBytes(DataType.Memory, 0, 0, M0_0_On); // Write 1 byte to Memory

            if (dem == cmd_ar.Length - 1)
            {
                signFinish = true;
                MessageBox.Show("Complete command"); 
                dem = 0;
            }

            button1.Enabled = false;
        }
        #endregion

        #endregion

        #region Kiểm tra ky tự
        private void btn_check_Click(object sender, EventArgs e)
        {
            if (cmd_input.Text != "")
            {
                string chuoi;
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

                    string[] lenh = chuoi.Split(';');

                    for (int i = 0; i < solenh; i++)
                    {
                        string s = lenh[i];
                        s = s.Trim();
                        string[] lenhh = s.Split();

                        for (int j = 0; j < lenhh.Length; j++)
                        {
                            //TH2: Kiểm tra ký tự có phù hợp không
                            string[] chuoikt = { "q", "w", "e", "r", "t", "u", "i", "o", "p", "a", "f", "q", "g", "h", "j", "k", "l", "c", "v", "b", "n", ":", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "=", "[", "]", "{", "}", "<", ">", "?", "/" };
                            for (int k = 0; k < chuoikt.Length; k++)
                            {
                                if (lenhh[j].Contains(chuoikt[k]) == true)
                                {
                                    failcombine2 = true;
                                    MessageBox.Show("Err chua ky tu khong hop le");
                                    break;
                                }
                            }
                            if (failcombine2 == true) break;

                            string[] chuoihl = { "x", "y", "z", "t", "d" };
                            int dem = 0;
                            for (int k = 0; k < chuoihl.Length; k++)
                            {
                                if (lenhh[j].Contains(chuoihl[k]) == true)
                                {
                                    dem++;
                                }
                            }
                            if (dem >= 2)
                            {
                                failcombine6 = true;
                                MessageBox.Show("Err lap toa do");
                                break;
                            }

                            //TH3: Kiểm tra lệnh có chứa toạ độ số không
                            string[] chuoiso = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                            if ((lenhh[j].Contains(chuoiso[0]) || lenhh[j].Contains(chuoiso[1]) || lenhh[j].Contains(chuoiso[2]) || lenhh[j].Contains(chuoiso[3]) || lenhh[j].Contains(chuoiso[4]) || lenhh[j].Contains(chuoiso[5]) || lenhh[j].Contains(chuoiso[6]) || lenhh[j].Contains(chuoiso[7]) || lenhh[j].Contains(chuoiso[8]) || lenhh[j].Contains(chuoiso[9])) == true)
                            {
                                
                            }
                            else
                            {
                                failcombine3 = true;
                                MessageBox.Show("Err khong chua so");
                                break;
                            }

                            //TH4: Kiểm tra toạ độ có bắt đầu bằng ký tự hợp lệ không
                            char[] char1 = lenhh[j].ToCharArray();  
                            if (char1[0] == 'x' || char1[0] == 'y' || char1[0] == 'z' || char1[0] == 's' || char1[0] == 'd' || char1[0] == 'm')
                            {

                            }
                            else
                            {
                                failcombine4 = true;
                                MessageBox.Show("Err bat dau khong hop le");
                                break;
                            }
                        }
                        //TH5: Lặp kí tự
                        if ((s.IndexOf("x") != s.LastIndexOf("x")) || (s.IndexOf("y") != s.LastIndexOf("y")) || (s.IndexOf("z") != s.LastIndexOf("z")) || (s.IndexOf("m") != s.LastIndexOf("m")) || (s.IndexOf("s") != s.LastIndexOf("s")) || (s.IndexOf("d") != s.LastIndexOf("d")))
                        {
                            failcombine5 = true;
                            MessageBox.Show("Err lap ky tu");
                            break;
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
                    button1.Enabled = true;
                }
            }
            else MessageBox.Show("Please type command");
            
        }
        #endregion

        public bool check(string cmd)
        {
            string chuoi = cmd;
            bool fail1, fail2, fail3, fail4, fail5, fail6;

            fail1 = false; 
            fail2 = false; 
            fail3 = false; 
            fail4 = false; 
            fail5 = false; 
            fail6 = false;

            chuoi = cmd.ToLower();
            chuoi = chuoi.Trim();
            Regex trimmer = new Regex(@"\s\s+");
            chuoi = trimmer.Replace(chuoi, " ");
            string[] lenh = chuoi.Split();

            for (int j = 0; j < lenh.Length; j++)
            {
                //TH2: Kiểm tra ký tự có phù hợp không
                string[] chuoikt = { "q", "w", "e", "r", "t", "u", "i", "o", "p", "a", "f", "q", "g", "h", "j", "k", "l", "c", "v", "b", "n", ":", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "=", "[", "]", "{", "}", "<", ">", "?", ",", "/" };
                for (int k = 0; k < chuoikt.Length; k++)
                {
                    if (lenh[j].Contains(chuoikt[k]) == true)
                    {
                        fail2 = true;
                        failcombine = true;
                    }
                }

                string[] chuoihl = { "x", "y", "z", "t", "d" };
                int dem = 0;
                for (int k = 0; k < chuoihl.Length; k++)
                {
                    if (lenh[j].Contains(chuoihl[k]) == true)
                    {
                        dem++;
                    }
                }
                if (dem >= 2)
                {
                    failcombine6 = true;                                       
                }

                //TH3: Kiểm tra lệnh có chứa toạ độ số không
                string[] chuoiso = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                if ((lenh[j].Contains(chuoiso[0]) || lenh[j].Contains(chuoiso[1]) || lenh[j].Contains(chuoiso[2]) || lenh[j].Contains(chuoiso[3]) || lenh[j].Contains(chuoiso[4]) || lenh[j].Contains(chuoiso[5]) || lenh[j].Contains(chuoiso[6]) || lenh[j].Contains(chuoiso[7]) || lenh[j].Contains(chuoiso[8]) || lenh[j].Contains(chuoiso[9])) == true)
                {

                }
                else
                {
                    fail3 = true;
                    failcombine = true;
                }

                //TH4: Kiểm tra toạ độ có bắt đầu bằng ký tự hợp lệ không
                char[] char1 = lenh[j].ToCharArray(); 
                if (char1[0] == 'x' || char1[0] == 'y' || char1[0] == 'z' || char1[0] == 's' || char1[0] == 'd' || char1[0] == 'm')
                {

                }
                else
                {
                    fail4 = true;
                    failcombine = true;
                }
            }

            //TH5: Lặp kí tự
            if ((chuoi.IndexOf("x") != chuoi.LastIndexOf("x")) || (chuoi.IndexOf("y") != chuoi.LastIndexOf("y")) || (chuoi.IndexOf("z") != chuoi.LastIndexOf("z")) || (chuoi.IndexOf("m") != chuoi.LastIndexOf("m")) || (chuoi.IndexOf("s") != chuoi.LastIndexOf("s")) || (chuoi.IndexOf("d") != chuoi.LastIndexOf("d")))
            {
                fail5 = true;
                failcombine = true;
            }

            if ((fail1 || fail2 || fail3 || fail4 || fail5 || fail6) == true)
            {
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
                return true;
            }
        }

        public void write_PLC()
        {
            DB4 db1 = new DB4();
            TB_Write_x.Text = x;
            TB_Write_y.Text = y;
            TB_Write_z.Text = z;
            
            if (TB_Write_x.Text != "") db1.x = Convert.ToSingle(TB_Write_x.Text);

            if (TB_Write_y.Text != "") db1.y = Convert.ToSingle(TB_Write_y.Text);

            if (TB_Write_z.Text != "") db1.z = Convert.ToSingle(TB_Write_z.Text);

            db1.m = Convert.ToSingle(m);

            plc_s7_1200.WriteClass(db1, 1);

        }

        public void tranfer()
        {
            signFinish = false;
            stri_cmd = cmd_input.Text;
            cmd_run.Text = "";

            string stri_temp = stri_cmd;
            string[] cmd_ar = stri_temp.Split(';');
            for (int i = 0; i < cmd_ar.Length; i++)
            {
                Regex trimmer = new Regex(@"\s\s+");
                cmd_ar[i] = trimmer.Replace(cmd_ar[i], " ");
            }
            string stri_new = cmd_ar[dem];
            for (int i = dem; i < cmd_ar.Length; i++) cmd_run.Text = cmd_run.Text + cmd_ar[i] + System.Environment.NewLine + System.Environment.NewLine;
            string[] str_arr = stri_new.Split();
            for (int i = 0; i < str_arr.Length; i++)
            {
                if (str_arr[i].Contains('x')) tmp = str_arr[i].Substring(str_arr[i].IndexOf('x'), 1);
                if (str_arr[i].Contains('y')) tmp = str_arr[i].Substring(str_arr[i].IndexOf('y'), 1);
                if (str_arr[i].Contains('z')) tmp = str_arr[i].Substring(str_arr[i].IndexOf('z'), 1);
                if (str_arr[i].Contains('m')) tmp = str_arr[i].Substring(str_arr[i].IndexOf('m'), 1);

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
                    case "m":
                        m = str_arr[i].Trim('m');
                        break;
                }
                write_PLC();
            }
            dem++;
            x = "";
            y = "";
            z = "";
            m = "";

            byte[] M0_0_On = { 1 };  // Array byte have 1 element | Number 1 => M0.0 = true

            plc_s7_1200.WriteBytes(DataType.Memory, 0, 0, M0_0_On); // Write 1 byte to Memory

            if (dem == cmd_ar.Length - 1)
            {
                signFinish = true;
                MessageBox.Show("Complete command");
                dem = 0;  
            }







        }
    }
}