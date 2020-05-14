using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum OutputMessageType {info, warning, error}

public class OutputMessageIdentity : MonoBehaviour
{
    //UI references
    public TMP_Text uiHandle;

    public void SetupMessage(string msg, OutputMessageType type)
    {
        //set text
        uiHandle.text = msg;

        //set color
        if (type == OutputMessageType.warning)
            uiHandle.color = Color.yellow;
        else if (type == OutputMessageType.error)
            uiHandle.color = Color.red;
        else
            uiHandle.color = Color.green;

        //reset scale
        gameObject.transform.localScale = Vector3.one;

    }
}
