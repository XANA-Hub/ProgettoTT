using UnityEngine;

public class MasterManager : MonoBehaviour {


    // Istanza principale
    public static MasterManager instance { get; private set; }

    //
    // Manager
    //

    public SoundManager soundManager { get; private set; }
    public InputManager inputManager { get; private set; }
    public TouchManager touchManager{ get; private set; }
    //public GameManager gameManager { get; private set; }


    //
    // Metodi
    //

    private void Awake() {

        // Se c'è già un'istanza, la distruggo
        if(instance != null) {

            Destroy(this.gameObject);

            // Tutte le righe sotto prima del return derivano dal GameManager
            //
            //Destroy(player.gameObject);
            //Destroy(characterMenu);
            //Destroy(hud);
            

            return;
        }

        instance = this;


        // Ottengo i vari GameObject figli
        soundManager = GetComponentInChildren<SoundManager>();
        inputManager = GetComponentInChildren<InputManager>();
        touchManager = GetComponentInChildren<TouchManager>();
        //gameManager = GetComponentInChildren<GameManager>();

    }
    


}
