using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tutorial: https://www.youtube.com/watch?v=zit45k6CUMk

public class ParallaxController : MonoBehaviour {

    private float length;
    private float startPosition;
    public GameObject cam;
    public float parallaxEffectAmount;
    public float offset;


    void Start() {

        startPosition = transform.position.x;

        // Ci da la dimensione (in x) degli sprite
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update() {

        // Quanto lontani ci siamo mossi rispetto alla telecamera
        float temp = cam.transform.position.x * (1 - parallaxEffectAmount);

        // Ci dice di quanto abbiamo mosso il nostro background
        float distance = (cam.transform.position.x * parallaxEffectAmount);

        transform.position = new Vector3(startPosition + distance, transform.position.y, transform.position.z);

        if (temp > startPosition + (length - offset)) {
            startPosition += length;

        }
        else if (temp < startPosition - (length - offset)) {
            startPosition -= length;

        }
    }
}
