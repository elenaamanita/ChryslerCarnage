using UnityEngine;
using System.Collections;

public class TestButton : MonoBehaviour {
  

	public void Play () 
    {
        Application.LoadLevel(1);
    }

    public void Continue()
    {
        Application.LoadLevel(1);
    }
	
	public void Quit () 
    {
        Application.Quit();
	}

    public void NewGame()
    {
        Application.LoadLevel(1);
    }

    public void Credits()
    {
        // show credits
        Application.LoadLevel(2);
        //var credits = GameObject.Find("Cred").transform;
       // credits.position = new Vector3(400, credits.position.y, 0);
    }
}
