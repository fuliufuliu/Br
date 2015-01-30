using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;
using System.Threading;
using BR_Common;

public class UdpClientWork : MonoBehaviour {
    public static UdpClientWork self;
    public string serverIP = "192.168.1.100";
    public int serverLoginPort = 8888;
    public int serverGamePort = 8889;
    public int bufferSize = 1024;
    /// <summary>
    /// 回应有效期，单位为毫秒
    /// </summary>
    public int echoLife = 2500;

    private bool _isSending = false;
    /// <summary>
    /// 是否正在发送过程中，发送数据是瞬间完成的，不管有没有受到，直到接收到回应信息为止
    /// </summary>
    public bool isSending { get { return _isSending; } }
    private int _receivedDataSize;
    /// <summary>
    /// 接收到数据报的大小
    /// </summary>
    public int receivedDataSize { get { return _receivedDataSize; } }
    private byte[] _receiveBuffer;
    /// <summary>
    /// 接收数据报的缓冲
    /// </summary>
    public byte[] receiveBuffer { get { return _receiveBuffer; } }
    private bool _isReceivedData = false;
    /// <summary>
    /// 是否接收到数据，当接收到数据后为true，当处理数据完毕后设为false
    /// </summary>
    public bool isReceivedData { get { return _isReceivedData; } }

    private Thread listenThread;
    private Thread nlistenThread;
    UdpClient udpClient = new UdpClient();
    IPEndPoint serverEP;

    public bool isConnected = false;

