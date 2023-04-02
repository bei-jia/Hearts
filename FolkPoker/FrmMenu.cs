using FolkPokerBLL;
using FolkPokerModes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FolkPoker
{
    public partial class FrmMenu : Form
    {

        //实例化加载与保存类
        LoadAndSaveManager las = new LoadAndSaveManager();
        //游戏窗体
        FrmPlay frmPlay = new FrmPlay();
        

        public FrmMenu()
        {
            InitializeComponent();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true); // 双缓冲
            this.UpdateStyles();
        }

        //计时器
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (panel1.Visible == true)
            {
                panel1.Visible = false;
            }
            else
            {
                panel1.Visible = true;
            }
        }

        //窗体加载
        private void FrmMenu_Load(object sender, EventArgs e)
        {
            this.panel5.Visible = false;
            this.panel3.Visible = false;
            Util.MusicPlay("back", Util.isMusic);//播放背景音乐
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            ((Label)sender).ForeColor = Color.Black;
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            ((Label)sender).ForeColor = Color.Yellow;
        }

        //音效
        private void label1_ForeColorChanged(object sender, EventArgs e)
        {
            if (((Label)sender).ForeColor == Color.Black)
            {
                Util.VoicePlay("button", Util.isVoice);
            }
        }

        //游客模式
       
        private void label2_Click(object sender, EventArgs e)
        {
            //设置为游客模式
            Util.isPlay = true;
            frmPlay.menu = this;
            frmPlay.Show();
            this.Hide();
        }

        //游戏设置
        private void label4_Click(object sender, EventArgs e)
        {
            this.panel3.Visible = true;
        }

        //退出游戏
        private void label3_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("确定要退出游戏吗?","温馨提示",MessageBoxButtons.OKCancel);
            if (DialogResult.OK == res)
            {
                System.Environment.Exit(0);
                //Application.Exit();

            }
        }

        //开始游戏
        private void label1_Click(object sender, EventArgs e)
        {
            //设置为竞技模式
            Util.isPlay = false;
            if (!File.Exists("Info.xml"))
            {
                this.panel5.Visible = true;
            }
            else
            {
                frmPlay.menu = this;
                frmPlay.Show();
                this.Hide();
            }
        }

        //隐藏游戏设置
        private void panel4_Click(object sender, EventArgs e)
        {
            this.panel3.Visible = false;
        }

        //音效
        private void label17_Click(object sender, EventArgs e)
        {
            if (this.label17.Text.Equals(" 音  效 :ON"))
            {
                this.label17.Text = " 音  效 :OFF";
                Util.isVoice = false;
            }
            else
            {
                this.label17.Text = " 音  效 :ON";
                Util.isVoice = true;
            }
        }

        //背景音乐
        private void label16_Click(object sender, EventArgs e)
        {
            if (this.label16.Text.Equals("背景音乐:ON"))
            {
                this.label16.Text = "背景音乐:OFF";
                Util.play.Ctlcontrols.pause();//音乐关闭
                Util.isMusic = false;

            }
            else
            {
                this.label16.Text = "背景音乐:ON";
                Util.play.Ctlcontrols.play();//音乐打开
                Util.isMusic = true;

            }
        }

        //注册取消按钮
        private void label10_Click(object sender, EventArgs e)
        {
            this.panel5.Visible = false;
        }

        //单击注册按钮
        private void label9_Click(object sender, EventArgs e)
        {
            if (this.txtAccount.Text.Trim().Length != 0 && this.txtPwds.Text.Trim().Length != 0)
            {
                Util.VoicePlay("button", Util.isVoice);

                string name = this.txtAccount.Text;//账号名
                string pwd = this.txtPwds.Text;//密码
                User user = new User(name, pwd);//初始化,调用构造函数
                las.CheckFile();// 检查设置 Info.xml文件是否存在，不存在创建
                las.SaveUserInfo(user);
                this.panel5.Visible = false;
                //this.panel1.Visible = false;
                DialogResult ds = MessageBox.Show("注册成功","提示",MessageBoxButtons.OK);
                if (DialogResult.OK == ds)
                {
                    frmPlay.menu = this;
                    frmPlay.Show();
                    this.Hide();
                }
            }
            else
            {
                MessageBox.Show("信息不完整");
            }
        }


    }
}
