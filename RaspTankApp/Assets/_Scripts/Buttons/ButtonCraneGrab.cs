using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonCraneGrab : Button {
    
    
    public override void OnPointerUp(PointerEventData eventData) {
        base.OnPointerUp(eventData);
        
        MasterManager.instance.clientTCPManager.SendData(RobotCommands.armGrab);
        Debug.Log(this.gameObject.name + " OnPointerUp invocato!");
    }



}
