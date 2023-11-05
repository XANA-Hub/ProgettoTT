using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonMovementRight : Button {
    
    public override void OnPointerUp(PointerEventData eventData) {

        if(MasterManager.instance.clientTCPManager.GetConnectionState() == ConnectionState.CONNECTED) {
            Debug.Log("ButtonMovementRight: rilasciato");
            base.OnPointerUp(eventData);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.rotateRightStop);
        } else {
            Debug.Log("ButtonMovementRight: rilasciato ma non connesso!");
        }
    }


    public override void OnPointerDown(PointerEventData eventData) {
        
        if(MasterManager.instance.clientTCPManager.GetConnectionState() == ConnectionState.CONNECTED) {
            Debug.Log("ButtonMovementRight: premuto");
            base.OnPointerDown(eventData);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.rotateRightStart);
        } else {
            Debug.Log("ButtonMovemenRight: premuto ma non connesso!");
        }
    }


}
