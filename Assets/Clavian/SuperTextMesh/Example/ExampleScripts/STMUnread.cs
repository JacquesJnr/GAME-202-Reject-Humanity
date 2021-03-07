using UnityEngine;
using System.Collections;

public class STMUnread : MonoBehaviour {
	public SuperTextMesh stm;
	public KeyCode key = KeyCode.Return;
	
	void Update () {
		if(Input.GetKeyDown(key))
		{
			stm.UnRead();
		}
	}
}
