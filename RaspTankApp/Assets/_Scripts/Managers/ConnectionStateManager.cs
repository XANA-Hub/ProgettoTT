using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionStateManager : MonoBehaviour {


    private void Start() {

        if(!MasterManager.instance.clientTCPManager.isActiveAndEnabled) {
            ChangeConnectionState(Color.red, "CONNECTOR COMPONENT DISABLED");
        }
    }

    private void Update () {

        //
        // Cambio del ConnectionState (indicatore con la lampadina)
        //

        if(MasterManager.instance.clientTCPManager.GetConnectionState() == ConnectionState.NOT_CONNECTED) {
            ChangeConnectionState(Color.red, "NOT CONNECTED");
        } else if(MasterManager.instance.clientTCPManager.GetConnectionState() == ConnectionState.CONNECTION_IN_PROGRESS) {
            ChangeConnectionState(Color.yellow, "CONNECTING Retry: " + MasterManager.instance.clientTCPManager.GetCurrentRetry() + "/" + MasterManager.instance.clientTCPManager.GetMaxConnectionRetries());
        } else if(MasterManager.instance.clientTCPManager.GetConnectionState() == ConnectionState.CONNECTED) {
            ChangeConnectionState(Color.green, "CONNECTED");
            
            // Inizia la battaglia solo se sono nella scena del Robot!
            if(SceneHelper.GetCurrentSceneName() == "Robot") {
                SceneHelper.LoadScene("Battle"); 
            }
            
        } else if(MasterManager.instance.clientTCPManager.GetConnectionState() == ConnectionState.CONNECTION_ABORTED) {
            ChangeConnectionState(Color.red, "CONNECTION FAILED");
        } else {
            ChangeConnectionState(Color.cyan, "UNKNOWN CONNECTION STATE");
        }
        
    }

    private void ChangeConnectionState(Color newColor, string newText) {

        // 0 = IconState
        // 1 = TextState
        MasterManager.instance.connectionStateManager.transform.GetChild(0).gameObject.GetComponent<Image>().color = newColor;
        MasterManager.instance.connectionStateManager.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().SetText(newText);
    }
    
    public void Enable() {
        gameObject.SetActive(true);
    }

    public void Disable() {
        gameObject.SetActive(false);
    }



}
