using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMChanger : MonoBehaviour {

    public string musicName;

    private void Start() {
        Debug.Log($"CIAOOO");
        MasterManager.instance.soundManager.PlayMusic(musicName);
    }

}
