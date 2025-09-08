namespace MotorControl
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.bt_Link = new System.Windows.Forms.Button();
            this.cb_ComList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label1111 = new System.Windows.Forms.Label();
            this.lb_ID = new System.Windows.Forms.Label();
            this.bt_ReadInfo = new System.Windows.Forms.Button();
            this.lb_Pos = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lb_Velocity = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lb_Current = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.bt_PosOnce = new System.Windows.Forms.Button();
            this.bt_SpeedOnce = new System.Windows.Forms.Button();
            this.bt_InOnce = new System.Windows.Forms.Button();
            this.lb_LimitCurrent = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.bt_SetLimitCurrent = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.bt_SetPosCom = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.bt_Call = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.bt_SetVelocityCom = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.bt_SetCurrentCom = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.bt_OpenDocunment = new System.Windows.Forms.Button();
            this.numID = new System.Windows.Forms.NumericUpDown();
            this.numLimitCurrent = new System.Windows.Forms.NumericUpDown();
            this.num_Velocity = new System.Windows.Forms.NumericUpDown();
            this.num_Current = new System.Windows.Forms.NumericUpDown();
            this.bt_Enable = new System.Windows.Forms.Button();
            this.bt_StopNow = new System.Windows.Forms.Button();
            this.num_Position = new System.Windows.Forms.NumericUpDown();
            this.list_Postions = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.bt_RecordPos = new System.Windows.Forms.Button();
            this.bt_LoopPos = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLimitCurrent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Velocity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Current)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Position)).BeginInit();
            this.SuspendLayout();
            // 
            // bt_Link
            // 
            this.bt_Link.Location = new System.Drawing.Point(150, 19);
            this.bt_Link.Margin = new System.Windows.Forms.Padding(2);
            this.bt_Link.Name = "bt_Link";
            this.bt_Link.Size = new System.Drawing.Size(81, 24);
            this.bt_Link.TabIndex = 0;
            this.bt_Link.Text = "连接";
            this.bt_Link.UseVisualStyleBackColor = true;
            this.bt_Link.Click += new System.EventHandler(this.bt_Link_Click);
            // 
            // cb_ComList
            // 
            this.cb_ComList.FormattingEnabled = true;
            this.cb_ComList.Location = new System.Drawing.Point(55, 22);
            this.cb_ComList.Margin = new System.Windows.Forms.Padding(2);
            this.cb_ComList.Name = "cb_ComList";
            this.cb_ComList.Size = new System.Drawing.Size(92, 20);
            this.cb_ComList.TabIndex = 1;
            this.cb_ComList.DropDown += new System.EventHandler(this.cb_Com_DropDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "COM:";
            // 
            // label1111
            // 
            this.label1111.AutoSize = true;
            this.label1111.Location = new System.Drawing.Point(21, 88);
            this.label1111.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1111.Name = "label1111";
            this.label1111.Size = new System.Drawing.Size(53, 12);
            this.label1111.TabIndex = 3;
            this.label1111.Text = "电机ID：";
            // 
            // lb_ID
            // 
            this.lb_ID.AutoSize = true;
            this.lb_ID.Location = new System.Drawing.Point(142, 88);
            this.lb_ID.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_ID.MinimumSize = new System.Drawing.Size(112, 0);
            this.lb_ID.Name = "lb_ID";
            this.lb_ID.Size = new System.Drawing.Size(112, 12);
            this.lb_ID.TabIndex = 4;
            this.lb_ID.Text = "（空）";
            this.lb_ID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // bt_ReadInfo
            // 
            this.bt_ReadInfo.Location = new System.Drawing.Point(23, 56);
            this.bt_ReadInfo.Margin = new System.Windows.Forms.Padding(2);
            this.bt_ReadInfo.Name = "bt_ReadInfo";
            this.bt_ReadInfo.Size = new System.Drawing.Size(83, 24);
            this.bt_ReadInfo.TabIndex = 5;
            this.bt_ReadInfo.Text = "读取信息";
            this.bt_ReadInfo.UseVisualStyleBackColor = true;
            this.bt_ReadInfo.Click += new System.EventHandler(this.bt_ReadInfo_Click);
            // 
            // lb_Pos
            // 
            this.lb_Pos.AutoSize = true;
            this.lb_Pos.Location = new System.Drawing.Point(142, 110);
            this.lb_Pos.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_Pos.MinimumSize = new System.Drawing.Size(112, 0);
            this.lb_Pos.Name = "lb_Pos";
            this.lb_Pos.Size = new System.Drawing.Size(112, 12);
            this.lb_Pos.TabIndex = 7;
            this.lb_Pos.Text = "（空）";
            this.lb_Pos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 110);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "电机位置（rad）：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(446, 15);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "电机ID：";
            // 
            // lb_Velocity
            // 
            this.lb_Velocity.AutoSize = true;
            this.lb_Velocity.Location = new System.Drawing.Point(142, 132);
            this.lb_Velocity.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_Velocity.MinimumSize = new System.Drawing.Size(112, 0);
            this.lb_Velocity.Name = "lb_Velocity";
            this.lb_Velocity.Size = new System.Drawing.Size(112, 12);
            this.lb_Velocity.TabIndex = 11;
            this.lb_Velocity.Text = "（空）";
            this.lb_Velocity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 132);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "电机速度(rad/s)：";
            // 
            // lb_Current
            // 
            this.lb_Current.AutoSize = true;
            this.lb_Current.Location = new System.Drawing.Point(142, 156);
            this.lb_Current.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_Current.MinimumSize = new System.Drawing.Size(112, 0);
            this.lb_Current.Name = "lb_Current";
            this.lb_Current.Size = new System.Drawing.Size(112, 12);
            this.lb_Current.TabIndex = 13;
            this.lb_Current.Text = "（空）";
            this.lb_Current.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 156);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "电机电流(mA)：";
            // 
            // bt_PosOnce
            // 
            this.bt_PosOnce.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_PosOnce.Location = new System.Drawing.Point(446, 94);
            this.bt_PosOnce.Margin = new System.Windows.Forms.Padding(2);
            this.bt_PosOnce.Name = "bt_PosOnce";
            this.bt_PosOnce.Size = new System.Drawing.Size(134, 24);
            this.bt_PosOnce.TabIndex = 14;
            this.bt_PosOnce.Text = "转到第一圈";
            this.bt_PosOnce.UseVisualStyleBackColor = true;
            this.bt_PosOnce.Click += new System.EventHandler(this.bt_PosOnce_Click);
            // 
            // bt_SpeedOnce
            // 
            this.bt_SpeedOnce.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_SpeedOnce.Location = new System.Drawing.Point(446, 122);
            this.bt_SpeedOnce.Margin = new System.Windows.Forms.Padding(2);
            this.bt_SpeedOnce.Name = "bt_SpeedOnce";
            this.bt_SpeedOnce.Size = new System.Drawing.Size(134, 24);
            this.bt_SpeedOnce.TabIndex = 15;
            this.bt_SpeedOnce.Text = "以1rad/s的速度旋转";
            this.bt_SpeedOnce.UseVisualStyleBackColor = true;
            this.bt_SpeedOnce.Click += new System.EventHandler(this.bt_SpeedOnce_Click);
            // 
            // bt_InOnce
            // 
            this.bt_InOnce.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_InOnce.Location = new System.Drawing.Point(446, 150);
            this.bt_InOnce.Margin = new System.Windows.Forms.Padding(2);
            this.bt_InOnce.Name = "bt_InOnce";
            this.bt_InOnce.Size = new System.Drawing.Size(134, 24);
            this.bt_InOnce.TabIndex = 16;
            this.bt_InOnce.Text = "以1A的电流旋转";
            this.bt_InOnce.UseVisualStyleBackColor = true;
            this.bt_InOnce.Click += new System.EventHandler(this.bt_InOnce_Click);
            // 
            // lb_LimitCurrent
            // 
            this.lb_LimitCurrent.AutoSize = true;
            this.lb_LimitCurrent.Location = new System.Drawing.Point(142, 178);
            this.lb_LimitCurrent.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lb_LimitCurrent.MinimumSize = new System.Drawing.Size(112, 0);
            this.lb_LimitCurrent.Name = "lb_LimitCurrent";
            this.lb_LimitCurrent.Size = new System.Drawing.Size(112, 12);
            this.lb_LimitCurrent.TabIndex = 18;
            this.lb_LimitCurrent.Text = "（空）";
            this.lb_LimitCurrent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 178);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(113, 12);
            this.label9.TabIndex = 17;
            this.label9.Text = "电机堵转电流(mA)：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 226);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(113, 12);
            this.label10.TabIndex = 19;
            this.label10.Text = "设置堵转电流(mA)：";
            // 
            // bt_SetLimitCurrent
            // 
            this.bt_SetLimitCurrent.Location = new System.Drawing.Point(191, 221);
            this.bt_SetLimitCurrent.Margin = new System.Windows.Forms.Padding(2);
            this.bt_SetLimitCurrent.Name = "bt_SetLimitCurrent";
            this.bt_SetLimitCurrent.Size = new System.Drawing.Size(75, 24);
            this.bt_SetLimitCurrent.TabIndex = 21;
            this.bt_SetLimitCurrent.Text = "设置";
            this.bt_SetLimitCurrent.UseVisualStyleBackColor = true;
            this.bt_SetLimitCurrent.Click += new System.EventHandler(this.bt_SetLimitCurrent_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(456, 244);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(17, 12);
            this.label13.TabIndex = 29;
            this.label13.Text = "圈";
            // 
            // bt_SetPosCom
            // 
            this.bt_SetPosCom.Location = new System.Drawing.Point(477, 239);
            this.bt_SetPosCom.Margin = new System.Windows.Forms.Padding(2);
            this.bt_SetPosCom.Name = "bt_SetPosCom";
            this.bt_SetPosCom.Size = new System.Drawing.Size(102, 24);
            this.bt_SetPosCom.TabIndex = 28;
            this.bt_SetPosCom.Text = "发送命令";
            this.bt_SetPosCom.UseVisualStyleBackColor = true;
            this.bt_SetPosCom.Click += new System.EventHandler(this.bt_SetPosCom_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(350, 243);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label14.Size = new System.Drawing.Size(41, 12);
            this.label14.TabIndex = 26;
            this.label14.Text = "转到第";
            // 
            // bt_Call
            // 
            this.bt_Call.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Call.Location = new System.Drawing.Point(446, 66);
            this.bt_Call.Margin = new System.Windows.Forms.Padding(2);
            this.bt_Call.Name = "bt_Call";
            this.bt_Call.Size = new System.Drawing.Size(134, 24);
            this.bt_Call.TabIndex = 30;
            this.bt_Call.Text = "电机校准";
            this.bt_Call.UseVisualStyleBackColor = true;
            this.bt_Call.Click += new System.EventHandler(this.bt_Call_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(392, 269);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(83, 12);
            this.label15.TabIndex = 34;
            this.label15.Text = "rad/s速度运行";
            // 
            // bt_SetVelocityCom
            // 
            this.bt_SetVelocityCom.Location = new System.Drawing.Point(477, 264);
            this.bt_SetVelocityCom.Margin = new System.Windows.Forms.Padding(2);
            this.bt_SetVelocityCom.Name = "bt_SetVelocityCom";
            this.bt_SetVelocityCom.Size = new System.Drawing.Size(102, 24);
            this.bt_SetVelocityCom.TabIndex = 33;
            this.bt_SetVelocityCom.Text = "发送命令";
            this.bt_SetVelocityCom.UseVisualStyleBackColor = true;
            this.bt_SetVelocityCom.Click += new System.EventHandler(this.bt_SetVelocityCom_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(315, 269);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(17, 12);
            this.label16.TabIndex = 31;
            this.label16.Text = "以";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(410, 294);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(65, 12);
            this.label17.TabIndex = 38;
            this.label17.Text = "mA电流运行";
            // 
            // bt_SetCurrentCom
            // 
            this.bt_SetCurrentCom.Location = new System.Drawing.Point(477, 289);
            this.bt_SetCurrentCom.Margin = new System.Windows.Forms.Padding(2);
            this.bt_SetCurrentCom.Name = "bt_SetCurrentCom";
            this.bt_SetCurrentCom.Size = new System.Drawing.Size(102, 24);
            this.bt_SetCurrentCom.TabIndex = 37;
            this.bt_SetCurrentCom.Text = "发送命令";
            this.bt_SetCurrentCom.UseVisualStyleBackColor = true;
            this.bt_SetCurrentCom.Click += new System.EventHandler(this.bt_SetCurrentCom_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(333, 294);
            this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(17, 12);
            this.label18.TabIndex = 35;
            this.label18.Text = "以";
            // 
            // bt_OpenDocunment
            // 
            this.bt_OpenDocunment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_OpenDocunment.Location = new System.Drawing.Point(446, 323);
            this.bt_OpenDocunment.Margin = new System.Windows.Forms.Padding(2);
            this.bt_OpenDocunment.Name = "bt_OpenDocunment";
            this.bt_OpenDocunment.Size = new System.Drawing.Size(134, 24);
            this.bt_OpenDocunment.TabIndex = 39;
            this.bt_OpenDocunment.Text = "打开电机说明文档";
            this.bt_OpenDocunment.UseVisualStyleBackColor = true;
            this.bt_OpenDocunment.Click += new System.EventHandler(this.bt_OpenDocunment_Click);
            // 
            // numID
            // 
            this.numID.Location = new System.Drawing.Point(511, 12);
            this.numID.Margin = new System.Windows.Forms.Padding(2);
            this.numID.Name = "numID";
            this.numID.Size = new System.Drawing.Size(68, 21);
            this.numID.TabIndex = 40;
            // 
            // numLimitCurrent
            // 
            this.numLimitCurrent.Location = new System.Drawing.Point(118, 222);
            this.numLimitCurrent.Margin = new System.Windows.Forms.Padding(2);
            this.numLimitCurrent.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numLimitCurrent.Name = "numLimitCurrent";
            this.numLimitCurrent.Size = new System.Drawing.Size(69, 21);
            this.numLimitCurrent.TabIndex = 41;
            // 
            // num_Velocity
            // 
            this.num_Velocity.DecimalPlaces = 2;
            this.num_Velocity.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.num_Velocity.Location = new System.Drawing.Point(335, 264);
            this.num_Velocity.Margin = new System.Windows.Forms.Padding(2);
            this.num_Velocity.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.num_Velocity.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.num_Velocity.Name = "num_Velocity";
            this.num_Velocity.Size = new System.Drawing.Size(56, 21);
            this.num_Velocity.TabIndex = 44;
            // 
            // num_Current
            // 
            this.num_Current.Location = new System.Drawing.Point(350, 289);
            this.num_Current.Margin = new System.Windows.Forms.Padding(2);
            this.num_Current.Name = "num_Current";
            this.num_Current.Size = new System.Drawing.Size(56, 21);
            this.num_Current.TabIndex = 45;
            // 
            // bt_Enable
            // 
            this.bt_Enable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Enable.Location = new System.Drawing.Point(446, 37);
            this.bt_Enable.Margin = new System.Windows.Forms.Padding(2);
            this.bt_Enable.Name = "bt_Enable";
            this.bt_Enable.Size = new System.Drawing.Size(133, 24);
            this.bt_Enable.TabIndex = 46;
            this.bt_Enable.Text = "使能电机";
            this.bt_Enable.UseVisualStyleBackColor = true;
            this.bt_Enable.Click += new System.EventHandler(this.bt_Enable_Click);
            // 
            // bt_StopNow
            // 
            this.bt_StopNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_StopNow.Location = new System.Drawing.Point(446, 178);
            this.bt_StopNow.Margin = new System.Windows.Forms.Padding(2);
            this.bt_StopNow.Name = "bt_StopNow";
            this.bt_StopNow.Size = new System.Drawing.Size(134, 24);
            this.bt_StopNow.TabIndex = 48;
            this.bt_StopNow.Text = "立即停止";
            this.bt_StopNow.UseVisualStyleBackColor = true;
            this.bt_StopNow.Click += new System.EventHandler(this.bt_StopNow_Click);
            // 
            // num_Position
            // 
            this.num_Position.DecimalPlaces = 2;
            this.num_Position.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.num_Position.Location = new System.Drawing.Point(393, 239);
            this.num_Position.Margin = new System.Windows.Forms.Padding(2);
            this.num_Position.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.num_Position.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.num_Position.Name = "num_Position";
            this.num_Position.Size = new System.Drawing.Size(56, 21);
            this.num_Position.TabIndex = 49;
            // 
            // list_Postions
            // 
            this.list_Postions.FormattingEnabled = true;
            this.list_Postions.ItemHeight = 12;
            this.list_Postions.Location = new System.Drawing.Point(273, 31);
            this.list_Postions.Margin = new System.Windows.Forms.Padding(2);
            this.list_Postions.Name = "list_Postions";
            this.list_Postions.Size = new System.Drawing.Size(133, 112);
            this.list_Postions.TabIndex = 50;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(273, 14);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 51;
            this.label4.Text = "位置列表:";
            // 
            // bt_RecordPos
            // 
            this.bt_RecordPos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_RecordPos.Location = new System.Drawing.Point(273, 150);
            this.bt_RecordPos.Margin = new System.Windows.Forms.Padding(2);
            this.bt_RecordPos.Name = "bt_RecordPos";
            this.bt_RecordPos.Size = new System.Drawing.Size(133, 24);
            this.bt_RecordPos.TabIndex = 52;
            this.bt_RecordPos.Text = "记录当前位置";
            this.bt_RecordPos.UseVisualStyleBackColor = true;
            this.bt_RecordPos.Click += new System.EventHandler(this.bt_RecordPos_Click);
            // 
            // bt_LoopPos
            // 
            this.bt_LoopPos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_LoopPos.Location = new System.Drawing.Point(273, 178);
            this.bt_LoopPos.Margin = new System.Windows.Forms.Padding(2);
            this.bt_LoopPos.Name = "bt_LoopPos";
            this.bt_LoopPos.Size = new System.Drawing.Size(133, 24);
            this.bt_LoopPos.TabIndex = 53;
            this.bt_LoopPos.Text = "循环运动";
            this.bt_LoopPos.UseVisualStyleBackColor = true;
            this.bt_LoopPos.Click += new System.EventHandler(this.bt_LoopPos_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 360);
            this.Controls.Add(this.bt_LoopPos);
            this.Controls.Add(this.bt_RecordPos);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.list_Postions);
            this.Controls.Add(this.num_Position);
            this.Controls.Add(this.bt_StopNow);
            this.Controls.Add(this.bt_Enable);
            this.Controls.Add(this.num_Current);
            this.Controls.Add(this.num_Velocity);
            this.Controls.Add(this.numLimitCurrent);
            this.Controls.Add(this.numID);
            this.Controls.Add(this.bt_OpenDocunment);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.bt_SetCurrentCom);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.bt_SetVelocityCom);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.bt_Call);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.bt_SetPosCom);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.bt_SetLimitCurrent);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.lb_LimitCurrent);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.bt_InOnce);
            this.Controls.Add(this.bt_SpeedOnce);
            this.Controls.Add(this.bt_PosOnce);
            this.Controls.Add(this.lb_Current);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lb_Velocity);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lb_Pos);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.bt_ReadInfo);
            this.Controls.Add(this.lb_ID);
            this.Controls.Add(this.label1111);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cb_ComList);
            this.Controls.Add(this.bt_Link);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Jyker电机上位机";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLimitCurrent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Velocity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Current)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Position)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_Link;
        private System.Windows.Forms.ComboBox cb_ComList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label1111;
        private System.Windows.Forms.Label lb_ID;
        private System.Windows.Forms.Button bt_ReadInfo;
        private System.Windows.Forms.Label lb_Pos;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lb_Velocity;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lb_Current;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button bt_PosOnce;
        private System.Windows.Forms.Button bt_SpeedOnce;
        private System.Windows.Forms.Button bt_InOnce;
        private System.Windows.Forms.Label lb_LimitCurrent;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button bt_SetLimitCurrent;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button bt_SetPosCom;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button bt_Call;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button bt_SetVelocityCom;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button bt_SetCurrentCom;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button bt_OpenDocunment;
        private System.Windows.Forms.NumericUpDown numID;
        private System.Windows.Forms.NumericUpDown numLimitCurrent;
        private System.Windows.Forms.NumericUpDown num_Velocity;
        private System.Windows.Forms.NumericUpDown num_Current;
        private System.Windows.Forms.Button bt_Enable;
        private System.Windows.Forms.Button bt_StopNow;
        private System.Windows.Forms.NumericUpDown num_Position;
        private System.Windows.Forms.ListBox list_Postions;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button bt_RecordPos;
        private System.Windows.Forms.Button bt_LoopPos;
    }
}

