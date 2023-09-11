using UnityEngine;

public class SceneInitialization : MonoBehaviour {

    //
    // Managers
    //

    [SerializeField] private bool audioManagerEnabled = false;
    [SerializeField] private bool clientTCPManagerEnabled = false;
    [SerializeField] private bool videoReceiverManagerEnabled = false;
    [SerializeField] private bool battleManagerEnabled = false;


    private void Awake() {

        // 
        // Managers
        //

        if(audioManagerEnabled) {
            MasterManager.instance.audioManager.Enable();
        } else {
            MasterManager.instance.audioManager.Disable();
        }
        if(clientTCPManagerEnabled) {
            MasterManager.instance.clientTCPManager.Enable();
        } else {
            MasterManager.instance.clientTCPManager.Disable();
        }
        if(videoReceiverManagerEnabled) {
            MasterManager.instance.videoReceiverManager.Enable();
        } else {
            MasterManager.instance.videoReceiverManager.Disable();
        }
        if(battleManagerEnabled) {
            MasterManager.instance.battleManager.Enable();
        } else {
            MasterManager.instance.battleManager.Disable();
        }


        
        //
        // Others
        //

       
    }


}