    void Start()
    {
        self = this;
    }
    void OnEnable()
    {
        _receiveBuffer = new byte[bufferSize];
        serverEP = new IPEndPoint(IPAddress.Parse(serverIP),serverLoginPort);

        //发送接收测试，同时也是一个连接的过程！
        try
        {
            IPEndPoint anyIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] buffer = new byte[bufferSize];
            string sendTestString = "Test Connection!";
            byte[] sendTestBytes = Encoding.ASCII.GetBytes(sendTestString);
            udpClient.Send(sendTestBytes, sendTestBytes.Length, serverEP);
            print("已发送测试字节数组\"" + sendTestString + "\"到服务器！服务器EndPoint：" + serverEP);
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 200);
            buffer = udpClient.Receive(ref anyIpEndPoint);
            print("接收到一个数据包！\"" + Encoding.ASCII.GetString(buffer) + "\"");
            udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 0);
            //新建线程监听服务器消息
            listenThread = new Thread(new ParameterizedThreadStart(ListenThread));
            listenThread.Start(udpClient);
            isConnected = true;
        }
        catch (Exception ex)
        {
            //throw new Exception ("连接服务器失败！"+ex.ToString());
            print("连接服务器失败！" + ex.ToString());
            isConnected = false;
        }
    }

    /// <summary>
    /// 更换GamePort,标志着正式进入游戏
    /// </summary>
    /// <returns>执行成功则返回true</returns>
    public bool ChangeNextPort()
    {
        try
        {
            ////停止已有线程
            //if (listenThread != null) listenThread.Abort();
            //更换port
            serverEP = new IPEndPoint(IPAddress.Parse(serverIP), serverGamePort);

            //IPEndPoint anyIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            //byte[] buffer = new byte[bufferSize];
            //string sendTestString = "Test Connection!";
            //byte[] sendTestBytes = Encoding.ASCII.GetBytes(sendTestString);
            //udpClient.Send(sendTestBytes, sendTestBytes.Length, serverEP);
            //print("已发送测试字节数组\"" + sendTestString + "\"到服务器！服务器EndPoint：" + serverEP);
            //udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 1000);
            //buffer = udpClient.Receive(ref anyIpEndPoint);
            //print("接收到一个数据包！\"" + Encoding.ASCII.GetString(buffer) + "\"");
            //udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 0);

            ////新建线程监听服务器消息
            //nlistenThread = new Thread(new ParameterizedThreadStart(ListenThread));
            //nlistenThread.Start(udpClient);
            return true;
        }
        catch(Exception ex)
        {
            Debug.LogError(ex.ToString());
            return false;
        }
    }



    /// <summary>
    /// 发送请求给服务器，并等待服务器的回应消息，最多等待echoLife毫秒，此方法有阻塞，最多阻塞echoLife毫秒
    /// </summary>
    /// <param name="bytes">发送的内容</param>
    /// <returns>接收到则返回接收到的字节数组，长度是实际收到的字节数，未接收到则返回null</returns>
    public byte[] Send(byte[] bytes)
    {
        _isSending = true;
        if (!SingleSend(bytes)) 
        { 
            _isSending = false; 
            return null; 
        }
        //发送成功继续执行接收服务器回应信息
        int sendTime = DateTime.Now.Second*1000+DateTime.Now.Millisecond;
        int timeDelta;
        while (true)
        {
            //未收到信息，则被阻塞
            timeDelta = (DateTime.Now.Second * 1000 + DateTime.Now.Millisecond) - sendTime;
            timeDelta = timeDelta >= 0 ? timeDelta : (timeDelta + 60000);
            if (timeDelta > echoLife)
            {
                _isSending = false;
                return null;
            }            
            if (_isReceivedData)
            //接收到信息，不仅仅是响应,还有可能来自于服务端的命令（请求）
            {
                if (NetCmdTranslator.CheckProtocal(bytes, _receiveBuffer))
                {
                    Debug.Log("接收到" + _receivedDataSize + "个字节");
                    byte[] newBytes = new byte[_receivedDataSize];
                    Array.Copy(_receiveBuffer, newBytes, _receivedDataSize);
                    InitializeReceiveBuff();
                    _isSending = false;
                    return newBytes;
                }
            }                
            Thread.Sleep(1);
        }
    }

    /// <summary>
    /// 单程发送，不接收回应消息
    /// </summary>
    /// <param name="bytes">发送的内容</param>
    /// <returns></returns>
    bool SingleSend(byte[] bytes)
    {
        try
        {
            if (bytes == null) { Debug.LogError("要发送的数据为空"); return false; }
            //if (udpSendSocket == null) { Debug.LogError("udpClientSocket为空"); return false; }
            //udpSendSocket.Send(bytes);
            print(serverEP);
            udpClient.Send(bytes, bytes.Length,serverEP);
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError("发送数据时发生错误！" + ex.ToString());
            return false;
        }
    }

    void ListenThread(object obj)
    {
        UdpClient udpRcvClient = (UdpClient)obj;
        IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverLoginPort);
        byte[] buffer = new byte[bufferSize];
        IPEndPoint anyIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
        while (true)
        {
            try
            {
                print("开始接收！");
                buffer = udpRcvClient.Receive(ref anyIpEndPoint);
                print("接收到一个数据包！");
                if ((anyIpEndPoint as IPEndPoint).Address.Equals((serverEndPoint as IPEndPoint).Address))
                {
                    _receiveBuffer = buffer;
                    ReceiveCompletedEvent(buffer, _receiveBuffer.Length);
                }
                else
                {
                    print("接收到来自于非服务器端的数据包");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.ToString());
                break;
            }
        }
    }

    /// <summary>
    /// 接收完成时触发此事件，完成接收消息的初步分类
    /// </summary>
    /// <param name="receiveBuffer">缓冲接收字节</param>
    /// <param name="receiveSize">接收字节数</param>
    void ReceiveCompletedEvent(byte[] receiveBuffer, int receiveSize)
    {
        if (receiveSize > 0)
        {
            Debug.Log("接收到一条信息！"+Encoding.Default.GetString(receiveBuffer,0,receiveSize));
            this._receiveBuffer = receiveBuffer;
            this._receivedDataSize = receiveSize;
            this._isReceivedData = true;
            //如果不是客户端请求的响应信息，则提交翻译
            if ((NetCmd)receiveBuffer[0] == NetCmd.ServerCmd)
            {
                NetCmdTranslator.ServerCmdTranslator(receiveBuffer, receiveSize);
                InitializeReceiveBuff();
            }
        }
        else
        {
            Debug.LogError("接收到0字节信息！");
        }
    }

    /// <summary>
    /// 初始化接收缓冲区以及相应的参数
    /// </summary>
    void InitializeReceiveBuff()
    {
        this._receiveBuffer = new byte[bufferSize];
        this._receivedDataSize = 0;
        this._isReceivedData = false;
    }

    void OnDisable()
    {
        if (udpClient != null) udpClient.Close();
        if (listenThread != null) listenThread.Abort();
        if (nlistenThread != null) nlistenThread.Abort();
    }

    void OnApplicationQuit()
    {
        if (listenThread != null) listenThread.Abort();
        if (udpClient != null) udpClient.Close();
    }

}
