using FSGarvityTool.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace FSGarvityTool
{
    

    public partial class Form1 : Form
    {
        public static int Con = 0;
        private DateTime current_time = new DateTime();
        public static int txf = 0;
        public static int Dtr = 1;
        public static int Rts = 0;
        //======================================
        //======================================

        public Form1()
        {
            InitializeComponent();
            button1.BackColor = Color.FromArgb(6, 196, 162);
            button6.BackColor = Color.FromArgb(6, 196, 162);
            button7.BackColor = Color.FromArgb(188, 27, 14);
            button4.Enabled = false;
            checkBox2.Checked = true;
            

            richTextBox1.AppendText("Start search serialPort...\r\n");
            SearchAndAddSerialToComboBox(serialPort1, comboBox1);           //扫描并将串口添加至下拉列表

            void SearchAndAddSerialToComboBox(SerialPort MyPort, ComboBox MyBox)
            {

                string[] ArryPort;                                          // 定义字符串数组，数组名为 Buffer
                ArryPort = SerialPort.GetPortNames();                       // SerialPort.GetPortNames()函数功能为获取计算机所有可用串口，以字符串数组形式输出
                string scom = String.Join("\r\n", ArryPort);
                richTextBox1.AppendText(scom + "\r\n");
                MyBox.Items.Clear();                                        // 清除当前组合框下拉菜单内容                  
                for (int i = 0; i < ArryPort.Length; i++)
                {
                    MyBox.Items.Add(ArryPort[i]);                           // 将所有的可用串口号添加到端口对应的组合框中
                }
                richTextBox1.AppendText("Search serialPort succeed!\r\n");
                richTextBox1.ScrollToCaret();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //扫描串口
            if (checkBox6.Checked)
            {
                //显示时间
                current_time = System.DateTime.Now;     //获取当前时间
                richTextBox1.AppendText(current_time.ToString("HH:mm:ss") + "  ");

            }
            richTextBox1.AppendText("Start search serialPort...\r\n");
            SearchAndAddSerialToComboBox(serialPort1, comboBox1);           //扫描并将串口添加至下拉列表

            void SearchAndAddSerialToComboBox(SerialPort MyPort, ComboBox MyBox)
            {
                
                string[] ArryPort;                                          // 定义字符串数组，数组名为 Buffer
                ArryPort = SerialPort.GetPortNames();                       // SerialPort.GetPortNames()函数功能为获取计算机所有可用串口，以字符串数组形式输出
                string scom = String.Join("\r\n", ArryPort);
                richTextBox1.AppendText(scom + "\r\n");
                MyBox.Items.Clear();                                        // 清除当前组合框下拉菜单内容                  
                for (int i = 0; i < ArryPort.Length; i++)
                {
                    MyBox.Items.Add(ArryPort[i]);                           // 将所有的可用串口号添加到端口对应的组合框中
                }
                richTextBox1.AppendText("Search serialPort succeed!\r\n");
                richTextBox1.ScrollToCaret();
            }



            /*
            void SearchAndAddSerialToComboBox(SerialPort MyPort, ComboBox MyBox)
            {                                                               //将可用端口号添加到ComboBox
                string[] MyString = new string[30];                         //最多容纳30个，太多会影响调试效率
                string Buffer;                                              //缓存
                MyBox.Items.Clear();                                        //清空ComboBox内容
                for (int i = 1; i < 20; i++)                                //循环
                {
                    try                                                     //核心原理是依靠try和catch完成遍历
                    {
                        Buffer = "COM" + i.ToString();
                        MyPort.PortName = Buffer;
                        MyPort.Open();                                      //如果失败，后面的代码不会执行
                        MyString[i - 1] = Buffer;
                        MyBox.Items.Add(Buffer);                            //打开成功，添加至下俩列表
                        MyPort.Close();                                     //关闭
                    }
                    catch
                    {

                    }
                }
                //   MyBox.Text = MyString[0];                              //初始化
            }
            */
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Con == 0)
            {
                try
                {
                    serialPort1.PortName = comboBox1.Text;                      //开启的串口名称为选择串口的ComboBox组件中的内容
                                                                                //serialPort1.BaudRate = 9600;
                    serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text);     //将选择波特率ComboBox组件中的数据转为Int型，并且进行波特率的设置

                    serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), comboBox4.Text);                       //校验位
                    serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), comboBox5.Text);                 //停止位

                    serialPort1.DataBits = Convert.ToInt32(comboBox3.Text);     //数据位8
                    serialPort1.ReadTimeout = 5000;
                    //serialPort1.DtrEnable = true;                               //启用数据终端就绪信息
                    serialPort1.Encoding = Encoding.UTF8;
                    serialPort1.ReceivedBytesThreshold = 1;                     //DataReceived触发前内部输入缓冲器的字节数

                    button3.Enabled = false;
                    button4.Enabled = true;

                    serialPort1.Open();                                         //打开串口
                    button1.BackColor = Color.FromArgb(188, 27, 14);
                    richTextBox1.AppendText("SerialPort IS OPEN" + "\r\n");
                    button1.Text = "DISCONNECT";
                    Con = 1;
                }
                catch                                                           //如果打开串口失败 需要做如下警示
                {
                    richTextBox1.AppendText("打开串口失败，请检查相关设置" + "\r\n");
                    //MessageBox.Show("打开串口失败，请检查相关设置", "错误");
                    button3.Enabled = true;
                    button4.Enabled = false;
                    Con = 0;
                }
            }
            else
            {
                //RXTextBox.Text = RXTextBox.Text + "SerialPort IS DISCONNECT\r\n";

                try
                {
                    serialPort1.Close();                                        //关闭串口
                    button3.Enabled = true;
                    button4.Enabled = false;
                    richTextBox1.AppendText("SerialPort IS CLOSE" + "\r\n");
                    button1.BackColor = Color.FromArgb(6, 196, 162);
                }
                catch (Exception)//一般情况下关闭串口不会出错，所以不需要加处理程序
                {
                    richTextBox1.AppendText("err" + "\r\n");
                    button3.Enabled = false;
                    button4.Enabled = true;
                }
                button1.Text = "CONNECT";
                Con = 0;
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void fSGarvityToolToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }

        private void fairingStudioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void 最小化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void 窗口化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {
            richTextBox1.Multiline = true;     //将Multiline属性设置为true，实现显示多行
            richTextBox1.ScrollBars = RichTextBoxScrollBars.Vertical; //设置ScrollBars属性实现只显示垂直滚动

            //richTextBox1.AppendText("Search serialPort succeed!\r\n");


            
        }

        private void SerialPort1_DataReceived1(object sender, SerialDataReceivedEventArgs e)
        {
            throw new NotImplementedException();

        }

        private void fSGarvityTool设置ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripProgressBar1_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)            // 如果串口设备已经打开了
            {
                if (!checkBox3.Checked)        // 如果是以字符的形式发送数据
                {
                    char[] str = new char[1];  // 定义一个字符数组，只有一位

                    try
                    {
                        if (checkBox6.Checked)
                        {
                            //显示时间
                            current_time = System.DateTime.Now;     //获取当前时间
                            richTextBox1.AppendText(current_time.ToString("HH:mm:ss") + "  ");

                        }
                        for (int i = 0; i < textBox1.Text.Length; i++)
                        {
                            str[0] = Convert.ToChar(textBox1.Text.Substring(i, 1));  // 取待发送文本框中的第i个字符
                            serialPort1.Write(str, 0, 1);                            // 写入串口设备进行发送
                        }
                        richTextBox1.AppendText("TX: " + textBox1.Text + "\r\n");
                        if (checkBox2.Checked)
                        {
                            richTextBox1.ScrollToCaret();
                        }
                        else
                        {

                        }
                        txf = 1;
                    }
                    catch
                    {
                        //MessageBox.Show("串口字符写入错误!", "错误");   // 弹出发送错误对话框
                        richTextBox1.AppendText("串口字符写入错误!" + "\r\n");
                        serialPort1.Close();                          // 关闭串口
                        button1.BackColor = Color.FromArgb(1, 153, 226);               // 将串口开关按键的颜色，改为青绿色
                        //button1.Text = "打开串口";                    // 将串口开关按键的文字改为“打开串口”
                    }
                }
                else                                                  // 如果以数值的形式发送
                {
                    byte[] Data = new byte[1];                        // 定义一个byte类型数据，相当于C语言的unsigned char类型
                    int flag = 0;                                     // 定义一个标志，标志这是第几位
                    try
                    {
                        if (checkBox6.Checked)
                        {
                            //显示时间
                            current_time = System.DateTime.Now;     //获取当前时间
                            richTextBox1.AppendText(current_time.ToString("HH:mm:ss") + "  ");

                        }
                        for (int i = 0; i < textBox1.Text.Length; i++)
                        {
                            if (textBox1.Text.Substring(i, 1) == " " && flag == 0)                // 如果是第一位，并且为空字符
                            {
                                continue;
                            }

                            if (textBox1.Text.Substring(i, 1) != " " && flag == 0)                // 如果是第一位，但不为空字符
                            {
                                flag = 1;                                                         // 标志转到第二位数据去
                                if (i == textBox1.Text.Length - 1)                                // 如果这是文本框字符串的最后一个字符
                                {
                                    Data[0] = Convert.ToByte(textBox1.Text.Substring(i, 1), 16);  // 转化为byte类型数据，以16进制显示
                                    serialPort1.Write(Data, 0, 1);                                // 通过串口发送
                                    richTextBox1.AppendText(Data + " ");
                                    flag = 0;                                                     // 标志回到第一位数据去
                                }
                                continue;
                            }
                            else if (textBox1.Text.Substring(i, 1) == " " && flag == 1)           // 如果是第二位，且第二位字符为空
                            {
                                Data[0] = Convert.ToByte(textBox1.Text.Substring(i - 1, 1), 16);  // 只将第一位字符转化为byte类型数据，以十六进制显示
                                serialPort1.Write(Data, 0, 1);                                    // 通过串口发送
                                richTextBox1.AppendText(Data + " ");
                                flag = 0;                                                         // 标志回到第一位数据去
                                continue;
                            }
                            else if (textBox1.Text.Substring(i, 1) != " " && flag == 1)           // 如果是第二位字符，且第一位字符不为空
                            {
                                Data[0] = Convert.ToByte(textBox1.Text.Substring(i - 1, 2), 16);  // 将第一，二位字符转化为byte类型数据，以十六进制显示
                                serialPort1.Write(Data, 0, 1);                                    // 通过串口发送
                                richTextBox1.AppendText(Data + " ");
                                flag = 0;                                                         // 标志回到第一位数据去
                                continue;
                            }

                        }
                        richTextBox1.AppendText("\r\n");
                        if (checkBox2.Checked)
                        {
                            richTextBox1.ScrollToCaret();
                        }
                        else
                        {

                        }
                        txf = 1;
                    }
                    catch
                    {
                        //MessageBox.Show("串口数值写入错误!", "错误");
                        richTextBox1.AppendText("串口字符写入错误!" + "\r\n");
                        serialPort1.Close();
                        button1.BackColor = Color.LightSkyBlue; // 将串口开关按键的颜色，改为青绿色
                    }
                }
                textBox1.Text = "";
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            //
            richTextBox1.ScrollToCaret();
            
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (txf == 1)
            {
                richTextBox1.AppendText("\r\n");
                txf = 0;
            }
            if (!checkBox1.Checked)                                         // 如果以字符串形式读取
            {
                string str = serialPort1.ReadExisting();                    // 读取串口接收缓冲区字符串
                if (checkBox6.Checked)
                {
                    //显示时间
                    current_time = System.DateTime.Now;     //获取当前时间
                    richTextBox1.AppendText(current_time.ToString("HH:mm:ss") + "  ");

                }
                richTextBox1.AppendText(str + "");                          // 在接收文本框中进行显示
                if (checkBox2.Checked)
                {
                    richTextBox1.ScrollToCaret();
                }
                else
                {

                }
                
            }
            else                                                            // 以数值形式读取
            {
                int length = serialPort1.BytesToRead;                       // 读取串口接收缓冲区字节数

                byte[] data = new byte[length];                             // 定义相同字节的数组

                serialPort1.Read(data, 0, length);                          // 串口读取缓冲区数据到数组中

                for (int i = 0; i < length; i++)
                {
                    string str = Convert.ToString(data[i], 16).ToUpper();                                   // 将数据转换为字符串格式
                    richTextBox1.AppendText("0X" + (str.Length == 1 ? "0" + str + " " : str + " "));        // 添加到串口接收文本框中
                    if (checkBox2.Checked)
                    {
                        richTextBox1.ScrollToCaret();
                    }
                    else
                    {

                    }
                }
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        
        private void button6_Click(object sender, EventArgs e)
        {
            if(Dtr == 0)
            {
                button6.BackColor = Color.FromArgb(6, 196, 162);
                serialPort1.DtrEnable = true;
                Dtr = 1;
            }
            else
            {
                button6.BackColor = Color.FromArgb(188, 27, 14);
                serialPort1.DtrEnable = false;
                Dtr = 0;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (Rts == 0)
            {
                button7.BackColor = Color.FromArgb(6, 196, 162);
                serialPort1.RtsEnable = true;
                Rts = 1;
            }
            else
            {
                button7.BackColor = Color.FromArgb(188, 27, 14);
                serialPort1.RtsEnable = false;
                Rts = 0;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            serialPort1.RtsEnable = true;
            Thread.Sleep(10);
            serialPort1.DtrEnable = true;
            Thread.Sleep(10);
            serialPort1.DtrEnable = false;
            Thread.Sleep(10);
            serialPort1.RtsEnable = false;
            button6_Click(sender, e);
            Thread.Sleep(50);
            button6_Click(sender, e);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void 下位机设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;   //将窗体一进行显示
            panel2.Visible = false;
            panel4.Visible = false;
            button9.BackColor = Color.FromArgb(128, 128, 128);
            button10.BackColor = Color.FromArgb(56, 56, 56);
            button2.BackColor = Color.FromArgb(56, 56, 56);
            button2.ForeColor = Color.FromArgb(255, 255, 255);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;   //将窗体一进行显示
            panel1.Visible = false;
            panel4.Visible = false;
            button10.BackColor = Color.FromArgb(128, 128, 128);
            button9.BackColor = Color.FromArgb(56, 56, 56);
            button2.BackColor = Color.FromArgb(56, 56, 56);
            button2.ForeColor = Color.FromArgb(255, 255, 255);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            panel4.Visible = true;   //将窗体一进行显示
            panel1.Visible = false;
            panel2.Visible = false;
            button2.ForeColor = Color.FromArgb(0, 0, 0);
            button2.BackColor = Color.FromArgb(230, 224, 0);
            button10.BackColor = Color.FromArgb(56, 56, 56);
            button9.BackColor = Color.FromArgb(56, 56, 56);
            //button2.BackgroundImage
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://fairingstudio.com/");

            //MessageBox.Show("FSGarvityTool BY FairingStudio" + "\r\n" + "2023.12.3" + "\r\n" + "BUILD V0.1.1203.2_b19 DEV", "About FSGarvityTool");//关于版本信息 "2022.8.25 V0.1"
            //About f2 = new About();
            //f2.Owner = this;
            //f2.Show();

            //System.Environment.Exit(0);//退出程序
            //this.WindowState = FormWindowState.Minimized;//最小化程序
            //this.WindowState = FormWindowState.Maximized;//最大化程序

            //Settings f2 = new Settings();
            //f2.Owner = this;
            //f2.Show();

            //Settings2 f2 = new Settings2();
            //f2.Owner = this;
            //f2.Show();
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
