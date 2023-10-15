using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMChanger : MonoBehaviour {
    
    [SerializeField] private string[] musicNames;


    private void Start() {

        if (musicNames.Length > 0) {
            
            // Genera un indice casuale nell'intervallo degli indici dell'array
            int index = Random.Range(0, musicNames.Length);
            MasterManager.instance.audioManager.PlayMusic(musicNames[index]);
        } else {
            Debug.LogError("L'array dei nomi delle canzoni deve essere almeno 1!");
        }
        
    }


}
