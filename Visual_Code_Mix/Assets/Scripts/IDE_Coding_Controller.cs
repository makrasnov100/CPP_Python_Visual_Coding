using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IDE_Coding_Controller : MonoBehaviour
{
    static public IDE_Coding_Controller instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    public TMP_InputField simpleCodeInputField;
    public NodeIdentity targetNode;
    List<string> parameters;

    //Target Editing
    public void setTarget(NodeIdentity targetNode, List<string> parameters, string title)
    {
        simpleCodeInputField.text = "";
        this.targetNode = targetNode;
        parameters = this.parameters;
    }

    public void onEditSimpleCodeField()
    {

    }
}
