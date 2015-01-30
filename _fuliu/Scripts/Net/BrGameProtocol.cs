using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace BR_Common
{
    public enum NetCmd //Response也使用此协议
    {
        //登录
        Login = 5,
        SignIn = 6,

        //系统
        ExitRoom = 4,
        QuitGame = 7,

        //房间玩家操作
        Use = 8,
        Pickup = 9,
        MoveTo = 10,
        Buy = 11,
        Chat = 12,
        MakeTeam = 13,
        EnterTeam = 14,
        ExitTeam = 15,

        //大厅玩家操作
        BuyProp = 20,
        EnterRoom = 21,
        GetPlayerItemsInfo = 22,
        GetLobbyInfo = 23,
        GetRoomsInfo = 24,

        //回应
        Echo = 31,
        //服务器命令
        ServerCmd = 32,
    }

    public enum ServerCmd
    {
        //禁区事件
        AddForbiddenZone = 15,
        RemoveForbiddenZon = 16,
        KillYouByForbiddenZone = 17,
        FreeInForbiddenZone = 18,

        //天气事件
        ChangeWeather = 20,
        ChangeDayOrNight = 21,


        //遭遇
        Meet = 30

    }

    /// <summary>
    /// 网络数据类型
    /// </summary>
    public enum NetDataType
    {
        Bool = 10,
        Float =12,
        Int=14,
        PlayerUserName=16,
        String20=18,//长度为20的字符串
        String100 = 20,
        StringEnd = 22,//未以“[End]”结尾的字符串
        Char = 24,
        Vector3 = 26,
        BrObjectID =28,//Br游戏物品的ID
        Byte,
    }

    /// <summary>
    /// 网络数据类型字典
    /// </summary>
    public class NetDataTypeDictionary
    {
        private static Dictionary<NetDataType, int> dic = new Dictionary<NetDataType, int>();
        static NetDataTypeDictionary()
        {
            dic.Add(NetDataType.Bool, 1);
            dic.Add(NetDataType.Float, 4);
            dic.Add(NetDataType.Int, 4);
            dic.Add(NetDataType.PlayerUserName, 20);
            dic.Add(NetDataType.String20, 20);
            dic.Add(NetDataType.StringEnd, 0);
            dic.Add(NetDataType.String100, 100);
            dic.Add(NetDataType.Char, 1);
            dic.Add(NetDataType.Vector3, 12);
            dic.Add(NetDataType.BrObjectID, 4);
            dic.Add(NetDataType.Byte, 1);
        }
        /// <summary>
        /// 获取NetData所占用的字节数
        /// </summary>
        /// <param name="ndt">Key</param>
        /// <returns>value</returns>
        public static int Query(NetDataType ndt)
        {
            int result;
            if (dic.TryGetValue(ndt, out result))
            {
                return result;
            }
            else
            {
                throw new Exception("网络可传输的数据类型出错！无该类型！");
            }
        }


        /// <summary>
        /// 由字节数组得到相应的数据保存到object数组中
        /// </summary>
        /// <param name="bytes">待翻译的字节数组</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="count">数量</param>
        /// <returns>返回数据到object中</returns>
        public static object[] GetValue(byte[] bytes, int startIndex, int count)
        {
            int endIndex = startIndex + count-1;
            if (endIndex >= bytes.Length) throw new Exception(string.Format(
                "协议转换异常！给定的字节数组大小{0}<给定的起始位置{1}+数量{2}",bytes.Length,startIndex,count));
            ArrayList results = new ArrayList();
            //try
            //{
                for (int i = startIndex; i <= endIndex; i++)
                {
                    switch ((NetDataType)bytes[i])
                    {
                        case NetDataType.Bool:
                            bool rb = bytes[i + 1] == 1 ? true : false;
                            i++;
                            results.Add(rb);
                            break;
                        case NetDataType.Float:
                            //float rf = float.Parse(Encoding.Default.GetString(bytes, i + 1, 4));
                            float rf = BitConverter.ToSingle(bytes, i + 1);
                            i += 4;
                            results.Add(rf);
                            break;
                        case NetDataType.BrObjectID:
                        case NetDataType.Int:
                            //Int32 ri = int.Parse(Encoding.Default.GetString(bytes, i + 1, 4));
                            Int32 ri = BitConverter.ToInt32(bytes, i + 1);
                            i += 4;
                            results.Add(ri);
                            break;
                        case NetDataType.PlayerUserName:
                        case NetDataType.String20:
                            string s1 = new String20(bytes, i + 1).ToString();
                            i += 20;
                            results.Add(s1);
                            break;
                        case NetDataType.String100:
                            string s2 = new String100(bytes, i + 1).ToString();
                            i += 100;
                            results.Add(s2);
                            break;
                        case NetDataType.StringEnd:
                            string ss = Encoding.Default.GetString(bytes, i + 1, bytes.Length - (i + 1));
                            int stringEndsNumber = 0;
                            string[] sss = ss.Split(new string[] { StringEnd.endTag + (char)NetDataType.StringEnd  ,StringEnd.endTag}, StringSplitOptions.RemoveEmptyEntries);
                            if (ss.EndsWith("[End]")) stringEndsNumber = sss.Length;
                            else stringEndsNumber = sss.Length - 1;
                            //ss = sss[0];
                            for (int j = 0; j < stringEndsNumber; j++)
                            {
                                i += Encoding.Default.GetBytes(sss[j]).Length + StringEnd.endTag.Length +1;
                                results.Add(sss[j]);
                            }
                            break;
                        case NetDataType.Char:
                            char rc = (char)bytes[i + 1];
                            i++;
                            results.Add(rc);
                            break;
                        case NetDataType.Vector3:
                            Vector3 v3 = new Vector3();
                            v3.x = float.Parse(Encoding.Default.GetString(bytes, i + 1, 4));
                            v3.y = float.Parse(Encoding.Default.GetString(bytes, i + 5, 4));
                            v3.z = float.Parse(Encoding.Default.GetString(bytes, i + 9, 4));
                            results.Add(v3);
                            i += 12;
                            break;
                        case NetDataType.Byte:
                            byte b = bytes[i + 1];
                            i += 1;
                            results.Add(b);
                            break;
                        default:
                            throw new Exception("协议转换异常！未定义此类型的数据类型。要在" + typeof(NetDataType).ToString() + "枚举定义中指定过才行！获得的数字是:" + bytes[i]);
                    }
                }
            //}
            //finally
            //{

            //}
            return results.ToArray();
        }

        /// <summary>
        /// 将不同参数的列表转换成字节数组
        /// </summary>
        /// <param name="paramList">不同参数的列表</param>
        /// <returns></returns>
        public static byte[] GetByteArray(ArrayList paramList)
        {
            List<byte> result = new List<byte>();
            foreach (var item in paramList)
            {
                if (item.GetType() == typeof(int) || item.GetType() == typeof(Int32))
                {
                    result.Add((byte)NetDataType.Int);
                    result.AddRange(BitConverter.GetBytes((Int32)item));
                }
                else if (item.GetType() == typeof(String20))
                {
                    result.Add((byte)NetDataType.String20);
                    result.AddRange(((String20)item).GetByteArray());
                }
                else if (item.GetType() == typeof(String100))
                {
                    result.Add((byte)NetDataType.String100);
                    result.AddRange(((String100)item).GetByteArray());
                }
                else if (item.GetType() == typeof(StringEnd))
                {
                    result.Add((byte)NetDataType.StringEnd);
                    result.AddRange(((StringEnd)item).GetByteArray());
                }
                else if (item.GetType() == typeof(float))
                {
                    result.Add((byte)NetDataType.Float);
                    result.AddRange(BitConverter.GetBytes((float)item));
                }
                else if (item.GetType() == typeof(char))
                {
                    result.Add((byte)NetDataType.Char);
                    result.AddRange(BitConverter.GetBytes((char)item));
                }
                else if (item.GetType() == typeof(bool))
                {
                    result.Add((byte)NetDataType.Bool);
                    result.Add((byte)((bool)item == true ? 1:126));
                }
                else if (item.GetType() == typeof(Vector3))
                {
                    result.Add((byte)NetDataType.Vector3);
                    result.AddRange(BitConverter.GetBytes(((Vector3)item).x));
                    result.AddRange(BitConverter.GetBytes(((Vector3)item).y));
                    result.AddRange(BitConverter.GetBytes(((Vector3)item).z));
                }
                else if (item.GetType() == typeof(byte))
                {
                    result.Add((byte)NetDataType.Byte);
                    result.Add((byte)(item));
                }
            }
            return result.ToArray();
        }
    }//End NetDataTypeDictionary

    #region 新数据类型定义

    public class String20
    {
        byte[] buff = new byte[20];
        int count = 0;
        private string s;
        public String20(string s)
        {
            byte[] temp = Encoding.Default.GetBytes(s);
            count = temp.Length > 20 ? 20 : s.Length;
            this.s = s;
            for (int i = 0; i < 20; i++)
            {
                if (i < temp.Length)
                {
                    buff[i] = temp[i];
                }
                else
                {
                    buff[i] = 126;
                }
            }
        }


        public String20(byte[] bytes,int startIndex)
        {
            if (bytes.Length < startIndex + 20) 
                throw new Exception(string.Format("String20构造异常,参数字节数组必须长度大于等于20!而此长度为:{0}",bytes.Length));
            List<byte> temp = new List<byte>();
            for (int i = 0; i < 20;i++ )
            {
                buff[i] = bytes[startIndex + i];
                if (bytes[startIndex + i] != 126)
                temp.Add(bytes[startIndex + i]);
            }
            buff = temp.ToArray();
            s = Encoding.Default.GetString(buff);
            count = s.Length;
        }

        public byte[] GetByteArray()
        {
            return buff;
        }

        public override string ToString()
        {
            return s;
        }

    }

    public class String100
    {
        byte[] buff = new byte[100];
        int count = 0;
        string s;
        public String100(string s)
        {
            byte[] temp = Encoding.Default.GetBytes(s);
            count = temp.Length > 100 ? 100 : s.Length;
            this.s = s;
            for (int i = 0; i < 100; i++)
            {
                if (i < temp.Length)
                {
                    buff[i] = temp[i];
                }
                else
                {
                    buff[i] = 126;
                }
            }
        }

        public String100(byte[] bytes,int startIndex)
        {
            if (bytes.Length < startIndex + 100) 
                throw new Exception(string.Format("String100构造异常,参数字节数组必须长度大于等于20!而此长度为:{0}",bytes.Length));
            List<byte> temp = new List<byte>();
            for (int i = 0; i < 100;i++ )
            {
                buff[i] = bytes[startIndex + i];
                if (bytes[startIndex + i] != 126)
                temp.Add(bytes[startIndex + i]);
            }
            buff = temp.ToArray();
            s = Encoding.Default.GetString(buff);
            count = s.Length;
        }

        public byte[] GetByteArray()
        {
            return buff;
        }

        public override string ToString()
        {
            return s;
        }
    }

    /// <summary>
    /// 以[End]为结束符的字符串
    /// </summary>
    public class StringEnd
    {
        private static string _endTag = "[End]";
        string s;
        /// <summary>
        /// 结束符串
        /// </summary>
        public static string endTag { get { return _endTag; } }
        string targetS = string.Empty;
        public StringEnd(string s)
        {
            this.s = s;
            this.targetS = s + endTag;
        }

        //public StringEnd(byte[] bytes)
        //{
        //    this.s = Encoding.Default.GetString(bytes);
        //    string[] ss = s.Split(new string[] { "[End]" }, StringSplitOptions.None);
        //    if(ss.Length>0)this.s = ss[0];
        //    targetS = s + "[End]";
        //}

        public byte[] GetByteArray()
        {
            return Encoding.Default.GetBytes(targetS);
        }
        public override string ToString()
        {
            return s;
        }
    }
    #endregion 新数据类型定义

    public class ByteArrayConvertor
    {
        public static string BytesToString(byte[] bytes)
        {
            string text = string.Empty;
            if (bytes == null || bytes.Length == 0) return string.Empty;
            foreach (byte b in bytes)
            {
                if (b < 33 || b == 127) text += "[" + b + "]";
                else text += (char)b;
            }
            return text;
        }
    }

}
