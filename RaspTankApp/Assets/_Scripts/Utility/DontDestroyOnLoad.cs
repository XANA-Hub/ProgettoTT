using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Viene usato semplicemente negli oggetti in cui bisogna tenere i riferimenti
// tra i cambi di scena

public class DontDestroyOnLoad : MonoBehaviour {

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

}
