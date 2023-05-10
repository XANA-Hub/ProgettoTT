
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchManager : MonoBehaviour {

    private PlayerInput playerInput;
    private InputAction touchPositionAction;
    private InputAction touchPressAction;

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();

        touchPressAction = playerInput.actions["TouchPress"];
        touchPositionAction = playerInput.actions["TouchPosition"];
    }

    private void OnEnable() {
        touchPressAction.performed += TouchPressed;
    }

    private void OnDisable() {
        touchPressAction.performed -= TouchPressed;
    }

    public void Enable() {
        this.gameObject.SetActive(true);
    }

    public void Disable() {
        this.gameObject.SetActive(false);
    }

    private void TouchPressed(InputAction.CallbackContext context) {
        
        /*
        Vector3 position = Camera.main.ScreenToWorldPoint(touchPositionAction.ReadValue<Vector2>());
        position.z = player.transform.position.z;

        player.transform.position = position;
        */

    }

    private void Update() {

        /*
        if(touchPressAction.WasPerformedThisFrame()) {
            Vector3 position = Camera.main.ScreenToWorldPoint(touchPositionAction.ReadValue<Vector2>());
            position.z = player.transform.position.z;
            player.transform.position = position;
        }
        */

    }
}
