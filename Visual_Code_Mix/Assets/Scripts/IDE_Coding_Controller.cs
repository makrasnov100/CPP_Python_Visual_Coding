using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public enum InputType {nodeType, dataType, name, value}

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
    public bool canEditTarget = true;
    public List<InputType> parameters = new List<InputType>();

    //Target Editing
    public void setTarget(NodeIdentity targetNode, string title)
    {
        canEditTarget = false;
        string curNodeVal = title;
        this.targetNode = targetNode;
        if (targetNode.nodeType == NodeType.none)
        {
            parameters = new List<InputType>() { InputType.nodeType, InputType.dataType, InputType.name, InputType.value };
            if (targetNode.nodeType != NodeType.none)
            {
                curNodeVal += " " + targetNode.nodeType.ToString();
            }
        }
        else 
        {
            Debug.LogError("Unrecognized Node is being edited!");
        }

        simpleCodeInputField.text = curNodeVal;
        canEditTarget = true;
    }

    public void onEditSimpleCodeField()
    {
        if (!canEditTarget)
            return;

        List<string> args = new List<string>(simpleCodeInputField.text.Split(' '));
        args.RemoveAt(0);
        List<InputType> finalArgsTypes = new List<InputType>();
        List<object> finalArgs = new List<object>();

        if (args.Count > parameters.Count)
        {
            Debug.LogError("More arguments than there are node parameters");
        }
        else //argumets passed the first row of tests
        {
            for(int i = 0; i < args.Count; i++)
            {
                //check to see if entered text matchesthe parameter contraints
                bool isPass = true;
                object result = null;
                if (parameters[i] == InputType.nodeType)
                {
                    try 
                    {
                        if(args[i] == "")
                            result = Enum.Parse(typeof(NodeType), "none");
                        else
                            result = Enum.Parse(typeof(NodeType), args[i]);
                    } 
                    catch (ArgumentException e)
                    {
                        isPass = false;
                    }
                }
                else if (parameters[i] == InputType.dataType)
                {
                    isPass = false;
                }
                else if (parameters[i] == InputType.name)
                {
                    isPass = false;
                }
                else if (parameters[i] == InputType.value)
                {
                    isPass = false;
                }
                else
                {
                    isPass = false;
                }

                //If currecnt argument passed then put into argunment list
                if (isPass && result != null)
                {
                    finalArgsTypes.Add(parameters[i]);
                    finalArgs.Add(result);
                }
            }
        }

        targetNode.updateNodeState(finalArgsTypes, finalArgs);
    }
}
