using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour {
    
    [SerializeField] private Button btnArrowUp;
    [SerializeField] private Button btnArrowDown;
    [SerializeField] private Button btnArrowLeft;
    [SerializeField] private Button btnArrowRight;


    private void OnEnable() {
        //Register Button Events
        btnArrowLeft.onClick.AddListener(() => buttonCallBack(btnArrowLeft));
        btnArrowDown.onClick.AddListener(() => buttonCallBack(btnArrowDown));
        btnArrowLeft.onClick.AddListener(() => buttonCallBack(btnArrowLeft));
        btnArrowRight.onClick.AddListener(() => buttonCallBack(btnArrowRight));
    }

    private void buttonCallBack(Button buttonPressed) {

        if (buttonPressed == btnArrowLeft) {
            //Your code for button 1
            Debug.Log("Clicked: " + btnArrowLeft.name);
        }

        if (buttonPressed == btnArrowDown) {
            //Your code for button 2
            Debug.Log("Clicked: " + btnArrowDown.name);
        }

        if (buttonPressed == btnArrowLeft) {
            //Your code for button 3
            Debug.Log("Clicked: " + btnArrowLeft.name);
        }

        if (buttonPressed == btnArrowRight) {
            //Your code for button 4
            Debug.Log("Clicked: " + btnArrowRight.name);
        }
    }

}
