using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes2D;
using TMPro;

public enum NodeType{none,data,selection,loop,function}
public enum DataType {noneT, intT, doubleT,stringT,boolT}
public enum FunctionType {none, cpp, python, visual}
public class NodeIdentity : MonoBehaviour
{
    //UI references
    public Shape nodeShape;
    public TMP_Text label;
    public GameObject outputHandle;

    //Node settings;
    string id = "";
    public NodeType nodeType { get; set; }
    public DataType dataType { get; set; }
    public string nodeName { get; set; }
    public string nodeValue { get; set; }
    public FunctionType funcType { get; set; }

    private void Start()
    {
        clearNode();
    }

    public void setID(string id)
    {
        this.id = id;
    }

    
    public void clearNode() 
    {
        nodeType = NodeType.none;
        dataType = DataType.noneT;
        funcType = FunctionType.none;
        nodeName = "";
        nodeValue = "";
    }

    public void updateNodeState(List<InputType> inputType, List<object> inputValue, List<string> inputValueStr)
    {
        if (inputType.Count != inputValue.Count || inputValue.Count != inputValueStr.Count)
        {
            Debug.LogError("Not thesame number of tpyes of nodes ");
            return;
        }

        //Get all values
        for (int i = 0; i < inputType.Count; i++)
        {
            if (inputType[i] == InputType.nodeType)
            {
                nodeType = (NodeType)inputValue[i];
            }
            else if (inputType[i] == InputType.dataType)
            {
                dataType = (DataType)inputValue[i];
            }
            else if (inputType[i] == InputType.name)
            {
                nodeName = inputValueStr[i];
            }
            else if (inputType[i] == InputType.value)
            {
                nodeValue = inputValueStr[i];
            }
        }

        //Alter the look of the node
        // - node color (based on type)
        if (nodeType == NodeType.data)
            nodeShape.settings.fillColor = new Color(.9f, .7f, 1);
        else if (nodeType == NodeType.function)
            nodeShape.settings.fillColor = Color.cyan;
        else if (nodeType == NodeType.loop)
            nodeShape.settings.fillColor = Color.yellow;
        else if (nodeType == NodeType.selection)
            nodeShape.settings.fillColor = Color.green;
        else if (nodeType == NodeType.none)
        {
            nodeShape.settings.fillColor = new Color(.95f, .95f, .95f);
            clearNode();
        }
        else
        {
            Debug.LogError("Node type provided is not supported!");
            nodeShape.settings.fillColor = Color.red;
        }
        // - node text
        string nodeText = " ";
        if (nodeType == NodeType.data)
        {
            if (dataType != DataType.noneT)
            {
                nodeText += dataType.ToString().Substring(0, dataType.ToString().Length-1);
                if (nodeName != "")
                {
                    nodeText += " " + nodeName;
                    if (nodeValue != "")
                    {
                        string visualNodeVal = " " + nodeValue;
                        if (dataType == DataType.stringT)
                            visualNodeVal = " \"" + nodeValue + "\"";

                        nodeText += " = " + visualNodeVal;
                    }
                }
            }
        }
        label.text = nodeText;
        outputHandle.SetActive(nodeText.Contains(" = "));

        //determine the outputs posible (if any)
        //ad them to the all possible values
    }
}
