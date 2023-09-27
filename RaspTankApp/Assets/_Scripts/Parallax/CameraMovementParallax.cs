using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementParallax : MonoBehaviour {

    public float cameraSpeed = 0.32f;

    void Update() {
        
        // Sposta semplicemente a destra la camera
        this.transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime);

    }

}
