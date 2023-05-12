using UnityEngine;

public class SceneInitialization : MonoBehaviour {

    //
    // Managers
    //

    [SerializeField] private bool audioManagerEnabled = false;
    [SerializeField] private bool touchManagerEnabled = false;
    [SerializeField] private bool inputManagerEnabled = false;


    private void Awake() {

        // 
        // Managers
        //

        if(audioManagerEnabled) {
            MasterManager.instance.audioManager.Enable();
        } else {
            MasterManager.instance.audioManager.Disable();
        }

        if(touchManagerEnabled) {
            MasterManager.instance.touchManager.Enable();
        } else {
            MasterManager.instance.touchManager.Disable();
        }

        if(inputManagerEnabled) {
            MasterManager.instance.inputManager.Enable();
        } else {
            MasterManager.instance.inputManager.Disable();
        }

        
        //
        // Others
        //

       
    }


}
