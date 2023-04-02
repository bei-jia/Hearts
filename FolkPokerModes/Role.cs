using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkPokerModes
{
    public class Role
    {
        public Role(Player player)
        {
            this.player = player;
        }

        public Player player;//玩家信息（玩家与角色绑定）

        public Player Player
        {
            get { return player; }
            set { player = value; }
        }

        public int Location;//角色在界面的哪个位置

        public int location
        {
            get { return Location; }
            set { Location = value; }
        }



        public bool Landlord = false;//角色类型，是否为地主（false为农民，true为地主）

        public bool landlord
        {
            get { return Landlord; }
            set { Landlord = value; }
        }

        public ArrayList Remain_Subscript = new ArrayList();//保存每个角色剩余牌的图形的下标

        public ArrayList Remain_Subscript1
        {
            get { return Remain_Subscript; }
            set { Remain_Subscript = value; }
        }

        

        public ArrayList Remain_Poker = new ArrayList();//剩余的牌

        public ArrayList remain_poker
        {
            get { return Remain_Poker; }
            set { Remain_Poker = value; }
        }

        public ArrayList Hand_Poker = new ArrayList();//保存上手牌

        public ArrayList hand_poker
        {
            get { return Hand_Poker; }
            set { Hand_Poker = value; }
        }

        public ArrayList Already_Poker = new ArrayList();//已出的所有牌

        public ArrayList already_poker
        {
            get { return Already_Poker; }
            set { Already_Poker = value; }
        }
    }
}
