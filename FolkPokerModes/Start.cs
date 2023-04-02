using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolkPokerModes
{
    public class Start
    {
        public bool ready;//判断是否开局，（3个玩家是否都准备完毕）

        public bool Ready
        {
            get { return ready; }
            set { ready = value; }
        }

        public int gameNum = 0;//本局ID

        public int GameNum
        {
            get { return gameNum; }
            set { gameNum = value; }
        }

        public int allGameNum = 0;//已对战局数

        public int AllGameNum
        {
            get { return allGameNum; }
            set { allGameNum = value; }
        }

        public Role PlayCard_Role;//当前出牌的角色

        internal Role playCard_role
        {
            get { return PlayCard_Role; }
            set { PlayCard_Role = value; }
        }

        public Role Last_Role;//上手出牌的角色

        internal Role last_role
        {
            get { return Last_Role; }
            set { Last_Role = value; }
        }

        public int basicNum = 100;//游戏基数(即下注额)

        public int BasicNum
        {
            get { return basicNum; }
            set { basicNum = value; }
        }

        public int End_Points = 1;//底分(有1,2,3三种)

        public int end_points
        {
            get { return End_Points; }
            set { End_Points = value; }
        }
        public int A_Multiple = 1;//倍数(如明牌，春天，炸弹等)

        public int a_multiple
        {
            get { return A_Multiple; }
            set { A_Multiple = value; }
        }

        public int End_points = 0;//一局完毕，最后得分

        public int End_points_b
        {
            get { return End_points; }
        }

        public int jisuan()//计算方法
        {
            End_points = basicNum * End_Points * A_Multiple;
            return End_points;
        }
    }
}
