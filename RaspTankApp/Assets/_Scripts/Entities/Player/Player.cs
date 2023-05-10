using UnityEngine;


public class Player : Entity {
    
    [Header("Walk Speed")]
    public float walkSpeed; // Velocità di movimento
    private Vector2 movement;

    /*
    private void Update() { 
        
        // Leggo gli input dell'utente
        movement = MasterManager.instance.inputManager.GetMoveDirection();
        
    }

    private void FixedUpdate() {
        
        // Permette di muovere l'entità
        Move(movement, walkSpeed);

    }
    */

   
}
