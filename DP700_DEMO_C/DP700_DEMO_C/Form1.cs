using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using DP700_TIMER_C;
using System.Threading;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace DP700_TIMER_C
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void comBox_MouseClick(object sender, EventArgs e)
        {
            bool bcomExit = false;
            SerialportBox.Items.Clear();
            for (int i = 0; i < 15; i++)
            {
                try
                {
                    SerialPort sp = new SerialPort("COM" + (i + 1).ToString());
                    sp.Open();
                    sp.Close();
                    SerialportBox.Items.Add("COM" + (i + 1).ToString());
                    bcomExit = true;
                }
                catch (Exception ep)
                {
                    //MessageBox.Show(ep.Message);
                    continue;
                }
            }
            if (bcomExit)
            {
                SerialportBox.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("No available Serial Port Found!");
            }

        }
        private bool bOpen = false;
        private void power_MouseClick(object sender, EventArgs e)
        {
            if (bOpen)
            {
                bOpen = false;
                serialPort.Write(":OUTPUT:STATE CH1,OFF\n");
                Power_button.Text = "Open Power";
            }
            else
            {
                bOpen = true;
                serialPort.Write(":OUTPUT:STATE CH1,ON\n");
                Power_button.Text = "Close Power";
            }
        }

        private void OpenSerial_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
            try
            {
                serialPort.PortName = SerialportBox.Text.Trim();//串口号，Trim（）是一个字符串方法，用于删除字符串开头和结尾的空白字符
                serialPort.BaudRate = int.Parse(BaudBox.Text);//波特率
                serialPort.DataBits = int.Parse(DataBox.Text);//数据位
            }
            catch (Exception)
            {
                MessageBox.Show("Error");
            }
            float fstopValue = Convert.ToSingle(StopBox.Text.Trim());//将文本框的内容转换为单精度浮点数类型
            if (fstopValue == 0)
            {
                serialPort.StopBits = StopBits.None;
            }
            else if (fstopValue == 1)
            {
                serialPort.StopBits = StopBits.One;
            }
            if (ParityBox.Text.Trim() == "None")
            {
                serialPort.Parity = Parity.None;
            }
            else if (ParityBox.Text.Trim() == "Odd")
            {
                serialPort.Parity = Parity.Odd;
            }
            else
            {
                serialPort.Parity = Parity.Even;
            }
            try
            {
                serialPort.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("Error!");
            }
        }

        


       
        private void OpenTimer_Click(object sender, EventArgs e)
        {
            Mat srcImage = new Mat(new OpenCvSharp.Size(500, 500), MatType.CV_8UC3, Scalar.All(0));
            Cv2.Circle(srcImage, 250, 250, 100, new Scalar(255, 0, 0), 20);
            Cv2.Circle(srcImage, 250, 250, 80, new Scalar(0, 255, 0), 20);
            Cv2.Circle(srcImage, 250, 250, 60, new Scalar(0, 0, 255), 20);

            Bitmap bitmap = BitmapConverter.ToBitmap(srcImage);
            pictureBox1.Image = bitmap;
        }

        private void Waveform1_Click(object sender, EventArgs e)
        {
            //定时器输出总组数=输出组数*循环数
            serialPort.Write(":TIMEr:STATe OFF\n");//关闭定时器
            Thread.Sleep(100);
            serialPort.Write(":TIMEr:GROUPs 21\n");//设置定时器输出组数
            Thread.Sleep(100);
            serialPort.Write(":TIMEr:CYCLEs N,10\n");//设置定时器循环数
            Thread.Sleep(100);
            //设置定时器出发模式为自动（DEFault），意为按照定时器参数配置依次输出，直至完成总组数输出
            //另一种触发模式为SINGle，意为定时器输出需要每次按OK键，不会按照输出组数依次往后输出
            serialPort.Write(":TIMEr:TRIGer DEFault\n");
            Thread.Sleep(100);
            //设置定时器终止状态为最后一组（LAST），意为当输出完成后，仪器保持最后一组的输出状态
            //与之相对应的是关闭（OFF），意为当输出完成后，仪器关闭输出
            serialPort.Write(":TIMEr:ENDState LAST\n");
            Thread.Sleep(100);
            serialPort.Write(":TIMEr:PARAmeter 1,13.5,3,2\n");//设置第一组参数电压13.5V，电流3A，持续事件2s
            Thread.Sleep(100);
            serialPort.Write(":TIMEr:PARAmeter 2,10.0,3,0.01\n");//线性变换的曲线无法输出，此处我们取值v1和v3的平均值，并让其输出0.01s
            Thread.Sleep(100);
            serialPort.Write(":TIMEr:PARAmeter 3,6.5,2,0.01\n");
            Thread.Sleep(100);

            /*在电压由V3（6.5V）变到V2（8.5V）的过程中，持续时间为50ms，
             * 由于DP700系列自身的缺陷，设置定时器参数只能按照固有格式输出，不能使用变量，
             * 没有内置的线性波形方法，由于时间的最小值为10ms，由于波形一的线性变化时间不久，
             * 所以我们按照10ms的最小时间变化，每过10ms，电压上升0.4V，五个10ms后，从6.5V变化到8.5V，
             * 代码见下面的五行，定时器的第四组参数到第八组参数。*/
            serialPort.Write(":TIMEr:PARAmeter 4,6.9,2,0.01\n");
            Thread.Sleep(100);
            serialPort.Write(":TIMEr:PARAmeter 5,7.3,2,0.01\n");
            Thread.Sleep(100);
            serialPort.Write(":TIMEr:PARAmeter 6,7.7,2,0.01\n");
            Thread.Sleep(100);
            serialPort.Write(":TIMEr:PARAmeter 7,8.1,2,0.01\n");
            Thread.Sleep(100);
            serialPort.Write(":TIMEr:PARAmeter 8,8.5,2,0.01\n");
            Thread.Sleep(100);

            serialPort.Write(":TIMEr:PARAmeter 9,8.5,2,5\n");
            Thread.Sleep(100);
            serialPort.Write(":TIMEr:PARAmeter 10,9,2,0.01\n");
            Thread.Sleep(100);
            serialPort.Write(":TIMEr:PARAmeter 11,9.5,2,0.01\n");
            Thread.Sleep(100);
            serialPort.Write(":TIMEr:PARAmeter 12,10,2,0.01\n");
            Thread.Sleep(100);
            serialPort.Write(":TIMEr:PARAmeter 13,10.5,2,0.01\n");
            Thread.Sleep(100);
            serialPort.Write(":TIMEr:PARAmeter 14,11,2,0.01\n");
            Thread.Sleep(100);
            serialPort.Write(":TIMEr:PARAmeter 15,11.5,2,0.01\n");
            Thread.Sleep(100);
            serialPort.Write(":TIMEr:PARAmeter 16,12,2,0.01\n");
            Thread.Sleep(100);
            serialPort.Write(":TIMEr:PARAmeter 17,12.5,2,0.01\n");
            Thread.Sleep(100);
            serialPort.Write(":TIMEr:PARAmeter 18,13,2,0.01\n");
            Thread.Sleep(100);
            serialPort.Write(":TIMEr:PARAmeter 19,13.5,2,0.01\n");
            Thread.Sleep(100);

            serialPort.Write(":TIMEr:PARAmeter 20,13.5,2,5\n");
            Thread.Sleep(100);
            serialPort.Write(":TIMEr:PARAmeter 21,0,2,1\n");
            Thread.Sleep(100);
            
        }
       private bool state = true;
        private void Timer_Click(object sender, EventArgs e)
        {

            if (state == true)
            {
                serialPort.Write(":TIMEr:STATe ON\n");
                state = false;
            }
            else
            {
                serialPort.Write(":TIMEr:STATe OFF\n");
                state = true;
            }
        }

        private void Waveform2_Click(object sender, EventArgs e)
        {
            serialPort.Write(":TIMEr:STATe OFF\n");
            serialPort.Write(":TIMEr:GROUPs 41\n");
            serialPort.Write(":TIMEr:CYCLEs N,2\n");
            serialPort.Write(":TIMEr:TRIGer DEFault\n");
            serialPort.Write(":TIMEr:ENDState LAST\n");

            /* 根据波形二的波形图，循环输出20次，再将13.5V电压恒压输出10s，此处不能靠循环组数来达到循环二十次
             ，只能手动输入数值，就像下面的代码一样，从第一组输出到第40组，第41组再输出一个持续10s的13.5V的电压*/
            serialPort.Write(":TIMEr:PARAmeter 1,13.5,3,1\n");
            serialPort.Write(":TIMEr:PARAmeter 2,0,1,1\n");
            serialPort.Write(":TIMEr:PARAmeter 3,13.5,3,1\n");
            serialPort.Write(":TIMEr:PARAmeter 4,0,1,1\n");
            serialPort.Write(":TIMEr:PARAmeter 5,13.5,3,1\n");
            serialPort.Write(":TIMEr:PARAmeter 6,0,1,1\n");
            serialPort.Write(":TIMEr:PARAmeter 7,13.5,3,1\n");
            serialPort.Write(":TIMEr:PARAmeter 8,0,1,1\n");
            serialPort.Write(":TIMEr:PARAmeter 9,13.5,3,1\n");
            serialPort.Write(":TIMEr:PARAmeter 10,0,1,1\n");
            serialPort.Write(":TIMEr:PARAmeter 11,13.5,3,1\n");
            serialPort.Write(":TIMEr:PARAmeter 12,0,1,1\n");
            serialPort.Write(":TIMEr:PARAmeter 13,13.5,3,1\n");
            serialPort.Write(":TIMEr:PARAmeter 14,0,1,1\n");
            serialPort.Write(":TIMEr:PARAmeter 15,13.5,3,1\n");
            serialPort.Write(":TIMEr:PARAmeter 16,0,1,1\n");
            serialPort.Write(":TIMEr:PARAmeter 17,13.5,3,1\n");
            serialPort.Write(":TIMEr:PARAmeter 18,0,1,1\n");
            serialPort.Write(":TIMEr:PARAmeter 19,13.5,3,1\n");
            serialPort.Write(":TIMEr:PARAmeter 20,0,1,1\n");
            serialPort.Write(":TIMEr:PARAmeter 21,13.5,3,1\n");
            serialPort.Write(":TIMEr:PARAmeter 22,0,1,1\n");
            serialPort.Write(":TIMEr:PARAmeter 23,13.5,3,1\n");
            serialPort.Write(":TIMEr:PARAmeter 24,0,1,1\n");
            serialPort.Write(":TIMEr:PARAmeter 25,13.5,3,1\n");
            serialPort.Write(":TIMEr:PARAmeter 26,0,1,1\n");
            serialPort.Write(":TIMEr:PARAmeter 27,13.5,3,1\n");
            serialPort.Write(":TIMEr:PARAmeter 28,0,1,1\n");
            serialPort.Write(":TIMEr:PARAmeter 29,13.5,3,1\n");
            serialPort.Write(":TIMEr:PARAmeter 30,0,1,1\n");
            serialPort.Write(":TIMEr:PARAmeter 31,13.5,3,1\n");
            serialPort.Write(":TIMEr:PARAmeter 32,0,1,1\n");
            serialPort.Write(":TIMEr:PARAmeter 33,13.5,3,1\n");
            serialPort.Write(":TIMEr:PARAmeter 34,0,1,1\n");
            serialPort.Write(":TIMEr:PARAmeter 35,13.5,3,1\n");
            serialPort.Write(":TIMEr:PARAmeter 36,0,1,1\n");
            serialPort.Write(":TIMEr:PARAmeter 37,13.5,3,1\n");
            serialPort.Write(":TIMEr:PARAmeter 38,0,1,1\n");
            serialPort.Write(":TIMEr:PARAmeter 39,13.5,3,1\n");
            serialPort.Write(":TIMEr:PARAmeter 40,0,1,1\n");
            serialPort.Write(":TIMEr:PARAmeter 41,13.5,2,10\n");

            
        }

        private void Waveform3_Click(object sender, EventArgs e)
        {
            serialPort.Write(":TIMEr:STATe OFF\n");
            serialPort.Write(":TIMEr:GROUPs 546\n");
            serialPort.Write(":TIMEr:CYCLEs N,1\n");
            serialPort.Write(":TIMEr:TRIGer DEFault\n");
            serialPort.Write(":TIMEr:ENDState LAST\n");

            {
                serialPort.Write(":TIMEr:PARAmeter 1,0.1,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 2,0.2,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 3,0.3,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 4,0.4,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 5,0.5,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 6,0.6,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 7,0.7,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 8,0.8,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 9,0.9,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 10,1.0,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 11,1.1,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 12,1.2,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 13,1.3,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 14,1.4,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 15,1.5,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 16,1.6,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 17,1.7,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 18,1.8,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 19,1.9,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 20,2.0,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 21,2.1,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 22,2.2,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 23,2.3,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 24,2.4,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 25,2.5,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 26,2.6,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 27,2.7,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 28,2.8,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 29,2.9,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 30,3.0,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 31,3.1,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 32,3.2,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 33,3.3,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 34,3.4,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 35,3.5,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 36,3.6,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 37,3.7,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 38,3.8,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 39,3.9,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 40,4.0,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 41,4.1,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 42,4.2,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 43,4.3,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 44,4.4,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 45,4.5,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 46,4.6,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 47,4.7,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 48,4.8,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 49,4.9,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 50,5.0,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 51,5.1,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 52,5.2,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 53,5.3,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 54,5.4,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 55,5.5,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 56,5.6,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 57,5.7,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 58,5.8,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 59,5.9,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 60,6.0,2.0,0.5\n");
            }

            serialPort.Write(":TIMEr:PARAmeter 61,6,2,2\n");

            {
                serialPort.Write(":TIMEr:PARAmeter 62,6.05,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 63,6.10,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 64,6.15,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 65,6.20,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 66,6.25,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 67,6.30,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 68,6.35,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 69,6.40,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 70,6.45,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 71,6.50,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 72,6.55,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 73,6.60,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 74,6.65,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 75,6.70,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 76,6.75,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 77,6.80,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 78,6.85,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 79,6.90,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 80,6.95,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 81,7.00,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 82,7.05,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 83,7.10,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 84,7.15,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 85,7.20,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 86,7.25,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 87,7.30,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 88,7.35,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 89,7.40,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 90,7.45,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 91,7.50,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 92,7.55,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 93,7.60,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 94,7.65,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 95,7.70,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 96,7.75,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 97,7.80,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 98,7.85,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 99,7.90,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 100,7.95,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 101,8.00,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 102,8.05,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 103,8.10,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 104,8.15,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 105,8.20,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 106,8.25,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 107,8.30,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 108,8.35,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 109,8.40,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 110,8.45,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 111,8.50,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 112,8.55,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 113,8.60,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 114,8.65,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 115,8.70,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 116,8.75,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 117,8.80,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 118,8.85,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 119,8.90,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 120,8.95,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 121,9.00,2.0,0.5\n");
            }

            serialPort.Write(":TIMEr:PARAmeter 122,9,2,5\n");

            {
                serialPort.Write(":TIMEr:PARAmeter 123,9.07,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 124,9.15,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 125,9.22,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 126,9.30,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 127,9.37,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 128,9.45,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 129,9.52,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 130,9.60,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 131,9.67,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 132,9.75,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 133,9.82,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 134,9.90,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 135,9.97,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 136,10.05,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 137,10.12,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 138,10.20,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 139,10.27,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 140,10.35,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 141,10.42,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 142,10.50,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 143,10.57,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 144,10.65,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 145,10.72,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 146,10.80,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 147,10.87,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 148,10.95,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 149,11.02,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 150,11.10,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 151,11.17,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 152,11.25,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 153,11.32,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 154,11.40,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 155,11.47,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 156,11.55,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 157,11.62,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 158,11.70,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 159,11.77,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 160,11.85,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 161,11.92,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 162,12.00,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 163,12.07,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 164,12.15,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 165,12.22,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 166,12.30,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 167,12.37,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 168,12.45,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 169,12.52,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 170,12.60,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 171,12.67,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 172,12.75,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 173,12.82,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 174,12.90,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 175,12.97,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 176,13.05,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 177,13.12,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 178,13.20,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 179,13.27,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 180,13.35,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 181,13.42,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 182,13.50,2.0,0.5\n");
            }

            serialPort.Write(":TIMEr:PARAmeter 183,13.5,2.0,10\n");

            {
                serialPort.Write(":TIMEr:PARAmeter 184,13.39,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 185,13.27,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 186,13.16,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 187,13.05,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 188,12.94,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 189,12.82,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 190,12.71,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 191,12.60,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 192,12.49,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 193,12.37,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 194,12.26,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 195,12.15,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 196,12.04,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 197,11.92,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 198,11.81,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 199,11.70,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 200,11.59,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 201,11.47,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 202,11.36,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 203,11.25,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 204,11.14,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 205,11.02,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 206,10.91,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 207,10.80,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 208,10.69,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 209,10.57,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 210,10.46,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 211,10.35,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 212,10.24,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 213,10.12,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 214,10.01,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 215,9.90,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 216,9.79,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 217,9.67,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 218,9.56,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 219,9.45,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 220,9.34,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 221,9.22,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 222,9.11,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 223,9.00,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 224,8.89,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 225,8.77,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 226,8.66,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 227,8.55,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 228,8.44,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 229,8.32,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 230,8.21,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 231,8.10,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 232,7.99,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 233,7.87,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 234,7.76,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 235,7.65,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 236,7.54,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 237,7.42,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 238,7.31,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 239,7.20,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 240,7.09,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 241,6.97,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 242,6.86,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 243,6.75,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 244,6.64,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 245,6.52,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 246,6.41,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 247,6.30,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 248,6.19,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 249,6.07,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 250,5.96,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 251,5.85,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 252,5.74,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 253,5.62,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 254,5.51,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 255,5.40,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 256,5.29,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 257,5.17,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 258,5.06,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 259,4.95,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 260,4.84,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 261,4.72,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 262,4.61,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 263,4.50,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 264,4.39,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 265,4.27,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 266,4.16,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 267,4.05,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 268,3.94,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 269,3.82,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 270,3.71,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 271,3.60,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 272,3.49,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 273,3.37,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 274,3.26,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 275,3.15,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 276,3.04,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 277,2.92,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 278,2.81,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 279,2.70,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 280,2.59,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 281,2.47,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 282,2.36,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 283,2.25,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 284,2.14,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 285,2.02,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 286,1.91,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 287,1.80,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 288,1.69,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 289,1.57,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 290,1.46,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 291,1.35,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 292,1.24,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 293,1.12,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 294,1.01,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 295,0.90,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 296,0.79,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 297,0.67,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 298,0.56,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 299,0.45,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 300,0.34,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 301,0.22,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 302,0.11,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 303,0.00,2.0,0.5\n");
            }
            serialPort.Write(":TIMEr:PARAmeter 304,0,2,30\n");

            {
                serialPort.Write(":TIMEr:PARAmeter 305,0.22,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 306,0.43,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 307,0.65,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 308,0.86,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 309,1.08,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 310,1.30,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 311,1.51,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 312,1.73,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 313,1.94,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 314,2.16,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 315,2.38,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 316,2.59,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 317,2.81,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 318,3.02,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 319,3.24,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 320,3.46,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 321,3.67,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 322,3.89,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 323,4.10,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 324,4.32,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 325,4.54,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 326,4.75,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 327,4.97,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 328,5.18,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 329,5.40,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 330,5.62,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 331,5.83,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 332,6.05,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 333,6.26,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 334,6.48,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 335,6.70,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 336,6.91,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 337,7.13,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 338,7.34,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 339,7.56,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 340,7.78,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 341,7.99,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 342,8.21,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 343,8.42,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 344,8.64,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 345,8.86,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 346,9.07,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 347,9.29,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 348,9.50,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 349,9.72,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 350,9.94,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 351,10.15,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 352,10.37,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 353,10.58,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 354,10.80,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 355,11.02,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 356,11.23,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 357,11.45,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 358,11.66,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 359,11.88,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 360,12.10,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 361,12.31,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 362,12.53,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 363,12.74,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 364,12.96,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 365,13.18,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 366,13.39,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 367,13.61,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 368,13.82,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 369,14.04,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 370,14.26,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 371,14.47,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 372,14.69,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 373,14.90,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 374,15.12,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 375,15.34,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 376,15.55,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 377,15.77,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 378,15.98,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 379,16.20,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 380,16.42,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 381,16.63,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 382,16.85,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 383,17.06,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 384,17.28,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 385,17.50,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 386,17.71,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 387,17.93,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 388,18.14,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 389,18.36,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 390,18.58,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 391,18.79,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 392,19.01,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 393,19.22,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 394,19.44,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 395,19.66,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 396,19.87,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 397,20.09,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 398,20.30,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 399,20.52,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 400,20.74,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 401,20.95,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 402,21.17,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 403,21.38,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 404,21.60,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 405,21.82,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 406,22.03,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 407,22.25,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 408,22.46,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 409,22.68,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 410,22.90,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 411,23.11,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 412,23.33,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 413,23.54,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 414,23.76,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 415,23.98,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 416,24.19,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 417,24.41,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 418,24.62,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 419,24.84,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 420,25.06,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 421,25.27,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 422,25.49,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 423,25.70,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 424,26,2.0,0.5\n");

            }
            serialPort.Write(":TIMEr:PARAmeter 425,26,2.0,10\n");

            {
                serialPort.Write(":TIMEr:PARAmeter 426,25.79,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 427,25.58,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 428,25.38,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 429,25.17,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 430,24.96,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 431,24.75,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 432,24.54,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 433,24.34,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 434,24.13,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 435,23.92,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 436,23.71,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 437,23.50,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 438,23.30,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 439,23.09,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 440,22.88,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 441,22.67,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 442,22.46,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 443,22.26,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 444,22.05,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 445,21.84,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 446,21.63,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 447,21.42,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 448,21.22,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 449,21.01,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 450,20.80,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 451,20.59,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 452,20.38,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 453,20.18,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 454,19.97,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 455,19.76,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 456,19.55,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 457,19.34,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 458,19.14,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 459,18.93,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 460,18.72,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 461,18.51,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 462,18.30,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 463,18.10,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 464,17.89,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 465,17.68,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 466,17.47,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 467,17.26,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 468,17.06,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 469,16.85,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 470,16.64,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 471,16.43,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 472,16.22,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 473,16.02,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 474,15.81,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 475,15.60,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 476,15.39,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 477,15.18,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 478,14.98,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 479,14.77,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 480,14.56,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 481,14.35,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 482,14.14,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 483,13.94,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 484,13.73,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 485,13.5,2.0,0.5\n");
            }

            serialPort.Write(":TIMEr:PARAmeter 486,13.5,2.0,10\n");

            {
                serialPort.Write(":TIMEr:PARAmeter 487,13.28,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 488,13.05,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 489,12.83,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 490,12.60,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 491,12.38,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 492,12.15,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 493,11.93,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 494,11.70,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 495,11.48,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 496,11.25,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 497,11.03,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 498,10.80,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 499,10.58,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 500,10.35,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 501,10.13,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 502,9.90,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 503,9.68,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 504,9.45,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 505,9.23,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 506,9.00,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 507,8.78,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 508,8.55,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 509,8.33,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 510,8.10,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 511,7.88,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 512,7.65,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 513,7.43,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 514,7.20,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 515,6.98,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 516,6.75,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 517,6.53,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 518,6.30,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 519,6.08,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 520,5.85,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 521,5.63,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 522,5.40,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 523,5.18,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 524,4.95,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 525,4.73,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 526,4.50,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 527,4.28,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 528,4.05,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 529,3.83,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 530,3.60,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 531,3.38,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 532,3.15,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 533,2.93,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 534,2.70,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 535,2.48,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 536,2.25,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 537,2.03,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 538,1.80,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 539,1.58,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 540,1.35,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 541,1.13,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 542,0.90,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 543,0.68,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 544,0.45,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 545,0.23,2.0,0.5\n");
                serialPort.Write(":TIMEr:PARAmeter 546,0.00,2.0,0.5\n");
            }



        }

        private void Waveform4_Click(object sender, EventArgs e)
        {
            serialPort.Write(":TIMEr:STATe OFF\n");
            Thread.Sleep(100);
            serialPort.Write(":TIMEr:GROUPs 4\n");
            Thread.Sleep(100);
            serialPort.Write(":TIMEr:CYCLEs N,1\n");
            Thread.Sleep(100);

            serialPort.Write(":TIMEr:TRIGer DEFault\n");
            Thread.Sleep(100);
            serialPort.Write(":TIMEr:ENDState LAST\n");
            Thread.Sleep(100);
            
            serialPort.Write(":TIMEr:PARAmeter 1,13.5,2,2\n");
            Thread.Sleep(100);

            serialPort.Write(":TIMEr:PARAmeter 2,4.5,2,0.01\n");
            Thread.Sleep(100);

            serialPort.Write(":TIMEr:PARAmeter 3,13.5,2,5\n");
            Thread.Sleep(100);

            serialPort.Write(":TIMEr:PARAmeter 4,0,2,1\n");
            Thread.Sleep(100);


        }

        private void Waveform5_Click(object sender, EventArgs e)
        {
            serialPort.Write(":TIMEr:STATe OFF\n");
            serialPort.Write(":TIMEr:GROUPs 25\n");
            serialPort.Write(":TIMEr:CYCLEs N,1\n");
            serialPort.Write(":TIMEr:TRIGer DEFault\n");
            serialPort.Write(":TIMEr:ENDState LAST\n");

            serialPort.Write(":TIMEr:PARAmeter 1,0,2,3\n");
            serialPort.Write(":TIMEr:PARAmeter 2,13.5,2,0.5\n");
            serialPort.Write(":TIMEr:PARAmeter 3,0,2,0.5\n");
            serialPort.Write(":TIMEr:PARAmeter 4,13.5,2,0.5\n");
            serialPort.Write(":TIMEr:PARAmeter 5,0,2,0.5\n");
            serialPort.Write(":TIMEr:PARAmeter 6,13.5,2,0.5\n");
            serialPort.Write(":TIMEr:PARAmeter 7,0,2,0.5\n");
            serialPort.Write(":TIMEr:PARAmeter 8,13.5,2,0.5\n");
            serialPort.Write(":TIMEr:PARAmeter 9,0,2,0.5\n");
            serialPort.Write(":TIMEr:PARAmeter 10,13.5,2,0.5\n");
            serialPort.Write(":TIMEr:PARAmeter 11,0,2,0.5\n");

            serialPort.Write(":TIMEr:PARAmeter 12,13.5,2,10\n");
            serialPort.Write(":TIMEr:PARAmeter 13,0,2,0.5\n");
            serialPort.Write(":TIMEr:PARAmeter 14,13.5,2,0.5\n");
            serialPort.Write(":TIMEr:PARAmeter 15,0,2,0.5\n");
            serialPort.Write(":TIMEr:PARAmeter 16,13.5,2,0.5\n");
            serialPort.Write(":TIMEr:PARAmeter 17,0,2,0.5\n");
            serialPort.Write(":TIMEr:PARAmeter 18,13.5,2,0.5\n");
            serialPort.Write(":TIMEr:PARAmeter 19,0,2,0.5\n");
            serialPort.Write(":TIMEr:PARAmeter 20,13.5,2,0.5\n");
            serialPort.Write(":TIMEr:PARAmeter 21,0,2,0.5\n");
            serialPort.Write(":TIMEr:PARAmeter 22,13.5,2,0.5\n");
            serialPort.Write(":TIMEr:PARAmeter 23,0,2,0.5\n");

            serialPort.Write(":TIMEr:PARAmeter 24,0,2,600\n");
            serialPort.Write(":TIMEr:PARAmeter 25,0,2,180\n");
            
        }

        private void Waveform6_Click(object sender, EventArgs e)
        {
            serialPort.Write(":TIMEr:STATe OFF\n");
            serialPort.Write(":TIMEr:GROUPs 13\n");
            serialPort.Write(":TIMEr:CYCLEs N,1\n");
            //serialPort.Write(":TIMEr:TRIGer DEFfault\n");
            //serialPort.Write(":TIMEr:ENDState LAST\n");
            serialPort.Write(":TIMEr:PARAmeter 1,13.5,2,10\n");
            serialPort.Write(":TIMEr:PARAmeter 2,0,2,0.5\n");
            serialPort.Write(":TIMEr:PARAmeter 3,13.5,2,0.5\n");
            serialPort.Write(":TIMEr:PARAmeter 4,0,2,0.5\n");
            serialPort.Write(":TIMEr:PARAmeter 5,13.5,2,0.5\n");
            serialPort.Write(":TIMEr:PARAmeter 6,0,2,0.5\n");
            serialPort.Write(":TIMEr:PARAmeter 7,13.5,2,0.5\n");
            serialPort.Write(":TIMEr:PARAmeter 8,0,2,0.5\n");
            serialPort.Write(":TIMEr:PARAmeter 9,13.5,2,0.5\n");
            serialPort.Write(":TIMEr:PARAmeter 10,0,2,0.5\n");
            serialPort.Write(":TIMEr:PARAmeter 11,13.5,2,0.5\n");

            serialPort.Write(":TIMEr:PARAmeter 12,0,2,600\n");
            serialPort.Write(":TIMEr:PARAmeter 13,0,2,180\n");


        }

       
       
    }
}
