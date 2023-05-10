using UnityEngine;

// Suppongo che tutte le entità siano dei Fighter, e che quindi possano prendere danno
// ! NB: C'era scritto Abstract!

public class Entity : Fighter {

    protected Animator animator;
    protected Rigidbody2D rb;
    protected BoxCollider2D bc;


    protected override void Awake() {

        base.Awake();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bc = GetComponent<BoxCollider2D>();
    }

    public void Enable() {
        this.gameObject.SetActive(true);
    }

    public void Disable() {
        this.gameObject.SetActive(false);
    }

    // Protected, così da permettere alle classi figlie di poterci accedere
    protected virtual void Move(Vector2 movement, float speed) {

        // ! forse da mettere sotto, alla fine di questo metodo? !
        //ChangeAnimation(movement);

        // Se il moviemento non è nullo
        if(movement.x != 0 || movement.y != 0) {
            
            // Muoviamo l'entità
            rb.velocity = new Vector2(movement.x * speed * Time.fixedDeltaTime, movement.y * speed * Time.fixedDeltaTime);
        }
        else { 
            
            // Manteniamo l'entità ferma
            rb.velocity = Vector2.zero;
        }

        // Aggiungo il push-back dell'attacco, se presente
        //rb.velocity += pushDirection;

        // Riduco il push ogni frame, basato sulla recovery speed
        //pushDirection = Vector2.Lerp(pushDirection, Vector2.zero, pushRecoverySpeed);

    }



}
