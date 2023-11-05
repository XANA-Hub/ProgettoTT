using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonMovementUp : Button {
    
    public override void OnPointerUp(PointerEventData eventData) {

        if(MasterManager.instance.clientTCPManager.GetConnectionState() == ConnectionState.CONNECTED) {
            Debug.Log("ButtonMovementUp: rilasciato");
            base.OnPointerUp(eventData);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.forwardStop);
        } else {
            Debug.Log("ButtonMovementUp: rilasciato ma non connesso!");
        }
    }


    public override void OnPointerDown(PointerEventData eventData) {
        if(MasterManager.instance.clientTCPManager.GetConnectionState() == ConnectionState.CONNECTED) {
            Debug.Log("ButtonMovementUp: premuto");
            base.OnPointerDown(eventData);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.forwardStart);
        } else {
            Debug.Log("ButtonMovementUp: premuto ma non connesso!");
        }
    }


}
