namespace DP700_TIMER_C
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ParityBox = new System.Windows.Forms.ComboBox();
            this.StopBox = new System.Windows.Forms.ComboBox();
            this.DataBox = new System.Windows.Forms.ComboBox();
            this.BaudBox = new System.Windows.Forms.ComboBox();
            this.SerialportBox = new System.Windows.Forms.ComboBox();
            this.OpenSerial_button = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.OpenTimer = new System.Windows.Forms.Button();
            this.Power_button = new System.Windows.Forms.Button();
            this.serialPort = new System.IO.Ports.SerialPort(this.components);
            this.Oscillograph = new System.Windows.Forms.GroupBox();
            this.Waveform6 = new System.Windows.Forms.Button();
            this.Waveform5 = new System.Windows.Forms.Button();
            this.Waveform4 = new System.Windows.Forms.Button();
            this.Waveform3 = new System.Windows.Forms.Button();
            this.Waveform2 = new System.Windows.Forms.Button();
            this.Waveform1 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.Oscillograph.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ParityBox);
            this.groupBox1.Controls.Add(this.StopBox);
            this.groupBox1.Controls.Add(this.DataBox);
            this.groupBox1.Controls.Add(this.BaudBox);
            this.groupBox1.Controls.Add(this.SerialportBox);
            this.groupBox1.Controls.Add(this.OpenSerial_button);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(76, 53);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(384, 364);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DP700_DEMO";
            // 
            // ParityBox
            // 
            this.ParityBox.FormattingEnabled = true;
            this.ParityBox.Items.AddRange(new object[] {
            "None"});
            this.ParityBox.Location = new System.Drawing.Point(174, 270);
            this.ParityBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ParityBox.Name = "ParityBox";
            this.ParityBox.Size = new System.Drawing.Size(180, 26);
            this.ParityBox.TabIndex = 11;
            this.ParityBox.Text = "None";
            // 
            // StopBox
            // 
            this.StopBox.FormattingEnabled = true;
            this.StopBox.Items.AddRange(new object[] {
            "1"});
            this.StopBox.Location = new System.Drawing.Point(174, 223);
            this.StopBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.StopBox.Name = "StopBox";
            this.StopBox.Size = new System.Drawing.Size(180, 26);
            this.StopBox.TabIndex = 10;
            this.StopBox.Text = "1";
            // 
            // DataBox
            // 
            this.DataBox.FormattingEnabled = true;
            this.DataBox.Items.AddRange(new object[] {
            "8"});
            this.DataBox.Location = new System.Drawing.Point(174, 168);
            this.DataBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DataBox.Name = "DataBox";
            this.DataBox.Size = new System.Drawing.Size(180, 26);
            this.DataBox.TabIndex = 9;
            this.DataBox.Text = "8";
            // 
            // BaudBox
            // 
            this.BaudBox.FormattingEnabled = true;
            this.BaudBox.Items.AddRange(new object[] {
            "9600"});
            this.BaudBox.Location = new System.Drawing.Point(174, 115);
            this.BaudBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BaudBox.Name = "BaudBox";
            this.BaudBox.Size = new System.Drawing.Size(180, 26);
            this.BaudBox.TabIndex = 8;
            this.BaudBox.Text = "9600";
            // 
            // SerialportBox
            // 
            this.SerialportBox.FormattingEnabled = true;
            this.SerialportBox.Location = new System.Drawing.Point(174, 60);
            this.SerialportBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SerialportBox.Name = "SerialportBox";
            this.SerialportBox.Size = new System.Drawing.Size(180, 26);
            this.SerialportBox.TabIndex = 7;
            this.SerialportBox.Click += new System.EventHandler(this.comBox_MouseClick);
            // 
            // OpenSerial_button
            // 
            this.OpenSerial_button.Location = new System.Drawing.Point(127, 319);
            this.OpenSerial_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.OpenSerial_button.Name = "OpenSerial_button";
            this.OpenSerial_button.Size = new System.Drawing.Size(143, 35);
            this.OpenSerial_button.TabIndex = 5;
            this.OpenSerial_button.Text = "Open Serial";
            this.OpenSerial_button.UseVisualStyleBackColor = true;
            this.OpenSerial_button.Click += new System.EventHandler(this.OpenSerial_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(53, 265);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 18);
            this.label5.TabIndex = 4;
            this.label5.Text = "Parity";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(50, 223);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 18);
            this.label4.TabIndex = 3;
            this.label4.Text = "Stop Bits";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(46, 168);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "Data Bits";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 115);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "Baud Bits";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 65);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Serial Port";
            // 
            // OpenTimer
            // 
            this.OpenTimer.Location = new System.Drawing.Point(282, 467);
            this.OpenTimer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.OpenTimer.Name = "OpenTimer";
            this.OpenTimer.Size = new System.Drawing.Size(132, 35);
            this.OpenTimer.TabIndex = 2;
            this.OpenTimer.Text = "Open Timer";
            this.OpenTimer.UseVisualStyleBackColor = true;
            this.OpenTimer.Click += new System.EventHandler(this.OpenTimer_Click);
            // 
            // Power_button
            // 
            this.Power_button.Location = new System.Drawing.Point(76, 467);
            this.Power_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Power_button.Name = "Power_button";
            this.Power_button.Size = new System.Drawing.Size(143, 35);
            this.Power_button.TabIndex = 6;
            this.Power_button.Text = "Open Power";
            this.Power_button.UseVisualStyleBackColor = true;
            this.Power_button.Click += new System.EventHandler(this.power_MouseClick);
            // 
            // Oscillograph
            // 
            this.Oscillograph.Controls.Add(this.Waveform6);
            this.Oscillograph.Controls.Add(this.Waveform5);
            this.Oscillograph.Controls.Add(this.Waveform4);
            this.Oscillograph.Controls.Add(this.Waveform3);
            this.Oscillograph.Controls.Add(this.Waveform2);
            this.Oscillograph.Controls.Add(this.Waveform1);
            this.Oscillograph.Location = new System.Drawing.Point(488, 42);
            this.Oscillograph.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Oscillograph.Name = "Oscillograph";
            this.Oscillograph.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Oscillograph.Size = new System.Drawing.Size(166, 330);
            this.Oscillograph.TabIndex = 1;
            this.Oscillograph.TabStop = false;
            this.Oscillograph.Text = "Oscillograph";
            // 
            // Waveform6
            // 
            this.Waveform6.Location = new System.Drawing.Point(21, 276);
            this.Waveform6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Waveform6.Name = "Waveform6";
            this.Waveform6.Size = new System.Drawing.Size(117, 32);
            this.Waveform6.TabIndex = 5;
            this.Waveform6.Text = "Waveform6";
            this.Waveform6.UseVisualStyleBackColor = true;
            this.Waveform6.Click += new System.EventHandler(this.Waveform6_Click);
            // 
            // Waveform5
            // 
            this.Waveform5.Location = new System.Drawing.Point(21, 230);
            this.Waveform5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Waveform5.Name = "Waveform5";
            this.Waveform5.Size = new System.Drawing.Size(117, 32);
            this.Waveform5.TabIndex = 4;
            this.Waveform5.Text = "Waveform5";
            this.Waveform5.UseVisualStyleBackColor = true;
            this.Waveform5.Click += new System.EventHandler(this.Waveform5_Click);
            // 
            // Waveform4
            // 
            this.Waveform4.Location = new System.Drawing.Point(21, 188);
            this.Waveform4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Waveform4.Name = "Waveform4";
            this.Waveform4.Size = new System.Drawing.Size(117, 34);
            this.Waveform4.TabIndex = 3;
            this.Waveform4.Text = "Waveform4";
            this.Waveform4.UseVisualStyleBackColor = true;
            this.Waveform4.Click += new System.EventHandler(this.Waveform4_Click);
            // 
            // Waveform3
            // 
            this.Waveform3.Location = new System.Drawing.Point(21, 144);
            this.Waveform3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Waveform3.Name = "Waveform3";
            this.Waveform3.Size = new System.Drawing.Size(117, 31);
            this.Waveform3.TabIndex = 2;
            this.Waveform3.Text = "Waveform3";
            this.Waveform3.UseVisualStyleBackColor = true;
            this.Waveform3.Click += new System.EventHandler(this.Waveform3_Click);
            // 
            // Waveform2
            // 
            this.Waveform2.Location = new System.Drawing.Point(21, 96);
            this.Waveform2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Waveform2.Name = "Waveform2";
            this.Waveform2.Size = new System.Drawing.Size(117, 42);
            this.Waveform2.TabIndex = 1;
            this.Waveform2.Text = "Waveform2";
            this.Waveform2.UseVisualStyleBackColor = true;
            this.Waveform2.Click += new System.EventHandler(this.Waveform2_Click);
            // 
            // Waveform1
            // 
            this.Waveform1.Location = new System.Drawing.Point(21, 41);
            this.Waveform1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Waveform1.Name = "Waveform1";
            this.Waveform1.Size = new System.Drawing.Size(117, 34);
            this.Waveform1.TabIndex = 0;
            this.Waveform1.Text = "Waveform1";
            this.Waveform1.UseVisualStyleBackColor = true;
            this.Waveform1.Click += new System.EventHandler(this.Waveform1_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(501, 458);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(126, 50);
            this.button1.TabIndex = 2;
            this.button1.Text = "Timer ON/OFF";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Timer_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(718, 94);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(437, 414);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1211, 533);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.OpenTimer);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Oscillograph);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Power_button);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "serialport";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.Oscillograph.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ParityBox;
        private System.Windows.Forms.ComboBox StopBox;
        private System.Windows.Forms.ComboBox DataBox;
        private System.Windows.Forms.ComboBox BaudBox;
        private System.Windows.Forms.ComboBox SerialportBox;
        private System.Windows.Forms.Button Power_button;
        private System.Windows.Forms.Button OpenSerial_button;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.IO.Ports.SerialPort serialPort;
        private System.Windows.Forms.Button OpenTimer;
        private System.Windows.Forms.GroupBox Oscillograph;
        private System.Windows.Forms.Button Waveform6;
        private System.Windows.Forms.Button Waveform5;
        private System.Windows.Forms.Button Waveform4;
        private System.Windows.Forms.Button Waveform3;
        private System.Windows.Forms.Button Waveform2;
        private System.Windows.Forms.Button Waveform1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;

            }
}

