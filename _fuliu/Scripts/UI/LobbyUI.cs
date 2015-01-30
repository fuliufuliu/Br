using UnityEngine;
using System.Collections;
using System.Threading;

public class LobbyUI : MonoBehaviour {
    public UILabel LobbyIDLabel;
    public UITexture playerTextrue;
    public UILabel playerGrade;
    public UILabel playerExp;
    public UIPopupList LobbyList;

    public UIGrid roomGrid;
    public Transform roomTemplete;
    public Player player;
    private Thread getInfo;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<Player>();
        
        ShowPlayerInfo();
        StartCoroutine(GetInfoCoroutine());

        //getInfo = new Thread(new ThreadStart(GetInfo));
        //getInfo.Start();

	}

    void GetInfo()
    {
        
    }
    IEnumerator GetInfoCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        GetLobbyInfo();
        yield return new WaitForSeconds(0.3f);
        GetRoomsInfo();
        yield return new WaitForSeconds(0.3f);
        GetPlayerItemsInfo();
    }

    void GetLobbyInfo()
    {
        object[] lobbyInfo;
        string errInfo;
        if (NetCmdTranslator.Request(out errInfo, out lobbyInfo, BR_Common.NetCmd.GetLobbyInfo, null))
        {
            LobbyList.Clear();
            for (int i = 0; i < lobbyInfo.Length; i++)
            {
                LobbyList.AddItem(string.Format("Lobby{0}: ({1})", i, (byte)lobbyInfo[i]));
            }
        }
        else Debug.LogError("获取Lobby信息错误："+errInfo);
    }

    void GetRoomsInfo()
    {
        object[] roomsInfo;
        string errInfo;
        if (NetCmdTranslator.Request(out errInfo, out roomsInfo, BR_Common.NetCmd.GetRoomsInfo, null))
        {
            //LobbyList.Clear();
            roomGrid.ClearAllChildGO();
            print("获取RoomsInfo成功!");
            for (int i = 0; i < roomsInfo.Length; i++)
            {
                //LobbyList.AddItem(string.Format("Lobby{0}: ({1})", i, (byte)roomsInfo[i]));
                Transform newRoom = Instantiate(roomTemplete) as Transform;
                //newRoom.localScale = Vector3.one;
                newRoom.FindChild("Label").GetComponent<UILabel>().text = string.Format("房间{0} : ({1})",i,roomsInfo[i]);
                roomGrid.AddChild(newRoom);
                print("添加新房间号成功!");
            }
            roomGrid.Reposition();
            foreach (var item in roomGrid.GetChildList())
            {
                item.localScale = Vector3.one;
            }
        }
        else Debug.LogError("获取Rooms信息错误：" + errInfo);
    }

    void GetPlayerItemsInfo()
    {

    }

    private void ShowPlayerInfo()
    {
        if (!player) return;
        LobbyIDLabel.text = "大厅:" + player.roomOrLobbyIn;
        playerTextrue.mainTexture = FindHeadTextrueByID(player.headTextureID);
        playerGrade.text = player.grade.ToString();
        playerExp.text = player.exp.ToString();
        LobbyList.Clear();
        //for (int i = 0; i < player.allLobbyCount; i++)
        //{
        //    LobbyList.AddItem("大厅" + i);
        //}

    }

    private Texture FindHeadTextrueByID(int p)
    {
        Debug.LogError("FindHeadTextrueByID未定义!");
        return null;
    }
	

}
