using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IDE_Coding_Controller : MonoBehaviour
{
    static IDE_Coding_Controller instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    TMP_InputField simpleCodeInputField;
    NodeIdentity targetNode;
    List<string> parameters;

    //Target Editing
    void setTarget(NodeIdentity targetNode, List<string> parameters, string title)
    {
        simpleCodeInputField.text = "";
        this.targetNode = targetNode;
        parameters = this.parameters;
    }

    void onEditSimpleCodeField()
    {

    }
}
