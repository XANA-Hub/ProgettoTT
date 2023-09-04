using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonMovementRight : Button {
    
    
    public override void OnPointerUp(PointerEventData eventData) {
        base.OnPointerUp(eventData);
        
        MasterManager.instance.clientTCPManager.SendData(RobotCommands.rotateRightStop);
        Debug.Log(this.gameObject.name + " OnPointerUp invocato!");
    }


    public override void OnPointerDown(PointerEventData eventData) {
        base.OnPointerDown(eventData);

        MasterManager.instance.clientTCPManager.SendData(RobotCommands.rotateRightStart);
        Debug.Log(this.gameObject.name + " OnPointerDown invocato!");
    }


}
