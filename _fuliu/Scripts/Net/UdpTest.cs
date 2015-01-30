using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using System.Net;
using UnityEngine;

public class UdpTest : MonoBehaviour {
    [System.NonSerialized]
    public string serverIP = "192.168.1.100";
    [System.NonSerialized]
    public int port = 8888;

    UdpClient udpClient = new UdpClient();
    string sendMsg = "Hello Unity!";

	// Use this for initialization
	void Start () {
        IPEndPoint serverEP = new IPEndPoint(IPAddress.Parse(serverIP),port);
        byte[] sendBytes = Encoding.ASCII.GetBytes(sendMsg);
        udpClient.Send(sendBytes, sendBytes.Length, serverEP);
        print("已发送消息到：" + serverEP + ".消息内容是：" + sendMsg);

        IPEndPoint receiveEP = new IPEndPoint(0, 0);
        byte[] rcvBytes = udpClient.Receive(ref receiveEP);
        string rcvMessage = Encoding.ASCII.GetString(rcvBytes);

        print("接收到：" + rcvMessage);
        print("来自于："+receiveEP);
        udpClient.Close();  
	}
	
}
