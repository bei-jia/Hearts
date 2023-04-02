using FolkPokerModes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FolkPokerBLL
{
    /// <summary>
    /// 用于加载数据与保存数据的类
    /// </summary>
    public class LoadAndSaveManager
    {
        /// <summary>
        /// 检查设置 Info.xml文件是否存在，不存在创建
        /// </summary>
        public void CheckFile()
        {
            if (!File.Exists("Info.xml"))
            {
                try
                {
                    //创建文件流
                    FileStream fs = new FileStream("Info.xml", FileMode.Create);
                    //创建写入器
                    StreamWriter sw = new StreamWriter(fs);
                    //写入内容
                    sw.WriteLine("<?xml version='1.0' encoding='utf-8' ?>");
                    sw.WriteLine("<Info>");
                    sw.WriteLine("\t<user>");
                    sw.WriteLine("\t\t<id></id>");
                    sw.WriteLine("\t\t<name></name>");
                    sw.WriteLine("\t\t<password></password>");
                    sw.WriteLine("\t\t<money></money>");
                    sw.WriteLine("\t</user>");
                    sw.WriteLine("</Info>");
                    sw.Close();
                    fs.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }
        /// <summary>
        /// 读取用户信息存档
        /// </summary>
        public User LoadUserInfo()
        {
            User user = new User();
            XmlDocument xml = new XmlDocument();
            xml.Load("info.xml");//加载xml文件
            XmlNode userInfo = xml.DocumentElement;//读取根节点
            //循环读取用户信息
            foreach (XmlNode item in userInfo.ChildNodes)
            {
                //为user节点时，进行赋值
                if (item.Name.Equals("user"))
                {
                    foreach (XmlNode node in item.ChildNodes)
                    {
                        switch (node.Name)
                        {
                            case "id":
                                user.id = Convert.ToInt32(node.InnerText);
                                break;
                            case "name":
                                user.name = node.InnerText;
                                break;
                            case "money":
                                user.money = Convert.ToInt32(node.InnerText);
                                break;
                            case "password":
                                user.password = Convert.ToString(node.InnerText);
                                break;
                            //case "maxStage":
                            //    user.maxStage = Convert.ToInt32(node.InnerText);
                            //    break;
                        }
                    }
                }
            }
            return user;
        }
        /// <summary>
        /// 用户信息存档
        /// 保存注册信息
        /// </summary>
        public void SaveUserInfo(User user)
        {
            try
            {
                //Option option = new Option();
                XmlDocument xml = new XmlDocument();
                xml.Load("Info.xml");//加载xml文件
                XmlNode userInfo = xml.DocumentElement;//读取根节点
                //循环读取用户信息
                foreach (XmlNode item in userInfo.ChildNodes)
                {
                    //找到user节点进行赋值
                    if (item.Name.Equals("user"))
                    {
                        foreach (XmlNode node in item.ChildNodes)
                        {
                            switch (node.Name)
                            {
                                case "id":
                                    node.InnerText = user.id.ToString();
                                    break;
                                case "name":
                                    node.InnerText = user.name;
                                    break;
                                case "money":
                                    node.InnerText = user.money.ToString();
                                    break;
                                case "password":
                                    node.InnerText = user.password;
                                    break;
                            }
                        }
                    }
                }
                //保存读取的xml文件
                xml.Save("Info.xml");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}