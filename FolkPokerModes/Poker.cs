using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace FolkPokerModes
{
    public class Poker
    {
        public Poker(int color, int size,Image image )
        {
            this.color = color;
            this.size = size;
            this.image = image;
            
        }
       
        private int index;//做为牌的随机属性

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        private static Image backImage;//背面图，静态

        public static Image BackImage
        {
            get { return Poker.backImage; }
            set { Poker.backImage = value; }
        }

        private int color;//牌的花色
        
        public int Color
        {
            get { return color; }
        }
        
        private Image image;//牌的正面图

        public Image Image
        {
            get { return image; }
        }

        private int size;//牌的大小

        public int Size
        {
            get { return size; }
        }

        public string outPut()
        {
            string[] str = new string[] {"黑桃","红桃","梅花","方块"};
            string[] str1 = new string[] { "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A", "2"};
            if (size == 16) return "小王";
            if (size == 17) return "大王";
            return str[color-1] + str1[size-3];
        }
    }
}
