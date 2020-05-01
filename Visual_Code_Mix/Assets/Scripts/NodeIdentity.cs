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
    public GameObject inputHandle;

    //Node settings;
    public string id = "";
    public NodeType nodeType { get; set; }
    public DataType dataType { get; set; }
    public string nodeName { get; set; }
    public string nodeValue { get; set; }
    public FunctionType funcType { get; set; }
    public BaseFunction functionValue { get; set; }

    private void Start()
    {
        clearNode(0);
        updateNodeState(new List<InputType>(), new List<object>(), new List<string>());
    }

    public void setID(string id)
    {
        this.id = id;
    }

    
    //Level - 1 : clears everything
    //Level - 2 : clears only node tpye specific items
    public void clearNode(int level) 
    {
        if (level <= 2)
        {
            dataType = DataType.noneT;
            funcType = FunctionType.none;
            nodeName = "";
            nodeValue = "";

            if (level <= 1)
            {
                nodeType = NodeType.none;
            }
        }
    }

    public void updateNodeState(List<InputType> inputType, List<object> inputValue, List<string> inputValueStr)
    {
        if (inputType.Count != inputValue.Count || inputValue.Count != inputValueStr.Count)
        {
            Debug.LogError("Not thesame number of tpyes of nodes ");
            return;
        }

        //Clear node at aproprieate level
        if (nodeType != NodeType.none)
            clearNode(2);   //clear for specific node type
        else
            clearNode(1);   //clear all

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
            clearNode(1);
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
        inputHandle.SetActive(nodeType == NodeType.function);

        //determine the outputs posible (if any)
        //ad them to the all possible values
    }

    List<ConnectionLink> outputs = new List<ConnectionLink>();
    public void deselectOutputs()
    {
        foreach (ConnectionLink output in outputs)
        {
            output.Deselect();
        }
    }


    public override string ToString()
    {
        if (nodeType == NodeType.none)
            return "Empty Node";
        else if (nodeType == NodeType.data && nodeValue != "")
            return nodeValue;
        else 
            return nodeType.ToString().ToUpper()[0] + nodeType.ToString().Substring(1) + " Node";
    }
}
