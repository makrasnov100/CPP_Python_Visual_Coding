using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes2D;

public enum NodeType{none,data,selection,loop,function}
public enum DataType {noneT, intT, doubleT,stringT,boolT}
public enum FunctionType {none, cpp, python, visual}
public class NodeIdentity : MonoBehaviour
{
    //UI references
    public Shape nodeShape;

    //Node settings;
    string id = "";
    public NodeType nodeType { get; set; }
    public DataType dataType { get; set; }
    public string nodeName { get; set; }
    public FunctionType funcType { get; set; }

    private void Start()
    {
        nodeType = NodeType.none;
        dataType = DataType.noneT;
        funcType = FunctionType.none;
    }

    public void setID(string id)
    {
        this.id = id;
    }

    
    public void clearNode() {}

    public void updateNodeState(List<InputType> inputType, List<object> inputValue)
    {
        if (inputType.Count != inputValue.Count)
        {
            Debug.LogError("Not thesame number of tpyes of nodes ");
            return;
        }

        for (int i = 0; i < inputType.Count; i++)
        {
            if (inputType[i] == InputType.nodeType)
            {
                nodeType = (NodeType) inputValue[i];
                if (nodeType == NodeType.data)
                    nodeShape.settings.fillColor = new Color(.9f, .7f, 1);
                else if (nodeType == NodeType.function)
                    nodeShape.settings.fillColor = Color.cyan;
                else if (nodeType == NodeType.loop)
                    nodeShape.settings.fillColor = Color.yellow;
                else if (nodeType == NodeType.selection)
                    nodeShape.settings.fillColor = Color.green;
                else if (nodeType == NodeType.none)
                    nodeShape.settings.fillColor = new Color(.95f, .95f, .95f);
                else
                {
                    Debug.LogError("Node type provided is not supported!");
                    nodeShape.settings.fillColor = Color.red;
                }
            }
        }

        //determine the outputs posible (if any)
            //ad them to the all possible values
    }
}
