using UnityEngine;
using System.Collections;



public  class BrObject : MonoBehaviour,IUseEffect {
    public string objectName;
    public string instruction;
    public bool canOverlay = false;



	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void Use()
    {
        throw new System.NotImplementedException();
    }


}
