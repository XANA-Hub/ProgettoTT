using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonCraneGrab : Button {
    
    public override void OnPointerUp(PointerEventData eventData) {

        if(MasterManager.instance.clientTCPManager.GetConnectionState() == ConnectionState.CONNECTED) {
            Debug.Log("ButtonCraneGrab: premuto");
            base.OnPointerUp(eventData);
            MasterManager.instance.clientTCPManager.SendData(RobotCommands.armGrab);
        } else {
            Debug.Log("ButtonCraneGrab: premuto ma non connesso!");
        }
    }



}
