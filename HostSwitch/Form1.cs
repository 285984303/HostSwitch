using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace HostSwitch
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
            List<FileInfo> files = new List<FileInfo>();
            string[] fs = System.IO.Directory.GetFiles(@"C:\Windows\System32\drivers\etc");
            foreach (string file in fs)
            {
                files.Add(new FileInfo(file));
            }
            List<FileInfo> filesOrderedDESC = files.OrderByDescending(p => p.CreationTime).ToList();
            //List<FileInfo> filesOrderedASC = files.OrderBy(p => p.LastWriteTime).ToList();
            foreach (var item in filesOrderedDESC)
            {
                if(item.Name.Contains("hosts_"))
                {
                    ListItem li = new ListItem(item.FullName, item.Name);
                    this.checkedListBox1.Items.Add(li);
                }
                
            }
            
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.checkedListBox1.SelectedIndex >= 0)
            {
                var path = ((ListItem)this.checkedListBox1.SelectedItem).ID;
                // Open the stream and read it back.
                using (FileStream fs = File.Open(path, FileMode.Open))
                {
                    byte[] b = new byte[fs.Length];
                    UTF8Encoding temp = new UTF8Encoding(true);

                    while (fs.Read(b, 0, b.Length) > 0)
                    {
                        // Console.WriteLine(temp.GetString(b));
                        this.textBox1.Text = temp.GetString(b);
                    }
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.checkedListBox1.SelectedIndex >= 0)
            {
                try
                {

                
                    // Create a temporary file, and put some data into it.
                    //string path = Path.GetTempFileName();
                    var path = ((ListItem)this.checkedListBox1.SelectedItem).ID;
                    var host = @"C:\Windows\System32\drivers\etc\hosts";
                    using (FileStream fs = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes(this.textBox1.Text);
                        // Add some information to the file.
                        fs.Write(info, 0, info.Length);
                    }

                    using (FileStream fs = File.Open(host, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        Byte[] info2 = new UTF8Encoding(true).GetBytes(this.textBox1.Text);
                        // Add some information to the file.
                        fs.Write(info2, 0, info2.Length);
                    }

                    //RunCmd();

                    FrmMsg frm = new FrmMsg("当前hosts切换到"+ ((ListItem)this.checkedListBox1.SelectedItem).Name);
                    frm.Show();

                }
                catch
                {

                }


            }
        }

        private void RunCmd()
        {
            string str = "ipconfig /flushdns"; //ipconfig /displaydns    //显示DNS缓存内容

            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;//不显示程序窗口
            p.Start();//启动程序

            //向cmd窗口发送输入信息
            p.StandardInput.WriteLine(str + "&exit");

            p.StandardInput.AutoFlush = true;
            //p.StandardInput.WriteLine("exit");
            //向标准输入写入要执行的命令。这里使用&是批处理命令的符号，表示前面一个命令不管是否执行成功都执行后面(exit)命令，如果不执行exit命令，后面调用ReadToEnd()方法会假死
            //同类的符号还有&&和||前者表示必须前一个命令执行成功才会执行后面的命令，后者表示必须前一个命令执行失败才会执行后面的命令



            //获取cmd窗口的输出信息
            string output = p.StandardOutput.ReadToEnd();

            //StreamReader reader = p.StandardOutput;
            //string line=reader.ReadLine();
            //while (!reader.EndOfStream)
            //{
            //    str += line + "  ";
            //    line = reader.ReadLine();
            //}

            p.WaitForExit();//等待程序执行完退出进程
            p.Close();


            Console.WriteLine(output);
            Debug.WriteLine(output);
            //Trace.WriteLine(output);

        }

        private void checkedListBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = checkedListBox1.IndexFromPoint(e.X, e.Y);
            checkedListBox1.SelectedIndex = index;
            if (checkedListBox1.SelectedIndex != -1)
            {
                try
                {
                    // Create a temporary file, and put some data into it.
                    //string path = Path.GetTempFileName();
                    //var path = ((ListItem)this.checkedListBox1.SelectedItem).ID;
                    var host = @"C:\Windows\System32\drivers\etc\hosts";
                    //using (FileStream fs = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.None))
                    //{
                    //    Byte[] info = new UTF8Encoding(true).GetBytes(this.textBox1.Text);
                    //    // Add some information to the file.
                    //    fs.Write(info, 0, info.Length);
                    //}

                    using (FileStream fs = File.Open(host, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        Byte[] info2 = new UTF8Encoding(true).GetBytes(this.textBox1.Text);
                        // Add some information to the file.
                        fs.Write(info2, 0, info2.Length);
                    }

                    //RunCmd();

                    FrmMsg frm = new FrmMsg("当前hosts切换到" + ((ListItem)this.checkedListBox1.SelectedItem).Name);
                    frm.Show();
                }
                catch 
                {

                    
                }
                

            }
        }

        private void btn_FlushDNS_Click(object sender, EventArgs e)
        {
            RunCmd();
        }

        private void lnk_OpenDir_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string path = @"C:\Windows\System32\drivers\etc\";
            System.Diagnostics.Process.Start("explorer.exe", path);
        }
    }
}
