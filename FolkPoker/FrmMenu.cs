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

        }

        //窗体加载
        private void FrmMenu_Load(object sender, EventArgs e)
        {


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

            }
            else
            {
                frmPlay.menu = this;
                frmPlay.Show();
                this.Hide();
            }
        }



        //背景音乐

    }
}
