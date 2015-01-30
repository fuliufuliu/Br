using UnityEngine;
using System.Collections;
using BR_Common;
using System.Collections.Generic;
using System.Text;
using System;


/// <summary>
/// 网络翻译，
/// 基于游戏自定义协议，
/// 
/// </summary>
public class NetCmdTranslator {

    /// <summary>
    /// 发送时翻译把高层命令翻译为字节串
    /// </summary>
    /// <param name="netCmd">网络命令（网络请求）</param>
    /// <param name="datas">此网络命令需要的数据</param>
    /// <returns>翻译成为字节串</returns>
    public static byte[] Translate(NetCmd netCmd, params object[] datas)
    {
        byte[] result = null;
        switch (netCmd)
        {
            case NetCmd.Login:
            case NetCmd.SignIn:                
                //result = ByteArrayConnetion(netCmd, (string)datas[0], "[End]", (string)datas[1], "[End]");
                //result = ByteArrayConnetion(netCmd,
                //    new String20((string)datas[0]).GetByteArray(),
                //    new String20((string)datas[1]).GetByteArray());
                result = ByteArrayConnetion(netCmd, NetDataTypeDictionary.GetByteArray(new ArrayList() { 
                    datas[0], 
                    datas[1],
                }));

                //result = ByteArrayConnetion(netCmd,
                //    new String20((string)datas[0]).GetByteArray(),
                //    new String20((string)datas[1]).GetByteArray());
                break;
            default:
                
                if(datas == null)
                    result = ByteArrayConnetion(netCmd,
                        new String20(Player.self.playerName).GetByteArray());
                else 
                    result = ByteArrayConnetion(netCmd,
                        new String20(Player.self.playerName).GetByteArray(), 
                        NetDataTypeDictionary.GetByteArray(new ArrayList(datas)));
                break;
        }
        return result;
    }


    /// <summary>
    /// 发送时翻译把高层命令翻译为字节串,并发送给服务器，等待回应，等待过程处于阻塞状态
    /// </summary>
    /// <param name="errInfo">错误信息!</param>
    /// <param name="response">来自于服务器端的响应信息</param>
    /// <param name="netCmd">网络命令（网络请求）</param>
    /// <param name="datas">此网络命令需要的数据</param>
    /// <returns>成功执行返回true</returns>
    public static bool Request(out string errInfo,out object[] response, NetCmd netCmd, params object[] datas) {
        bool result = false;
        response = null;
        errInfo = string.Empty;
        try{
            byte[] responseBytes = UdpClientWork.self.Send(Translate(netCmd, datas));
            Debug.Log("接收到:"+responseBytes.Length+"个字节!");
            Debug.Log(ByteArrayConvertor.BytesToString(responseBytes));
            response = Translate(responseBytes);
            result = true;
        }catch(Exception ex){
            errInfo = ex.ToString();
        }
        return result;
    }

    /// <summary>
    /// 此翻译方法只用于服务器对客户端做出的响应的翻译
    /// </summary>
    /// <param name="receiveBytes">响应原数据</param>
    /// <returns>响应翻译后的数据，可通过typeof获得其类型</returns>
    public static object[] Translate(byte[] receiveBytes) {
        object[] result = null;
        if ((NetCmd)receiveBytes[0] == NetCmd.Echo)
        {
            result = NetDataTypeDictionary.GetValue(receiveBytes, 2, receiveBytes.Length - 2);
        }
        else
        {
            throw new Exception("此翻译方法只用于服务器对客户端做出的响应的翻译！");
        }
        return result;
    }



    /// <summary>
    /// 此方法用于翻译来自于服务器端的命令，并执行命令
    /// </summary>
    /// <param name="receiveBuffer"></param>
    /// <param name="receiveSize"></param>
    public static void ServerCmdTranslator(byte[] receiveBuffer, int receiveSize)
    {
        Debug.Log("接收到服务器端命令：" + ((ServerCmd)receiveBuffer[1]).ToString());
    }

    //private static void Echo(byte[] receiveBuffer, int receiveSize)
    //{
    //    Debug.Log("接收到服务器端命令：" + ((NetCmd)receiveBuffer[1]).ToString());
    //}

    private static byte[] ByteArrayConnetion(NetCmd netCmd, params byte[][] bytesArray)
    {
        List<byte> resultList = new List<byte>();
        resultList.Add((byte)netCmd);
        if (bytesArray != null && bytesArray.Length > 0)
        {
            foreach (byte[] bytes in bytesArray)
            {
                if (bytes == null) continue;
                for (int i = 0; i < bytes.Length; i++)
                {
                    resultList.Add(bytes[i]);
                }
            }
        }
        return resultList.ToArray();
    }

    private static byte[] ByteArrayConnetion(NetCmd netCmd, params string[] bytesArray)
    {
        List<byte> resultList = new List<byte>();
        resultList.Add((byte)netCmd);
        foreach (string bytes in bytesArray)
        {
            byte[] temp = Encoding.Default.GetBytes(bytes);
            for (int i = 0; i < temp.Length; i++)
            {
                resultList.Add(temp[i]);
            }
        }
        return resultList.ToArray();
    }

    /// <summary>
    /// 检查游戏网络协议，是否按照相应的协议传输
    /// 目前只能检测发送和接受信息是否对应
    /// </summary>
    /// <param name="sendBytes"></param>
    /// <param name="receiveBytes"></param>
    /// <returns></returns>
    public static bool CheckProtocal(byte[] sendBytes, byte[] receiveBytes)
    {
        if (sendBytes == null || receiveBytes == null || sendBytes.Length < 21 || receiveBytes.Length < 2) 
            throw new Exception("发送和接收未按照游戏协议进行！");
        else
        {
            if (sendBytes[0] == receiveBytes[1]) return true;
            else return false;
        }
    }
}
