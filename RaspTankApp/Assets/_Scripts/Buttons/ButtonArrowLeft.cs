using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonArrowLeft : Button {
    
    
    public override void OnPointerUp(PointerEventData eventData) {
        base.OnPointerUp(eventData);
        
        Debug.Log(this.gameObject.name + " OnPointerUp invocato!");
    }


    public override void OnPointerDown(PointerEventData eventData) {
        base.OnPointerDown(eventData);

        Debug.Log(this.gameObject.name + " OnPointerDown invocato!");
    }


}
