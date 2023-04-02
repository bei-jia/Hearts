using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkPokerModes
{
    /// <summary>
    /// 玩家类
    /// </summary>
    public class User
    {
        public int id { get; set; }//玩家编号
        public string name { get; set; }//玩家昵称(可以修改)
        public int money { get; set; }//玩家可用积分
        public string password { get; set; }//玩家密码
        public string imagePath { get; set; }//玩家头像路劲
        public string tag { get; set; }//用于区分玩家

        public User() { }

        public User(string name,string pwd)
        {
            this.id = 1;
            this.name = name;
            this.password = pwd;
            this.money = 10000;//默认给10000分
        }
        public User(string name, string pwd, string imagePath)
        {
            this.name = name;
            this.password = pwd;
            this.imagePath = imagePath;
            this.money = 10000;//默认给10000分
        }
    }
}
