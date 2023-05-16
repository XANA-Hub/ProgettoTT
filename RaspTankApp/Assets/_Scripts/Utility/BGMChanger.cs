using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMChanger : MonoBehaviour {
    
    [SerializeField] private string musicName = "Intro";


    private void Start() {
        MasterManager.instance.audioManager.PlayMusic(musicName);
    }


}
