
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchManager : MonoBehaviour {

    public void Enable() {
        this.gameObject.SetActive(true);
    }

    public void Disable() {
        this.gameObject.SetActive(false);
    }

}


