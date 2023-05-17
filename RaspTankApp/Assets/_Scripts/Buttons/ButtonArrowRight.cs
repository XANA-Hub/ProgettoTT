using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonArrowRight : Button {
    
    
    public override void OnPointerUp(PointerEventData eventData) {
        base.OnPointerUp(eventData);
        
        MasterManager.instance.clientTCPManager.SendData("ID:001;TYPE:Movement;BODY:RotateRight_Stop");
        Debug.Log(this.gameObject.name + " OnPointerUp invocato!");
    }


    public override void OnPointerDown(PointerEventData eventData) {
        base.OnPointerDown(eventData);

        MasterManager.instance.clientTCPManager.SendData("ID:001;TYPE:Movement;BODY:RotateRight_Start");
        Debug.Log(this.gameObject.name + " OnPointerDown invocato!");
    }


}
