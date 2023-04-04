namespace FolkPoker
{
    partial class FrmMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMenu));
            this.panel2 = new System.Windows.Forms.Panel();
            this.panClose = new System.Windows.Forms.Panel();
            this.panLoginPlay = new System.Windows.Forms.Panel();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panLogin = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panStart = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel2.SuspendLayout();
            this.panLogin.SuspendLayout();
            this.panStart.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.panClose);
            this.panel2.Controls.Add(this.panLoginPlay);
            this.panel2.Controls.Add(this.txtPwd);
            this.panel2.Controls.Add(this.txtUser);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Location = new System.Drawing.Point(115, 67);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(236, 341);
            this.panel2.TabIndex = 10;
            this.panel2.Tag = "9999";
            this.panel2.Visible = false;
            // 
            // panClose
            // 
            this.panClose.BackColor = System.Drawing.Color.Transparent;
            this.panClose.Location = new System.Drawing.Point(132, 196);
            this.panClose.Name = "panClose";
            this.panClose.Size = new System.Drawing.Size(98, 39);
            this.panClose.TabIndex = 5;
            // 
            // panLoginPlay
            // 
            this.panLoginPlay.BackColor = System.Drawing.Color.Transparent;
            this.panLoginPlay.Location = new System.Drawing.Point(16, 196);
            this.panLoginPlay.Name = "panLoginPlay";
            this.panLoginPlay.Size = new System.Drawing.Size(98, 39);
            this.panLoginPlay.TabIndex = 4;
            this.panLoginPlay.Tag = "9999";
            // 
            // txtPwd
            // 
            this.txtPwd.Location = new System.Drawing.Point(66, 151);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.Size = new System.Drawing.Size(164, 20);
            this.txtPwd.TabIndex = 3;
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(66, 95);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(164, 20);
            this.txtUser.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft YaHei", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.ForeColor = System.Drawing.Color.Yellow;
            this.label6.Location = new System.Drawing.Point(11, 147);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 27);
            this.label6.TabIndex = 1;
            this.label6.Text = "密码:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.Yellow;
            this.label5.Location = new System.Drawing.Point(11, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 27);
            this.label5.TabIndex = 0;
            this.label5.Text = "账户:";
            // 
            // panLogin
            // 
            this.panLogin.BackColor = System.Drawing.Color.DimGray;
            this.panLogin.Controls.Add(this.label3);
            this.panLogin.Location = new System.Drawing.Point(0, 216);
            this.panLogin.Name = "panLogin";
            this.panLogin.Size = new System.Drawing.Size(401, 72);
            this.panLogin.TabIndex = 9;
            this.panLogin.Tag = "9999";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.Yellow;
            this.label3.Location = new System.Drawing.Point(133, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 29);
            this.label3.TabIndex = 2;
            this.label3.Tag = "9999";
            this.label3.Text = "Guit Game";
            this.label3.ForeColorChanged += new System.EventHandler(this.label1_ForeColorChanged);
            this.label3.Click += new System.EventHandler(this.label3_Click);
            this.label3.MouseLeave += new System.EventHandler(this.label1_MouseLeave);
            this.label3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label1_MouseMove);
            // 
            // panStart
            // 
            this.panStart.BackColor = System.Drawing.Color.DarkGreen;
            this.panStart.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panStart.Controls.Add(this.label1);
            this.panStart.Controls.Add(this.panel2);
            this.panStart.Location = new System.Drawing.Point(-9, 12);
            this.panStart.Name = "panStart";
            this.panStart.Size = new System.Drawing.Size(419, 70);
            this.panStart.TabIndex = 6;
            this.panStart.Tag = "9999";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Yellow;
            this.label1.Location = new System.Drawing.Point(140, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 29);
            this.label1.TabIndex = 0;
            this.label1.Tag = "9999";
            this.label1.Text = "Start Game";
            this.label1.ForeColorChanged += new System.EventHandler(this.label1_ForeColorChanged);
            this.label1.Click += new System.EventHandler(this.label1_Click);
            this.label1.MouseLeave += new System.EventHandler(this.label1_MouseLeave);
            this.label1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label1_MouseMove);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 900;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FrmMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Controls.Add(this.panLogin);
            this.Controls.Add(this.panStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(400, 300);
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "FrmMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hearts-Group3";
            this.Load += new System.EventHandler(this.FrmMenu_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panLogin.ResumeLayout(false);
            this.panLogin.PerformLayout();
            this.panStart.ResumeLayout(false);
            this.panStart.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panClose;
        private System.Windows.Forms.Panel panLoginPlay;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panLogin;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
    }
}