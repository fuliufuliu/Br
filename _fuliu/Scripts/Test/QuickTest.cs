using UnityEngine;
using System.Collections;

public class QuickTest : MonoBehaviour {
    private bool isLoginOk;

	// Use this for initialization
	void Start () {
        GoToLoginScene();
	}

    void GoToLoginScene()
    {
        if(Player.self)
            Destroy(gameObject); 
        else Application.LoadLevel("Login");
        Object.DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        switch (Application.loadedLevel)
        {
            case 0:
                if (!isLoginOk) { Login(); Destroy(gameObject); }
                break;

        }
    }

    private void Login()
    {
        LoginUI.Login("testss", "testss");
    }

}
