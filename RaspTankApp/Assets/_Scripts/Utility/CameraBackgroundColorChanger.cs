using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBackgroundColorChanger : MonoBehaviour {

    public byte r;
    public byte g;
    public byte b;


    private void Start() {
        Camera.main.GetComponent<Camera>().backgroundColor = new Color32(r, g, b, 0);
    }

}
