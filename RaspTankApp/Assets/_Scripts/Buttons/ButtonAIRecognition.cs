using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAIRecognition : Button {
    
    public override void OnPointerUp(PointerEventData eventData) {

        if(MasterManager.instance.clientTCPManager.GetConnectionState() == ConnectionState.CONNECTED) {
            Debug.Log("ButtonAIRecognition: premuto");
            base.OnPointerUp(eventData);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.aiIdentifyCurrent);
        } else {
            Debug.Log("ButtonAIRecognition: premuto ma non connesso!");
        }
    }


}
