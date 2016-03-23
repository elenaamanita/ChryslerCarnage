using UnityEngine;
using System.Collections;

public class GoBack : MonoBehaviour {
    public string levelname;
    public void GoLevel() {
        Application.LoadLevel(levelname);
    }
}
