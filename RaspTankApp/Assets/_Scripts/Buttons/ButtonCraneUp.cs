using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonCraneUp : Button {
    
    
    public override void OnPointerUp(PointerEventData eventData) {
        base.OnPointerUp(eventData);
        
        MasterManager.instance.clientTCPManager.SendData(RobotCommands.armUp);
        Debug.Log(this.gameObject.name + " OnPointerUp invocato!");
    }



}
