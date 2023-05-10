using UnityEngine;

public class SceneInitialization : MonoBehaviour {

    //
    // Managers
    //

    [SerializeField] private bool soundManagerEnabled = false;
    [SerializeField] private bool inputManagerEnabled = false;


    private void Awake() {

        // 
        // Managers
        //

        if(soundManagerEnabled) {
            MasterManager.instance.soundManager.Enable();
        } else {
            MasterManager.instance.soundManager.Disable();
        }

        if(inputManagerEnabled) {
            MasterManager.instance.inputManager.Enable();
        } else {
            MasterManager.instance.inputManager.Disable();
        }

        /*
        if(gameManagerEnabled) {
            MasterManager.instance.gameManager.Enable();
        } else {
            MasterManager.instance.gameManager.Disable();
        }
        */

        
        //
        // Others
        //

       
    }


}
