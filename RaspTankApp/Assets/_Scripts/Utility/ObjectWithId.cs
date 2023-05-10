using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectWithId : MonoBehaviour {

    [Header("ID used to save if the object has been already get or not")]
    public string id;
    

    [ContextMenu("Generate GUID for ID")]
    protected virtual void GenerateGuid() {
        id = System.Guid.NewGuid().ToString();
    }

}
