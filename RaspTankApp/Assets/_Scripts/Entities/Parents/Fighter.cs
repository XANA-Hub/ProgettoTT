using UnityEngine;



// Classe che permette di gestire le entità che prendono danno
public class Fighter : MonoBehaviour {

    [Header("Entity Stats")]
    public int hp = 10;
    public int maxHp = 10;

    // Push
    protected Vector2 pushDirection;

    protected SpriteRenderer sr;


    protected virtual void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    protected virtual void Death() {
        
        MasterManager.instance.soundManager.PlaySound("EntityDeath");
        Debug.Log("L'entità " + this.name + " è morta!");
    }


}
