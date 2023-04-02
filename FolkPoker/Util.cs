using AxWMPLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace FolkPoker
{
    public class Util
    {
        public static SoundPlayer sond;//播放快捷声音
        //public static SoundPlayer sondMisic;//播放音乐
        public static AxWindowsMediaPlayer play = new AxWindowsMediaPlayer();//播放音乐
        public static Boolean isMusic = true;//是否为音乐
        public static Boolean isVoice = true;//是否为快捷声音
        public static Boolean isPlay = true;//是否为游客


        //播放鼠标移上lbl的声音
        public static void VoicePlay(String path, bool isOK)
        {
            if (isOK)
            {
                sond = new SoundPlayer(@"music\" + path + ".wav");
                sond.Play();
            }
        }
        //播放背景音乐
        public static void MusicPlay(String path, Boolean isMusic)
        {
            if (isMusic)
            {
                play.CreateControl();
                play.settings.setMode("loop", true);
                play.URL = @"music\" + path + ".mp3";
                play.Ctlcontrols.play();
            }
        }


    }
}
