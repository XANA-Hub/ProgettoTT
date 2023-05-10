using UnityEngine;
using UnityEngine.InputSystem;


//
// COME FUNZIONA
//

// Bisogna fare un metodo sotto per ogni pulsante che il giocatore può premere (esempio bottone "Attacco")

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour {

    private Vector2 moveDirection = Vector2.zero;
    private float zoom = 0f;
    private bool interactPressed = false;
    private bool submitPressed = false;
    private bool attackPressed = false;


    
    public void Enable() {
        this.gameObject.SetActive(true);
    }

    public void Disable() {
        this.gameObject.SetActive(false);
    }


    // METODI REGISTRAZIONE PRESSIONE DEI BOTTONI
    // Questi metodi servono solo per capire se il bottone è stato premuto dall'utente
    // NON usarli al di fuori di questo codice!

    public void MovePressed(InputAction.CallbackContext context) {

        if (context.performed) {
            moveDirection = context.ReadValue<Vector2>();
        }
        else if (context.canceled) {
            moveDirection = context.ReadValue<Vector2>();
        } 
    }

    // Aggiunto da me
    public void ZoomPressed(InputAction.CallbackContext context) {

        if(context.performed) {
            zoom = context.ReadValue<float>();
        }
        else if(context.canceled) {
            zoom = context.ReadValue<float>();
        }

    }

    public void InteractButtonPressed(InputAction.CallbackContext context) {

        if (context.performed) {
            interactPressed = true;
        }
        else if (context.canceled) {
            interactPressed = false;
        } 
    }
    public void SubmitPressed(InputAction.CallbackContext context) {

        if (context.performed) {
            submitPressed = true;
        }
        else if (context.canceled) {
            submitPressed = false;
        } 
    }

    public void AttackPressed(InputAction.CallbackContext context) {

        if (context.performed) {
            attackPressed = true;
        }
        else if (context.canceled) {
            attackPressed = false;
        } 
    }


    // METODI GET DEI VALORI
    // Questi metodi restituiscono i valori associati ai bottoni

    public Vector2 GetMoveDirection() {
        return moveDirection;
    }

    
    public float GetZoomAmount() {
        return zoom;
    }

    // METODI GET DEI BOTTONI (SE SONO STATI PREMUTI O NO)
    // Servono per sapere se il bottone corrispondente all'azione è stato premuto oppure no

    // for any of the below 'Get' methods, if we're getting it then we're also using it,
    // which means we should set it to false so that it can't be used again until actually
    // pressed again.

    public bool GetAttackPressed() {
        bool result = attackPressed;
        attackPressed = false;

        return result;
    }

    public bool GetInteractPressed() {
        bool result = interactPressed;
        interactPressed = false;

        return result;
    }

    public bool GetSubmitPressed() {
        bool result = submitPressed;
        submitPressed = false;

        return result;
    }

    public void RegisterSubmitPressed() {
        submitPressed = false;
    }

}
