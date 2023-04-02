using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FolkPokerModes;
using System.Media;
using System.Collections;
using FolkPokerBLL;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;

public delegate void Myweituo2(bool bl);

namespace FolkPoker
{

    public partial class FrmPlay : Form
    {
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]

        public static extern bool FlashWindow(IntPtr handle, bool bInvert);
        [DllImport("user32.dll")]
        public static extern IntPtr LoadCursorFromFile(string fileName);

        [DllImport("user32.dll")]
        public static extern IntPtr SetCursor(IntPtr cursorHandle);

        [DllImport("user32.dll")]
        public static extern uint DestroyCursor(IntPtr cursorHandle);





        public FrmPlay()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;//为false可以跨线程调用windows控件
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true); // 双缓冲
            this.UpdateStyles();
        }
        #region 属性

        private PictureBox[] paiImage;//一副牌的图形
        private Start start = new Start();//开局
        private Poker[] poker;//一副牌
        private Role role1; //角色1
        private Role role2;//角色2
        private Role role3;//角色3
        private PlayCard playcard;//出牌
        private ReceiveCard receive;//接拍
        private Thread th_daPai;//打牌线程
        private Thread th_faPai;//发牌线程
        private ComputerPlay This_PlayCard;//电脑出牌
        private bool bl_isDiZhu = false;//判断当前玩家是否当地主
        private bool bl_isFirst = false;//判断当前玩家是否出第一手牌
        private bool bl_chuPaiOver = false;//任何一方牌出完了，则结束本局
        private bool bl_tuoGuan = false;//判断当前玩家是否托管
        private bool bl_isRightTime = false;//判断是否为允许托管的时间段
        private bool bl_isRightTime2 = false;//判断是否为允许排序的时间段
        private bool bl_paiXu = false;//判断当前排序的状态
        private ArrayList saveList;//保存当前玩家的当前出牌
        private int buChuPai = 0;//判断有几方不出牌
        private int chuPaiWeiZhi = 109;//判断当前角色出牌的张数，然后决定出牌的位置
        private string lblTiShi = "";//如果出了炸弹，则提示
        private int tishi = 0;//提示出牌,如提示的牌不妥，则++选择后面的
        private int player1 = 0;//角色1得分
        private int player2 = 0;//角色2得分
        private int player3 = 0;//角色3得分
        private FrmGameOver gameOver;//游戏结果显示窗体
        private Myweituo2 weituo2;//玩家叫地主按钮的委托
        private Myweituo2 weituo3;//玩家出牌按钮显示及隐藏的委托
        private Myweituo2 weituo4;//玩家出牌，不出，提示按钮显示及隐藏的委托
        SoundPlayer sound = new SoundPlayer(); //通过文件播放 
        private Thread th_Master; //置地主图片线程
        LoadAndSaveManager las = new LoadAndSaveManager();
        PictureBox picMaster = new PictureBox(); //地主头像
        PictureBox picMasterCar = new PictureBox(); //地主牌
        PictureBox picMasterCar2 = new PictureBox(); //地主牌
        PictureBox picMasterCar3 = new PictureBox(); //地主牌

        #endregion

        //用于接主菜单
        public FrmMenu menu;

        private void FrmMain_Load(object sender, EventArgs e)
        {
            
            InitializeComponent();
            this.BackgroundImage = Image.FromFile(@"Images/BackgroundImages.jpg");  //设置背景图片
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;  //优化界面流畅度         
            Util.MusicPlay("normal", Util.isMusic);//播放背景音乐
            picMaster.Image = Image.FromFile(@"Images/Master.jpg");
            picMaster.SizeMode = PictureBoxSizeMode.StretchImage;
            picMaster.SetBounds(1500, 1500, 62, 68);
            this.Controls.Add(picMaster);

            picMasterCar.SizeMode = PictureBoxSizeMode.StretchImage;
            picMasterCar.SetBounds(1000, 30, 84, 119);
            this.Controls.Add(picMasterCar);


            picMasterCar2.SizeMode = PictureBoxSizeMode.StretchImage;
            picMasterCar2.SetBounds(1104, 30, 84, 119);
            this.Controls.Add(picMasterCar2);

            picMasterCar3.SizeMode = PictureBoxSizeMode.StretchImage;
            picMasterCar3.SetBounds(1208, 30, 84, 119);
            this.Controls.Add(picMasterCar3);


            //设置用户信息
            //判断用户选用是否是游客模式
            if (Util.isPlay)
            {
                this.lbl_DownJiFen.Text = "";
            }
            else
            {
                //读取用户信息
                User user = las.LoadUserInfo();
            }
            readyisture();
            tuoGuanisture();
        }

        public void readyisture()
        {
            this.btn_strat.Visible = true;
        }
        public void readyisfalse()
        {
            this.btn_strat.Visible = false;
        }
        //托管按钮显示
        public void tuoGuanisture()
        {


        }
        //托管按钮隐藏



        #region 开局（初始化）
        #region 主加载方法
        private void load()
        {
            start.GameNum++; //设置局数
            this.lbl_jushu.Text = "第" + start.GameNum + "局";//设置局数
            this.lbl_beishu.Text = "倍数:   ×1"; //设置倍数
            this.lbl_difen.Text = "底分:   ×1"; //设置低分
            //this.lbl_leftJiFen.Text = player3 + ""; //设置电脑积分
            //this.lbl_rightJiFen.Text = player1 + ""; //设置电脑积分
            this.lbl_DownJiFen.Text = player2 + ""; //设置玩家积分
            weituo2 = new Myweituo2(btnJiaoFen); //玩家叫地主按钮的委托
            weituo3 = new Myweituo2(btnChuPai_3);//玩家出牌按钮显示及隐藏的委托
            weituo4 = new Myweituo2(btnChuPai_1);//玩家出牌，不出，提示按钮显示及隐藏的委托
            poker = new Poker[54]; //设置54张牌数组
            paiImage = new PictureBox[54]; //牌pic数组
            playcard = new PlayCard();  //出牌类
            receive = new ReceiveCard(); //接牌类
            This_PlayCard = new ComputerPlay(); //电脑出牌类
            saveList = new ArrayList();//保存当前玩家的当前出牌
            //th_Master = new Thread(new ThreadStart(getMaster));//线程地主标志
            th_daPai = new Thread(new ThreadStart(daPai)); //线程打牌
            th_faPai = new Thread(new ThreadStart(fapai)); //线程发牌
            newPlayer("电脑2", "玩家", "电脑1"); //设置玩家
            newPai(); //New出54张牌
            suiji1(); //获得54个随机数
            newPaiImage(); //new出牌的图形
            //排序玩家与电脑的牌
            pai_paixu(role1);
            pai_paixu(role2);
            pai_paixu(role3);
            //保存玩家与电脑的牌
            addJuesePai(role1);
            addJuesePai(role2);
            addJuesePai(role3);
        }
        #endregion
        #region NEW出3个角色
        private void newPlayer(string name1, string name2, string name3)
        {
            //new3个角色
            Player player1 = new Player(name1);
            Player player2 = new Player(name2);
            Player player3 = new Player(name3);
            role1 = new Role(player1);
            role2 = new Role(player2);
            role3 = new Role(player3);
            //设置角色位置
            role1.Location = 1;
            role2.Location = 2;
            role3.Location = 3;
        }
        #endregion
        #region NEW出54张牌
        private void newPai()
        {
            Poker[,] pai1 = new Poker[4, 13];
            Poker.BackImage = Image.FromFile(@"Images/back.png"); //设置牌背面图
            Poker dawang = new Poker(1, 17, Image.FromFile(@"Images/1.png")); //大王
            Poker xiaowang = new Poker(1, 16, Image.FromFile(@"Images/2.png")); //小王
            poker[0] = dawang; poker[1] = xiaowang; //加入牌
            string[] imageName = new string[52]; //除大王。小王的其余52张牌
            for (int i = 0; i < imageName.Length; i++)
            {
                imageName[i] = (i + 3).ToString(); //得到牌的名字

            }
            int k = 0;
            int c = 0;
            for (int i = 0; i < 4; i++)
            {
                int j = 0;
                for (j = 0; j < 13; j++)
                {
                    pai1[i, j] = new Poker((i + 1), (j + 3), Image.FromFile(@"Images/" + imageName[c] + ".png")); //获得52张牌的图片
                    poker[k + j + 2] = pai1[i, j]; //加入牌
                    c++;
                }
                k = k + j;
            }
        }
        #endregion
        #region 获取54个不同的随机数
        private void suiji1()
        {
            Random rd = new Random();
            for (int i = 0; i < poker.Length; i++)
            {
                int k = rd.Next(54); //得到随机数
                if (i == 0)
                {
                    poker[i].Index = k; //poker类index得到随机数
                }
                for (int j = 0; j < i; j++)
                {
                    if (poker[j].Index == k)
                    {
                        i--;
                        break;
                    }
                    else if (j == i - 1)
                    {
                        poker[i].Index = k;
                    }
                }
            }
        }
        #endregion
        #region NEW出牌的图形
        private void newPaiImage()
        {
            int a = 500;
            for (int i = 0; i < paiImage.Length; i++)
            {
                paiImage[poker[i].Index] = new PictureBox();
                paiImage[poker[i].Index].SetBounds(a, 170, 65, 100); //设置位置与Pic牌大小
                paiImage[poker[i].Index].BackgroundImage = Poker.BackImage; //得到背面图
                paiImage[poker[i].Index].BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch; //优化界面流畅度
                this.Controls.Add(this.paiImage[poker[i].Index]);//加入界面
                if (i < 51)
                {
                    if (i % 3 == 0) role1.Remain_Subscript1.Add(poker[i].Index); //加入剩余牌
                    else if ((i + 2) % 3 == 0)
                    {
                        role2.Remain_Subscript1.Add(poker[i].Index);
                        paiImage[poker[i].Index].Click += new System.EventHandler(paiImage_Click); //设置点击事件
                    }
                    else role3.Remain_Subscript1.Add(poker[i].Index); //电脑加入剩余牌
                }
                a -= 5;
            }
        }
        #endregion
        #region 排序--牌
        private void pai_paixu(Role role)
        {
            for (int i = 0; i < role.Remain_Subscript1.Count; i++)
            {
                for (int j = i; j < role.Remain_Subscript1.Count; j++)
                {
                    if (poker[(int)role.Remain_Subscript1[i]].Size < poker[(int)role.Remain_Subscript1[j]].Size)
                    {
                        int temp = (int)role.Remain_Subscript1[i]; //得到剩余牌
                        role.Remain_Subscript1[i] = role.Remain_Subscript1[j];
                        role.Remain_Subscript1[j] = temp;
                    }
                }
            }
        }
        #endregion
        #region 排序--牌(按照4张,3张,2张,1张的顺序)
        private void pai_paixu2(Role ro)
        {
            ArrayList list = receive.basic(2, ro.Remain_Poker);  //搜索大于上手牌，返回arraylist
            //排序
            int[] a = receive.arrayToArgs((ArrayList)list[0]);
            int[] b = receive.arrayToArgs((ArrayList)list[1]);
            int[] c = receive.arrayToArgs((ArrayList)list[2]);
            int[] d = receive.arrayToArgs((ArrayList)list[3]);
            list.Clear(); //清除数组
            #region 将返回的牌按顺序添加到集合
            if (d != null)
            {
                for (int i = 0; i < d.Length; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        list.Add(d[i]);
                    }
                }
            }
            if (c != null)
            {
                for (int i = 0; i < c.Length; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        list.Add(c[i]);
                    }
                }
            }
            if (b != null)
            {
                for (int i = 0; i < b.Length; i++)
                {
                    list.Add(b[i]); list.Add(b[i]);
                }
            }
            if (a != null)
            {
                for (int i = 0; i < a.Length; i++)
                {
                    list.Add(a[i]);
                }
            }
            #endregion
            for (int k = 0; k < list.Count; k++)
            {
                for (int m = k; m < ro.Remain_Subscript.Count; m++)
                {
                    if ((int)list[k] == poker[(int)ro.Remain_Subscript[m]].Size)
                    {
                        int temp = (int)ro.Remain_Subscript[m];
                        ro.Remain_Subscript[m] = ro.Remain_Subscript[k];
                        ro.Remain_Subscript[k] = temp;
                    }
                }
            }
        }
        #endregion
        #region 排序--牌图形
        private void image_paixu(Role juese, int j)
        {
            if (juese.Remain_Poker.Count != 0)
            {
                if (juese.Location == 2)
                {
                    if (bl_paiXu) pai_paixu2(juese);
                    else pai_paixu(juese);
                    int a = 0;
                    for (int i = juese.Remain_Subscript.Count - 1; i >= 0; i--)
                    {
                        paiImage[(int)juese.Remain_Subscript[a]].BringToFront();
                        paiImage[(int)juese.Remain_Subscript[i]].Left = j;
                        a++; j -= 16;
                    }
                    pai_paixu(juese);
                }
                else
                {
                    int k = 390; int a = 0;
                    for (int i = 0; i < juese.Remain_Subscript.Count; i++)
                    {
                        paiImage[(int)juese.Remain_Subscript[i]].Top = k;
                        paiImage[(int)juese.Remain_Subscript[a]].BringToFront();
                        a++; k += 5;
                    }
                }
            }
        }
        #endregion
        #region 把牌的值添加到每个角色中
        private void addJuesePai(Role juese)
        {
            for (int i = 0; i < juese.Remain_Subscript.Count; i++)
            {
                juese.Remain_Poker.Add(poker[(int)juese.Remain_Subscript[i]].Size);
            }
        }
        #endregion
        #region 开始发牌
        private void fapai()
        {
            #region 3个角色发牌
            Thread.Sleep(1100);
            sound.Stream = new FileStream(@"music\sendcard.wav ", FileMode.Open, FileAccess.Read);
            if (Util.isVoice)
            {
                sound.Play();
            }
            int[] a = new int[] { 500, 10, 0, 10, 19, 0, 0 };
            for (int i = 0; i < 51; i++)
            {
                paiImage[poker[i].Index].BringToFront();
                for (int j = 0; j < 21; j += 5)
                {
                    paiImage[poker[i].Index].SetBounds(a[0] + j * 9 + a[1], 170 + 20 + j * 10 + a[2], 65, 100);
                    Thread.Sleep(5);
                }
                i++; a[0] -= 5;
                paiImage[poker[i].Index].BringToFront();
                for (int j = 0; j < 31; j += 5)
                {
                    paiImage[poker[i].Index].SetBounds(a[0] - j * a[3], 170 + j * 13, 65, 100);
                    if (j > 29)
                    {
                        paiImage[poker[i].Index].Left += 5; paiImage[poker[i].Index].Top += 5;
                        paiImage[poker[i].Index].Width = 80; paiImage[poker[i].Index].Height = 120;
                        paiImage[poker[i].Index].BackgroundImage = poker[poker[i].Index].Image;
                    }
                    Thread.Sleep(5);
                }
                i++; a[0] -= 5;
                paiImage[poker[i].Index].BringToFront();
                for (int j = 0; j < 26; j += 5)
                {
                    paiImage[poker[i].Index].SetBounds(a[0] - j * a[4] - a[5] + 3, 170 + 20 + j * 8 + a[2], 65, 100);
                    Thread.Sleep(5);
                }
                a[0] -= 5; a[1] += 15; a[2] += 5; a[3] -= 1; a[4] -= 1; a[5] += 10;
            }
            sound.Stop(); //停止播放
            #endregion
            #region 发底牌
            for (int i = 51; i < 54; i++)
            {
                for (int j = 0; j < 46; j += 5)
                {
                    paiImage[poker[i].Index].SetBounds(245 + j + a[6], 170 - j * 3, 65, 100);
                    Thread.Sleep(1);
                }
                switch (i)
                {
                    case 51: this.pic_1.BackgroundImage = Poker.BackImage; break;
                    case 52: this.pic_2.BackgroundImage = Poker.BackImage; break;
                    case 53: this.pic_3.BackgroundImage = Poker.BackImage; break;
                }
                a[6] += 65;
            }
            Thread.Sleep(100);
            #endregion
            image_paixu(role2, 460);
            image_paixu(role1, 460);
            image_paixu(role3, 460);
            bl_isRightTime2 = true;
            #region 随机决定谁先叫地主
            Random rd = new Random();
            int num = rd.Next(3) + 1;
            Thread.Sleep(500);
            int switchDiZhu = 0;
            bool bl1 = false; bool bl2 = false; bool bl3 = false; int count = 0;
            do
            {
                switch (num)
                {
                    case 1:
                        if (bl1 == false)
                        {
                            bl1 = true; count++; num++;
                            if (isJiaoDiZhu(role1))
                            {
                                count = 3; role1.Landlord = true; switchDiZhu = 1; start.End_Points = 3;
                            }
                            else
                            {
                                this.lbl_tiShi_R.Text = "不叫";
                                sound.Stream = new FileStream(@"music\buyao.wav ", FileMode.Open, FileAccess.Read);
                                if (Util.isVoice)
                                {
                                    sound.Play();
                                }
                                Thread.Sleep(1500);
                            }
                        }
                        break;
                    case 2:
                        if (bl2 == false)
                        {
                            bl2 = true; count++; num++;
                            if (isJiaoDiZhu(role3))
                            {
                                count = 3; role3.Landlord = true; switchDiZhu = 3; start.End_Points = 3;
                            }
                            else
                            {
                                this.lbl_tiShi_L.Text = "不叫";
                                sound.Stream = new FileStream(@"music\buyao.wav ", FileMode.Open, FileAccess.Read);
                                if (Util.isVoice)
                                {
                                    sound.Play();
                                }
                                Thread.Sleep(1500);
                            }
                        }
                        break;
                    case 3:
                        if (bl3 == false)
                        {
                            bl3 = true; count++; num = 1;
                            this.btn_yiFen.Invoke(weituo2, true);
                            th_faPai.Suspend();
                            if (bl_isDiZhu)
                            {
                                count = 3; role2.Landlord = true; switchDiZhu = 2;
                            }
                            else
                            {
                                this.lbl_tiShi_D.Text = "不叫";
                                sound.Stream = new FileStream(@"music\buyao.wav ", FileMode.Open, FileAccess.Read);
                                if (Util.isVoice)
                                {
                                    sound.Play();
                                }
                                Thread.Sleep(1500);
                            }
                        }
                        break;
                }
            } while (count != 3);
            Thread.Sleep(500);
            this.lbl_tiShi_L.Text = ""; this.lbl_tiShi_R.Text = ""; this.lbl_tiShi_D.Text = "";
            #endregion
            #region 判断谁是地主，然后发底牌
            if (switchDiZhu != 0)
            {
                if (switchDiZhu == 1)
                {
                    sound.Stream = new FileStream(@"music\calllord.wav ", FileMode.Open, FileAccess.Read);
                    if (Util.isVoice)
                    {
                        sound.Play();
                    }

                    picMaster.BringToFront();
                    this.lbl_difen.Text = "底分:   ×3";
                    this.pic_DiZhu.Visible = true;
                    kouDiPai(role1);
                }
                else if (switchDiZhu == 2)
                {
                    sound.Stream = new FileStream(@"music\calllord.wav ", FileMode.Open, FileAccess.Read);
                    if (Util.isVoice)
                    {
                        sound.Play();
                    }

                    picMaster.BringToFront();
                    this.lbl_difen.Text = "底分:   ×3";
                    this.pic_DiZhu.Visible = true;
                    kouDiPai(role2);
                }
                else if (switchDiZhu == 3)
                {
                    sound.Stream = new FileStream(@"music\calllord.wav ", FileMode.Open, FileAccess.Read);
                    if (Util.isVoice)
                    {
                        sound.Play();
                    }

                    picMaster.BringToFront();
                    this.lbl_difen.Text = "底分:   ×3";
                    this.pic_DiZhu.Visible = true;
                    kouDiPai(role3);
                }
                #region 翻底牌

                picMasterCar.Image = poker[poker[51].Index].Image;
                picMasterCar2.Image = poker[poker[52].Index].Image;
                picMasterCar3.Image = poker[poker[53].Index].Image;
                picMasterCar.SetBounds(260, 47, 84, 119);
                picMasterCar2.SetBounds(350, 47, 84, 119);
                picMasterCar3.SetBounds(440, 47, 84, 119);
                //pic_1.BackgroundImage = poker[poker[51].Index].Image;
                //pic_2.BackgroundImage = poker[poker[52].Index].Image;
                //pic_3.BackgroundImage = poker[poker[53].Index].Image;    
            
                #endregion
            }
            th_faPai.Abort();
            #endregion
        }
        #region 扣底牌
        private void kouDiPai(Role juese)
        {
            for (int i = 51; i < 54; i++)
            {
                juese.Remain_Subscript.Add(poker[i].Index);
                juese.Remain_Poker.Add(poker[poker[i].Index].Size);
                switch (juese.Location)
                {
                    case 1:
                        paiImage[poker[i].Index].Left = 690;
                        paiImage[poker[i].Index].Top = 475;
                        paiImage[poker[i].Index].Size = new System.Drawing.Size(65, 100);
                        break;
                    case 2:
                        paiImage[poker[i].Index].Top = 545;
                        paiImage[poker[i].Index].Size = new System.Drawing.Size(80, 120);
                        paiImage[poker[i].Index].BackgroundImage = poker[poker[i].Index].Image;
                        paiImage[poker[i].Index].Click += new System.EventHandler(paiImage_Click);
                        break;
                    case 3:
                        paiImage[poker[i].Index].Left = 18;
                        paiImage[poker[i].Index].Top = 475;
                        paiImage[poker[i].Index].Size = new System.Drawing.Size(65, 100);
                        break;
                }
            }
            pai_paixu(juese); image_paixu(juese, 495);
        }
        #endregion
        #region 随机决定电脑是否叫地主
        private bool isJiaoDiZhu(Role juese)
        {
            Random rd = new Random();
            int a = rd.Next(2);
            if (a == 0) return true;
            return false;
        }
        #endregion
        #endregion
        #endregion

        #region 出牌流程
        #region 主流程
        private void daPai()
        {

            this.btn_strat.Enabled = false;
            th_faPai.Start();
            th_faPai.Join();
            #region 开始出牌
            #region 确定地主并出第一手牌
            int num = 0;
            if (role1.Landlord)
            {
                num = 2;
                Thread.Sleep(2000);
                computerChuPai(role1);
            }
            else if (role2.Landlord)
            {
                num = 1;
                this.btn_chuPai.Invoke(weituo4, true);
                th_daPai.Suspend();
            }
            else if (role3.Landlord)
            {
                num = 3;
                Thread.Sleep(2000);
                computerChuPai(role3);
            }
            else
            {
                txt_liaoTian.Text += "\n\n  没有人叫地主,本局结束!";
                chongZhi();
                th_daPai.Abort();
            }

            bl_isFirst = true; bl_isRightTime = true;
            #endregion
            do
            {
                #region 角色一出牌或接牌
                if (num == 1)
                {

                    num++; lblTiShi = "";
                    Thread.Sleep(1000);
                    this.lbl_ZhaDan.Text = "";
                    if (buChuPai == 2)
                    {
                        computerChuPai(role1); buChuPai = 0;
                    }
                    else
                    {

                        computerJiePai(role1);
                    }
                    this.lbl_beishu.Text = "倍数:   ×" + start.A_Multiple;
                    this.lbl_tiShi_L.Text = "";
                    yiChuPai(role3);
                }
                if (bl_chuPaiOver)
                {
                    movePai(role3, receive.arrayToArgs(role3.Remain_Poker));
                    role3.Remain_Poker.Add(0);
                    break;
                }
                #endregion
                #region 角色三出牌或接牌
                if (num == 2)
                {

                    num++; lblTiShi = "";
                    this.lbl_ZhaDan.Text = "";
                    if (buChuPai == 2)
                    {
                        computerChuPai(role3); buChuPai = 0;
                    }
                    else
                    {

                        computerJiePai(role3);
                    }
                    this.lbl_beishu.Text = "倍数:   ×" + start.A_Multiple;
                    this.lbl_tiShi_D.Text = "";
                    yiChuPai(role2);
                }
                if (bl_chuPaiOver)
                {

                    movePai(role1, receive.arrayToArgs(role1.Remain_Poker));
                    role1.Remain_Poker.Add(0);
                    break;
                }
                #endregion
                #region 角色二出牌或接牌(当前玩家)
                if (num == 3)
                {
                    num = 1; lblTiShi = "";
                    if (bl_tuoGuan)
                    {
                        Thread.Sleep(1000);
                        this.lbl_ZhaDan.Text = "";
                        if (buChuPai == 2)
                        {
                            computerChuPai(role2); buChuPai = 0;
                        }
                        else computerJiePai(role2);
                    }
                    else
                    {
                        if (buChuPai == 2) this.btn_chuPai.Invoke(weituo4, true);
                        else this.btn_chuPai.Invoke(weituo3, true);
                        th_daPai.Suspend();
                    }
                    this.lbl_beishu.Text = "倍数:   ×" + start.A_Multiple;
                    this.lbl_tiShi_R.Text = "";
                    yiChuPai(role1);
                }
                if (bl_chuPaiOver)
                {
                    movePai(role1, receive.arrayToArgs(role1.Remain_Poker));
                    movePai(role3, receive.arrayToArgs(role3.Remain_Poker));
                    role1.Remain_Poker.Add(0);
                    role3.Remain_Poker.Add(0);
                    break;
                }
                #endregion
            } while (true);
            #endregion
            #region 出牌结束
            if (role2.Remain_Poker.Count == 0) ;
            else if (role1.Landlord && role1.Remain_Poker.Count != 0 || role3.Landlord && role3.Remain_Poker.Count != 0) ;
            else ;
            chongZhi(); th_daPai.Abort();
            #endregion
        }
        #endregion
        #region 电脑接牌
        private void computerJiePai(Role juese)
        {
            chuPaiWeiZhi -= role2.Hand_Poker.Count * 9;
            bool bl = tiShiJiePai(receive.isRight(playcard.PaiType, juese.Hand_Poker, juese.Remain_Poker), juese, false);
            chuPaiWeiZhi = 109;
            if (bl == false)
            {
                if (juese == role1)
                {
                    sound.Stream = new FileStream(@"music\buyao.wav ", FileMode.Open, FileAccess.Read);
                    if (Util.isVoice)
                    {
                        sound.Play();
                    }
                    this.lbl_tiShi_R.Text = "不出";
                    role3.Hand_Poker = (ArrayList)juese.Hand_Poker.Clone();
                    Thread.Sleep(1500);

                }
                else if (juese == role2)
                {
                    sound.Stream = new FileStream(@"music\buyao.wav ", FileMode.Open, FileAccess.Read);
                    if (Util.isVoice)
                    {
                        sound.Play();
                    }
                    this.lbl_tiShi_D.Text = "不出";
                    role1.Hand_Poker = (ArrayList)juese.Hand_Poker.Clone();
                    Thread.Sleep(1500);
                }
                else
                {
                    sound.Stream = new FileStream(@"music\buyao.wav ", FileMode.Open, FileAccess.Read);
                    if (Util.isVoice)
                    {
                        sound.Play();
                    }
                    this.lbl_tiShi_L.Text = "不出";
                    role2.Hand_Poker = (ArrayList)juese.Hand_Poker.Clone();
                    Thread.Sleep(1500);
                }
                buChuPai++; juese.Hand_Poker.Clear();
            }
            else buChuPai = 0;
        }
        #endregion
        #region 电脑出牌
        private void computerChuPai(Role juese)
        {
            ArrayList list = This_PlayCard.chuPai(juese.Remain_Poker);
            if (list != null && playcard.isRight(list))
            {

                role1.Hand_Poker.Clear();
                role2.Hand_Poker.Clear();
                role3.Hand_Poker.Clear();
                //chuPaiWeiZhi -= list.Count * 9;
                movePai(juese, receive.arrayToArgs(list));
                chuPaiWeiZhi = 109;
                isZhaDan_BeiShu_Add2();
            }
            else MessageBox.Show("程序出问题啦!");
        }
        #endregion
        #region 提示接牌
        private bool tiShiJiePai(ArrayList list, Role juese, bool bl_tishi)
        {
            if (playcard.PaiType == (int)Guize.天炸) return false;//如果上手出了火箭，直接要不起
            #region 单张
            else if (playcard.PaiType == (int)Guize.一张)
            {
                if (list != null)
                {
                    int[] jie = null;
                    if (((ArrayList)list[0]).Count != 0) jie = receive.mArrayToArgs((ArrayList)list[0]);
                    else if (((ArrayList)list[1]).Count != 0) jie = receive.mArrayToArgs((ArrayList)list[1]);
                    else if (((ArrayList)list[2]).Count != 0) jie = receive.mArrayToArgs((ArrayList)list[2]);
                    else if (((ArrayList)list[3]).Count != 0) jie = receive.mArrayToArgs((ArrayList)list[3]);
                    if (jie != null)
                    {
                        if (tishi == jie.Length) tishi = 0;
                        int[] _jie = new int[] { jie[tishi] };
                        if (bl_tishi) tiShiBottun(_jie);
                        else movePai(juese, _jie);
                        return true;
                    }
                }
            }
            #endregion
            #region 对子
            else if (playcard.PaiType == (int)Guize.对子)
            {
                if (list != null)
                {
                    int[] jie = null;
                    if (((ArrayList)list[0]).Count != 0) jie = receive.mArrayToArgs((ArrayList)list[0]);
                    else if (((ArrayList)list[1]).Count != 0) jie = receive.mArrayToArgs((ArrayList)list[1]);
                    else if (((ArrayList)list[2]).Count != 0) jie = receive.mArrayToArgs((ArrayList)list[2]);
                    if (jie != null)
                    {
                        if (tishi == jie.Length) tishi = 0;
                        int[] _jie = new int[] { jie[tishi], jie[tishi] };
                        if (bl_tishi) tiShiBottun(_jie);
                        else movePai(juese, _jie);
                        return true;
                    }
                }
            }
            #endregion
            #region 三张
            else if (playcard.PaiType == (int)Guize.三不带)
            {
                if (list != null)
                {
                    int[] jie = null;
                    if (((ArrayList)list[0]).Count != 0) jie = receive.mArrayToArgs((ArrayList)list[0]);
                    else if (((ArrayList)list[1]).Count != 0) jie = receive.mArrayToArgs((ArrayList)list[1]);
                    if (jie != null)
                    {
                        if (tishi == jie.Length) tishi = 0;
                        int[] _jie = new int[] { jie[tishi], jie[tishi], jie[tishi] };
                        if (bl_tishi) tiShiBottun(_jie);
                        else movePai(juese, _jie);
                        return true;
                    }
                }
            }
            #endregion
            #region 炸弹
            else if (playcard.PaiType == (int)Guize.炸弹)
            {
                if (list != null && list.Count != 0)
                {
                    int[] jie = receive.mArrayToArgs(list);
                    if (tishi == jie.Length) tishi = 0;
                    int[] _jie = new int[] { jie[tishi], jie[tishi], jie[tishi], jie[tishi] };
                    if (bl_tishi) tiShiBottun(_jie);
                    else
                    {
                        movePai(juese, _jie);
                        isZhaDan_BeiShu_Add2();
                        lblTiShi = "我也炸";
                    }
                    return true;
                }
            }
            #endregion
            #region 三带一,三带二,顺子,连对,飞机不带
            else if (playcard.PaiType > 4 && playcard.PaiType < 13)
            {
                if (list != null && list.Count != 0)
                {
                    if (tishi == list.Count) tishi = 0;
                    int[] jie = receive.mArrayToArgs((ArrayList)list[tishi]);
                    if (bl_tishi) tiShiBottun(jie);
                    else movePai(juese, jie);
                    return true;
                }
            }
            #endregion
            #region 四带二,四带两对,飞机带,三飞机带,四飞机带
            else if (playcard.PaiType > 12 && playcard.PaiType < 20)
            {
                if (list != null)
                {
                    int[] jie = receive.mArrayToArgs(list);
                    if (bl_tishi) tiShiBottun(jie);
                    else movePai(juese, jie);
                    return true;
                }
            }
            #endregion
            #region 如果同类型牌要不起，就判断是否有炸弹
            if (playcard.PaiType != (int)Guize.炸弹)
            {
                list = receive.findZhadan(juese.Remain_Poker);
                int[] jie = receive.mArrayToArgs(list);
                if (jie != null)
                {
                    if (tishi == jie.Length) tishi = 0;
                    int[] _jie = new int[] { jie[tishi], jie[tishi], jie[tishi], jie[tishi] };
                    if (bl_tishi) tiShiBottun(_jie);
                    else
                    {
                        playcard.PaiType = (int)Guize.炸弹;
                        movePai(juese, _jie);
                        isZhaDan_BeiShu_Add2();
                    }
                    return true;
                }
            }
            list = receive.findTianzha(juese.Remain_Poker);
            int[] huoJian = receive.mArrayToArgs(list);
            if (huoJian != null)
            {
                if (bl_tishi) tiShiBottun(huoJian);
                else
                {
                    movePai(juese, huoJian);
                    playcard.PaiType = (int)Guize.天炸;
                    isZhaDan_BeiShu_Add2();
                }
                return true;
            }
            #endregion
            return false;
        }
        #endregion
        #region 提示按钮
        private void tiShiBottun(int[] args)
        {
            for (int i = 0; i < role2.Remain_Subscript.Count; i++)
            {
                paiImage[(int)role2.Remain_Subscript[i]].Top = 565;
            }
            playcard.format(args);
            int num = 0;
            for (int i = 0; i < args.Length; i++)
            {
                for (int j = num; j < role2.Remain_Subscript.Count; j++)
                {
                    if (poker[(int)role2.Remain_Subscript[j]].Size == args[i])
                    {
                        paiImage[(int)role2.Remain_Subscript[j]].Top = 545;
                        num = j + 1; break;
                    }
                }
            }
        }
        #endregion
        #region 每个角色出牌(牌位置的移动)
        private void movePai(Role juese, int[] whatPai)
        {
            juese.Hand_Poker.Clear();
            playcard.format(whatPai);
            int j = 0; int place = 250;
            for (int i = 0; i < juese.Remain_Subscript.Count; i++)
            {
                if (poker[(int)juese.Remain_Subscript[i]].Size == whatPai[j])
                {
                    paiImage[(int)juese.Remain_Subscript[i]].BringToFront();
                    switch (juese.Location)
                    {
                        case 1: role3.Hand_Poker.Add(poker[(int)juese.Remain_Subscript[i]].Size);
                            paiImage[(int)juese.Remain_Subscript[i]].SetBounds(550, place, 65, 100);
                            paiImage[(int)juese.Remain_Subscript[i]].BackgroundImage = poker[(int)juese.Remain_Subscript[i]].Image;
                            break;
                        case 2: role1.Hand_Poker.Add(poker[(int)juese.Remain_Subscript[i]].Size);
                            paiImage[(int)juese.Remain_Subscript[i]].SetBounds(place + chuPaiWeiZhi, 410, 70, 105);
                            place -= 3;
                            break;
                        case 3: role2.Hand_Poker.Add(poker[(int)juese.Remain_Subscript[i]].Size);
                            paiImage[(int)juese.Remain_Subscript[i]].SetBounds(150, place, 65, 100);
                            paiImage[(int)juese.Remain_Subscript[i]].BackgroundImage = poker[(int)juese.Remain_Subscript[i]].Image;
                            break;


                    }
                    juese.Already_Poker.Add((int)juese.Remain_Subscript[i]);
                    juese.Remain_Poker.Remove(poker[(int)juese.Remain_Subscript[i]].Size);
                    juese.Remain_Subscript.RemoveAt(i);
                    place += 18; j++; i--;
                    if (j == whatPai.Length) break;
                }
            }


            if (juese.Remain_Poker.Count == 0) bl_chuPaiOver = true;
            int place1 = 0;
            if (juese.Location == 2 && juese.Landlord) place1 = 490 - (20 - juese.Remain_Subscript.Count) / 2 * 16;
            else place1 = 460 - (17 - juese.Remain_Subscript.Count) / 2 * 16;
            image_paixu(juese, place1);
            sound.Stream = new FileStream(@"music\givecard.wav ", FileMode.Open, FileAccess.Read); //出牌音效
            if (Util.isVoice)
            {
                sound.Play();
            }
        }
        #endregion
        #endregion

        #region 其它辅助方法
        #region 隐藏已出牌
        private void yiChuPai(Role juese)
        {
            for (int i = 0; i < juese.Already_Poker.Count; i++)
            {
                if (paiImage[(int)juese.Already_Poker[i]].Visible == true)
                {
                    paiImage[(int)juese.Already_Poker[i]].Visible = false;
                }
            }
        }
        #endregion

        #region 叫分按钮的显示及隐藏
        private void btnJiaoFen(bool bl)
        {
            if (bl)
            {
                this.btn_yiFen.Visible = true;
                this.btn_erFen.Visible = true;
                this.btn_sanFen.Visible = true;
                this.btn_buJiao.Visible = true;
            }
            else
            {
                this.btn_yiFen.Visible = false;
                this.btn_erFen.Visible = false;
                this.btn_sanFen.Visible = false;
                this.btn_buJiao.Visible = false;
            }
        }
        #endregion
        #region 出牌按钮的显示及隐藏
        private void btnChuPai_1(bool bl)
        {
            this.btn_chuPai.Left = 347;
            if (bl) this.btn_chuPai.Visible = true;
            else this.btn_chuPai.Visible = false;
        }
        #endregion
        #region 出牌、不出、提示按钮的显示及隐藏
        private void btnChuPai_3(bool bl)
        {
            this.btn_chuPai.Left = 265;
            if (bl)
            {
                this.btn_chuPai.Visible = true;
                this.btn_buChu.Visible = true;
                this.btn_tiShi.Visible = true;
            }
            else
            {
                this.btn_chuPai.Visible = false;
                this.btn_buChu.Visible = false;
                this.btn_tiShi.Visible = false;
            }
        }
        #endregion
        #region 判断是否出了炸弹，将倍数相乘
        private void isZhaDan_BeiShu_Add2()
        {
            if (playcard.PaiType == (int)Guize.炸弹)
            {
                
                //++
                //炸弹显示相应的音效
                sound.Stream = new FileStream(@"music\zhadan.wav ", FileMode.Open, FileAccess.Read);
                if (Util.isVoice)
                {
                    sound.Play();
                }



                start.A_Multiple *= 2; 
                lblTiShi = "我炸";
            }
            if (playcard.PaiType == (int)Guize.天炸)
            {
                //++
                //炸弹显示相应的音效
                sound.Stream = new FileStream(@"music\zhadan.wav ", FileMode.Open, FileAccess.Read);
                if (Util.isVoice)
                {
                    sound.Play();
                }
                start.A_Multiple *= 2; lblTiShi = "火箭";
            }
        }
        #endregion
        #region 判断线程状态，然后结束游戏
        private void closeForm()
        {
            if (th_faPai != null)
            {
                if (th_faPai.ThreadState == ThreadState.Suspended)
                {
                    th_faPai.Resume();
                    th_faPai.Abort();
                }
                else if (th_faPai.ThreadState == ThreadState.Running)
                {
                    th_faPai.Abort();
                    th_daPai.Abort();
                }
            }
            if (th_daPai != null)
            {
                if (th_daPai.ThreadState == ThreadState.Suspended)
                {
                    th_daPai.Resume();
                    th_daPai.Abort();
                }
                else if (th_daPai.ThreadState == ThreadState.WaitSleepJoin)
                {
                    th_daPai.Abort();
                }
                else if (th_daPai.ThreadState == ThreadState.Running)
                {
                    th_daPai.Abort();
                }
            }
        }
        #endregion
        #endregion

        #region 每局结束
        #region 重置所有对象
        private void chongZhi()
        {
            sound.Stream = new FileStream(@"music\runaway.wav ", FileMode.Open, FileAccess.Read);
            if (bl_chuPaiOver) jisuan();
            else
            {
                string[] name = new string[3] { role2.Player.Nickname, role1.Player.Nickname, role3.Player.Nickname };
                int[] score = new int[3] { 0, 0, 0 };
                gameOver = new FrmGameOver(name, score);

            }


            bl_tuoGuan = false;
            for (int i = 0; i < paiImage.Length; i++)
            {
                paiImage[i].Visible = false;
            }
            picMasterCar.Image = null;
            picMasterCar2.Image = null;
            picMasterCar3.Image = null;

            picMasterCar.SetBounds(1000, 30, 84, 119);
            picMasterCar2.SetBounds(1104, 30, 84, 119);
            picMasterCar3.SetBounds(1208, 30, 84, 119);


            picMaster.SetBounds(1100, 1100, 62, 68);
            this.pic_1.BackgroundImage = null;
            this.pic_2.BackgroundImage = null;
            this.pic_3.BackgroundImage = null;
            lblTiShi = null;
            lbl_ZhaDan.Text = "";
            lbl_tiShi_D.Text = "";
            lbl_tiShi_L.Text = "";
            lbl_tiShi_R.Text = "";
            start.AllGameNum++;
            start.End_Points = 1;
            start.A_Multiple = 1;
            bl_isDiZhu = false;
            bl_isFirst = false;
            bl_chuPaiOver = false;
            txt_paiMing.Visible = false;
            bl_isRightTime = false;
            bl_isRightTime2 = false;
            bl_paiXu = false;
            buChuPai = 0;
            this.btn_strat.Enabled = true;
            readyisture();

            tuoGuanisture();
        }
        #endregion
        #region 计算分数
        private void jisuan()
        {
            #region 计算
            int a = start.jisuan();
            List<Role> juese = new List<Role>();
            int[] player = new int[3] { player1, player2, player3 };
            int[] playerScore = new int[3];
            int sub = 0;
            juese.Add(role1);
            juese.Add(role2);
            juese.Add(role3);
            for (int i = 0; i < 3; i++)
            {
                if (juese[i].Landlord)
                {
                    if (juese[i].Remain_Poker.Count == 0) playerScore[i] += a * 2;
                    else playerScore[i] -= a * 2;
                    player[i] += playerScore[i];
                    sub = i;
                    for (int j = 0; j < 3; j++)
                    {
                        if (playerScore[j] == 0)
                        {
                            if (playerScore[i] < 0) playerScore[j] += a;
                            else playerScore[j] -= a;
                            player[j] += playerScore[j];
                        }
                    }
                    break;
                }
            }
            player1 = player[0]; player2 = player[1]; player3 = player[2];
            //this.lbl_leftJiFen.Text = player3 + "";
            //this.lbl_rightJiFen.Text = player1 + "";
            this.lbl_DownJiFen.Text = player2 + "";
            #endregion
            #region 弹出战局窗体
            string[] name = new string[3] { role2.Player.Nickname, role1.Player.Nickname, role3.Player.Nickname };
            int[] score = new int[3] { playerScore[1], playerScore[0], playerScore[2] };
            gameOver = new FrmGameOver(name, score);
            gameOver.ShowDialog();
            #endregion
            #region 排名
            string[] playerName = new string[] { role1.Player.Nickname, role2.Player.Nickname, role3.Player.Nickname };
            for (int i = 0; i < player.Length; i++)
            {
                for (int j = i; j < player.Length; j++)
                {
                    if (player[i] < player[j])
                    {
                        int temp = player[i];
                        player[i] = player[j];
                        player[j] = temp;
                        string temp1 = playerName[i];
                        playerName[i] = playerName[j];
                        playerName[j] = temp1;
                    }
                }
            }
            string paiMing = "  1\t   " + playerName[0] + "\t   " + player[0] +
                             "\n  2\t   " + playerName[1] + "\t   " + player[1] +
                             "\n  3\t   " + playerName[2] + "\t   " + player[2];
            txt_paiMing.Text = paiMing;
            #endregion
            #region 显示结果到聊天窗口
            if (sub > 0)
            {
                int temp = playerScore[sub];
                playerScore[sub] = playerScore[0];
                playerScore[0] = temp;
                Role temp1 = juese[sub];
                juese[sub] = juese[0];
                juese[0] = temp1;
            }
            string str = txt_liaoTian.Text + "\n\n  第" + start.GameNum + "局得分：\n   ";
            string str1 = "\t"; string str2 = "\t";
            if (juese[0].Remain_Poker.Count == 0) str1 = "\t ";
            else str2 = "\t ";
            str += juese[0].Player.Nickname + str1 + playerScore[0] + "\n   " +
                   juese[1].Player.Nickname + str2 + playerScore[1] + "\n   " +
                   juese[2].Player.Nickname + str2 + playerScore[2];
            txt_liaoTian.Text = str;
            #endregion
        }
        #endregion
        #endregion

        #region 调试用
        #region 测试发牌
        private void ceshi()
        {
            /*int[] pai1 = new int[] {2,4,3,15,17,16,28,29,30,41,42,43,//测试四带二对
                                    5,6,7,18,19,20,31,32,33,44,45,46,
                                    8,9,10,21,22,23,34,35,36,47,48,49,
                                    11,12,13,24,25,26,37,38,39,50,51,52,
                                    14,0,40,53,27,1};
            for (int i = 0; i < pai.Length; i++)
            {
                pai[i].Index = pai1[i];
            }*/

            /*int[] pai1 = new int[] {2,3,4,15,16,17,28,29,30,41,42,43,//测试炸弹
                                    5,6,7,18,19,20,31,32,33,44,45,46,
                                    8,9,10,21,22,23,34,35,36,47,48,49,
                                    11,12,13,24,25,26,37,38,39,50,51,52,
                                    14,27,40,53,0,1};
            for (int i = 0; i < pai.Length; i++)
            {
                pai[i].Index = pai1[i];
            }*/

            /*int[] pai1 = new int[] {10,0,11,23,16,3,36,29,28,2,42,4,//测试飞机
                                    46,7,8,24,17,21,37,30,34,15,43,47,
                                    12,5,9,25,18,22,13,31,35,51,44,48,
                                    45,6,14,20,19,27,39,32,1,52,40,26,
                                    50,33,41,49,53,38};
            for (int i = 0; i < pai.Length; i++)
            {
                pai[i].Index = pai1[i];
            }*/
        }
        #endregion
        #region 调试用，让电脑的牌显示出来
        private void tiaoshi(Role juese, int j)
        {
            /*if (juese.ShengYuPai.Count != 0)
            {
                if (juese.WeiZhi == 2)
                {
                    if (bl_paiXu) pai_paixu2(juese);
                    else pai_paixu(juese);
                }
                int k = 190; int a = 0;
                for (int i = juese.ImagePaiSub.Count - 1; i >= 0; i--)
                {
                    switch (juese.WeiZhi)
                    {
                        case 1: paiImage[(int)juese.ImagePaiSub[a]].SendToBack(); paiImage[(int)juese.ImagePaiSub[i]].Left = 620;
                            paiImage[(int)juese.ImagePaiSub[i]].Top = k; break;
                        case 2: paiImage[(int)juese.ImagePaiSub[a]].BringToFront();
                            paiImage[(int)juese.ImagePaiSub[i]].Left = j; break;
                        case 3: paiImage[(int)juese.ImagePaiSub[a]].SendToBack(); paiImage[(int)juese.ImagePaiSub[i]].Left = 85;
                            paiImage[(int)juese.ImagePaiSub[i]].Top = k; break;
                    }
                    a++; k += 18; j -= 16;
                }
                if (juese.WeiZhi == 2) pai_paixu(juese);
            }*/
        }
        #endregion
        #region 查看当前角色牌的信息
        private void ceshi1(Role juese)
        {
            /*string str = "";
            for(int i=0;i<juese.ShangShouPai.Count;i++)
            {
                str=str+(int)(juese.ShangShouPai[i])+",,";
            }
            str=str+" 剩余牌 ";
            for(int i=0;i<juese.ShengYuPai.Count;i++)
            {
                str=str+(int)(juese.ShengYuPai[i])+",,";
            }
            this.txt_liaoTian.Text += (((Guize)chupai.PaiType).ToString() + " 上手牌 " + str+"\n");*/
        }
        #endregion
        #endregion

        #region 所有事件
        #region 开始按钮事件
        private void btn_strat_Click(object sender, EventArgs e)
        {
            
            sound.Stream = new FileStream(@"music\start.wav ", FileMode.Open, FileAccess.Read);
            if (Util.isVoice)
            {
                sound.Play();
            }

            load();
            th_daPai.Start();
            readyisfalse();

        }


        #endregion
        #region 叫分按钮事件
        private void btn_jiaoDiZhu_Click(object sender, EventArgs e)
        {
            Util.MusicPlay("normal2", Util.isMusic);//抢地主换背景音乐

            Button btn = (Button)sender;
            if (btn.Name == "btn_yiFen" || btn.Name == "btn_erFen" || btn.Name == "btn_sanFen")
            {
                switch (btn.Name)
                {
                    case "btn_yiFen": start.End_Points = 1; break;
                    case "btn_erFen": start.End_Points = 2; break;
                    case "btn_sanFen": start.End_Points = 3; break;
                }
                difen3();
                //this.lbl_difen.Text = "底分:   ×" + start.End_Points; ;
                bl_isDiZhu = true;
            }
            btnJiaoFen(false);
            th_faPai.Resume();
        }
        //底分*3
        public void difen3() {
            this.lbl_difen.Text = "底分:  x" + start.End_Points; ;
        }

        #endregion

        #endregion
        #region 排序按钮事件

        #endregion
        #region 已出牌按钮事件

        #endregion
        #region 牌的点击事件
        private void paiImage_Click(object sender, EventArgs e)
        {
            if (((PictureBox)sender).Top == 565) ((PictureBox)sender).Top = 545;
            else ((PictureBox)sender).Top = 565;
        }
        #endregion
        #region 出牌按钮事件
        private void btn_chuPai_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < role2.Remain_Subscript.Count; i++)
            {
                if (paiImage[(int)role2.Remain_Subscript[i]].Top == 545)
                    saveList.Add(poker[(int)role2.Remain_Subscript[i]].Size);
            }
            int paiType = playcard.PaiType;
            if (saveList.Count != 0)
            {
                if (playcard.isRight(saveList))
                {
                    if (buChuPai != 2 && bl_isFirst)
                    {
                        if (receive.isRight(role2.Hand_Poker, saveList, paiType))
                        {

                            role1.Hand_Poker.Clear();
                            btnChuPai_3(false); chuPaiWeiZhi -= saveList.Count * 9;
                            movePai(role2, receive.arrayToArgs(saveList));
                            isZhaDan_BeiShu_Add2(); chuPaiWeiZhi = 109; buChuPai = 0;
                            th_daPai.Resume();
                        }
                        else
                        {
                            if (playcard.PaiType == paiType) MessageBox.Show("您出的牌小于上手的牌!");
                            else MessageBox.Show("您出的牌型不符!");
                            playcard.PaiType = paiType;
                        }
                    }
                    else
                    {
                        role1.Hand_Poker.Clear(); role1.Hand_Poker.Clear(); role3.Hand_Poker.Clear();
                        btnChuPai_1(false); chuPaiWeiZhi -= saveList.Count * 9;
                        movePai(role2, receive.arrayToArgs(saveList));
                        isZhaDan_BeiShu_Add2(); chuPaiWeiZhi = 109; buChuPai = 0;
                        th_daPai.Resume();
                    }
                }
                else
                {
                    playcard.PaiType = paiType;
                    MessageBox.Show("您出的牌不符合规则!");
                }
                saveList.Clear(); tishi = 0;
                for (int i = 0; i < role2.Remain_Subscript.Count; i++)
                {
                    paiImage[(int)role2.Remain_Subscript[i]].Top = 565;
                }
            }
        }
        #endregion
        #region 右击出牌事件
        private void DdzMian_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && btn_chuPai.Visible == true)
            {
                btn_chuPai_Click(sender, e);
            }
        }
        #endregion
        #region 提示按钮事件
        private void btn_tiShi_Click(object sender, EventArgs e)
        {
            bool bl = tiShiJiePai(receive.isRight(playcard.PaiType, role2.Hand_Poker, role2.Remain_Poker), role2, true);
            if (bl == false) btn_buChu_Click(sender, e);
            else tishi++;
        }
        #endregion
        #region 不出牌按钮事件
        private void btn_buChu_Click(object sender, EventArgs e)
        {
            sound.Stream = new FileStream(@"music\buyao.wav ", FileMode.Open, FileAccess.Read);
            if (Util.isVoice)
            {
                sound.Play();
            }
            for (int i = 0; i < role2.Remain_Subscript.Count; i++)
            {
                paiImage[(int)role2.Remain_Subscript[i]].Top = 565;
            }
            buChuPai++; tishi = 0;
            btnChuPai_3(false);
            role1.Hand_Poker.Clear();
            role1.Hand_Poker = (ArrayList)role2.Hand_Poker.Clone();
            role2.Hand_Poker.Clear();
            this.lbl_tiShi_D.Text = "不出";
            th_daPai.Resume();
        }
        #endregion
        #region 聊天窗口提交按钮事件

        #endregion
        #region 窗口关闭事件(关闭线程)
        private void DdzMian_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeForm();
        }
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeForm();
            this.Close();
        }
        #endregion
        #region 聊天窗口Text属性改变时,让垂直滚动条自动刷新到最底端
        private void txt_liaoTian_TextChanged(object sender, EventArgs e)
        {
            txt_liaoTian.SelectionStart = txt_liaoTian.Text.Length;
            txt_liaoTian.ScrollToCaret();
        }
        #endregion





        //返回主菜单
        private void label1_Click(object sender, EventArgs e)
        {

            DialogResult ds = MessageBox.Show("确定要退出游戏吗?", "温馨提示", MessageBoxButtons.OKCancel);
            if (ds == DialogResult.OK)
            {
                //隐藏牌
                this.btn_chuPai.Visible = false;
                this.btn_buChu.Visible = false;
                this.btn_tiShi.Visible = false;

                menu.Visible = true;
                //Application.ExitThread();退出线程
                if (role2 != null)//判断是否已经开局
                {
                    chongZhi();//重置
                    picMasterCar.Image = null;
                    picMasterCar2.Image = null;
                    picMasterCar3.Image = null;

                    picMasterCar.SetBounds(1300, 30, 84, 119);
                    picMasterCar2.SetBounds(1404, 30, 84, 119);
                    picMasterCar3.SetBounds(1508, 30, 84, 119);


                    picMaster.SetBounds(1500, 1500, 62, 68);

                }
                closeForm();//结束本局
                //th_daPai.Abort();
                this.Visible = false;
                Util.MusicPlay("back", Util.isMusic);//播放背景音乐
            }

        }

        private void FrmPlay_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);//程序全部退出
        }


        private void txt_liaoTian_TextChanged_1(object sender, EventArgs e)
        {

        }
        //鼠标单击花儿属性
        private void cbb_input_MouseClick(object sender, MouseEventArgs e)
        {
           
        }

       private void cbb_input_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbb_input.TabIndex == 0)
            {
                Console.WriteLine("sdas");
            }
        }

       private void btn_tiJiao_Click(object sender, EventArgs e)
       {
           this.txt_liaoTian.Text = "dasdasd";
       }



        //显示谁是地主
        //public void getMaster()
        //{
        //    PictureBox picMaster = new PictureBox();
        //    picMaster.Image = Image.FromFile(@"Images/back.png");
        //    picMaster.SetBounds(690,234,62,68);
        //    this.Invoke(new Action(() =>
        //   {
        //       this.Controls.Add(picMaster);
        //    }));
        //}


    }
}
