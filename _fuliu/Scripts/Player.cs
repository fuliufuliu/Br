using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour,IPicture {
    public string playerName;
    //public string ip;
    //public int port;
    //public Texture headTexture;
    public int headTextureID;
    public int grade = 1;
    public int exp = 0;
    public static int[] upgradeExps = {100,200,300,400,500,600,700,800,900,1000};
    //public int netIp;
    //public static int ownerNetIp;
    /// <summary>
    /// 唯一实例
    /// </summary>
    public static Player self;
    /// <summary>
    /// 当玩家进入房间后会创建一个游戏角色供给玩家控制
    /// </summary>
    public PlayerCharacter playerCharacter;
    /// <summary>
    /// 玩家所在的roomID号或LobbyID号,在服务器两个ID号是合并的
    /// </summary>
    public int roomOrLobbyIn;
    /// <summary>
    /// 当前Lobby的数量
    /// </summary>
    public int allLobbyCount;
    /// <summary>
    /// 当前玩家所在Lobby中玩家的数量
    /// </summary>
    public int lobbyCurrentPlayerCount;


	// Use this for initialization
	void Start () {
        self = this;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}



    public Texture FindPictrue(int id)
    {
        throw new System.NotImplementedException();
    }
}
