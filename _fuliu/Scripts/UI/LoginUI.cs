using UnityEngine;
using System.Collections;
using BR_Common;
using System.Text;
using System.Xml;
using TreeEditor;
using System;

public class LoginUI : MonoBehaviour {
    public UIInput userNameUIInput;
    public UIInput passwordUIInput;
    public UIButton loginButton;
    public UIButton signInButton;
    public UILabel errorLabel;
	public UIToggle saveUsernameToggle;
    public UIGrid roomGrid;

    public UdpClientWork udpClientWork;
    public Player player;
    private static LoginUI self;

	// Use this for initialization
	void Start () {
        self = this;
        UIEventListener.Get(loginButton.gameObject).onClick = LoginButtonOnClick;
        UIEventListener.Get(signInButton.gameObject).onClick = SignInButtonOnClick;
        if (!UdpClientWork.self.isConnected) errorLabel.text = "服务器连接失败!";

		XmlDocument dom = new XmlDocument();
        dom.Save("Assets/_fuliu/Scripts/UI/aaa.xml");
		dom.Load("Assets/_fuliu/Scripts/UI/aaa.xml");//装载XML文档 
		//遍历所有节点 
		int num = 0;
		foreach(XmlElement birthday in dom.DocumentElement.ChildNodes) 
		{ 
			//读取数据 
			string type = birthday.SelectSingleNode("type").InnerText; 
			string date = birthday.SelectSingleNode("date").InnerText; 
			string title = birthday.SelectSingleNode("title").InnerText; 
			string name = birthday.SelectSingleNode("name").InnerText; 
			string text = name + ":" + title;//节点文字 
			string image = type;//节点图片 
			string data = num.ToString();//节点对应数据 
			num++;
            //装载示例，将新建的节点添加到TreeView 
            //TreeNode node = new TreeNode();//create a new node 
            //treeView.Nodes.Add(node); 

			//编辑示例:将当前节点的生日更改为当前日期 
			birthday.SelectSingleNode("date").InnerText = DateTime.Now.ToString(); 
			//删除示例：将当前节点删除 
			birthday.ParentNode.RemoveChild(birthday);
            dom.Save("Assets/_fuliu/Scripts/UI/aaa.xml");

		} 
	}

    public static void Login(string username, string password)
    {
        self.userNameUIInput.value = username;
        self.passwordUIInput.value = password;
        self.LoginButtonOnClick(null);
    }

    private void LoginButtonOnClick(GameObject go)
    {
        print("LoginButtonOnClick");
        if (Check())
        {
            //byte[] echoBytes = udpClientWork.Send(
            //    NetCmdTranslator.Translate(
            //    NetCmd.Login, userNameUIInput.value, passwordUIInput.value
            //    ));
            object[] responseObjs;
                bool isSuccessed;
                string errInfo;
            isSuccessed = NetCmdTranslator.Request(out errInfo, out responseObjs, NetCmd.Login,new String20(userNameUIInput.value), new String20(passwordUIInput.value));
            if(isSuccessed){
                foreach (var item in responseObjs)
                {
                    print("-----"+item.ToString());
                }
                if ((bool)responseObjs[0] == true)
                {
                    player.playerName = userNameUIInput.value;
                    player.headTextureID = (int)responseObjs[1];
                    player.grade = (int)responseObjs[2];
                    player.exp = (int)responseObjs[3];
                    player.roomOrLobbyIn = (int)responseObjs[4];
                    player.allLobbyCount = (int)responseObjs[5];
                    player.lobbyCurrentPlayerCount = (int)responseObjs[6];
                    //print(AllScene.self.allScene[1].ToString().TrimEnd("(UnityEngine.SceneAsset)".ToCharArray()));
                    //Application.DontDestroyOnLoad(udpClientWork.gameObject);
                    DontDestroyOnLoad(udpClientWork.gameObject);
                    UdpClientWork.self.ChangeNextPort();
                    Application.LoadLevel("Lobby");
                }
                else
                {
                    errorLabel.text = (string)responseObjs[1];
                }


            }else{
                Debug.LogError("接收错误:" + errInfo);
            }

        }
    }

    private void SignInButtonOnClick(GameObject go)
    {
        print("SignInButtonOnClick");
        if (Check())
        {
            object[] responseObjs;
            bool isSuccessed;
            string errInfo;
            isSuccessed = NetCmdTranslator.Request(out errInfo, out responseObjs, NetCmd.SignIn, new String20(userNameUIInput.value), new String20(passwordUIInput.value));
            if (isSuccessed)
            {
                foreach (var item in responseObjs)
                {
                    print("-----" + item.ToString());
                }
                if ((bool)responseObjs[0] == true)
                {
                    errorLabel.text = (string)responseObjs[1];
                    //player.headTextureID = (int)responseObjs[1];
                    //player.grade = (int)responseObjs[2];
                    //player.exp = (int)responseObjs[3];
                    //player.roomOrLobbyIn = (int)responseObjs[4];
                    //player.allLobbyCount = (int)responseObjs[5];
                    //player.lobbyCurrentPlayerCount = (int)responseObjs[6];
                    //print(AllScene.self.allScene[1].ToString().TrimEnd("(UnityEngine.SceneAsset)".ToCharArray()));
                    ////Application.DontDestroyOnLoad(udpClientWork.gameObject);
                    //DontDestroyOnLoad(udpClientWork.gameObject);
                    //Application.LoadLevel("Lobby");
                }
                else
                {
                    errorLabel.text = (string)responseObjs[1];
                }


            }
            else
            {
                Debug.LogError("接收错误:" + errInfo);
            }

        }
    }

    bool Check()
    {
        int ul, pl;
        ul = Encoding.Default.GetBytes(userNameUIInput.value).Length;
        pl = Encoding.Default.GetBytes(passwordUIInput.value).Length;
        if (ul <= 5 ||
            pl <= 5 ||
            ul > 20 ||
            pl > 20)
        {
            errorLabel.text = "用户名或密码长度太短或太长，长度必须大于等于6个字符，小于等于20个字符！一个汉字占两个字符";
            return false;
        }
        else
        {
            errorLabel.text = "";
            return true;
        }
    }
	

}
