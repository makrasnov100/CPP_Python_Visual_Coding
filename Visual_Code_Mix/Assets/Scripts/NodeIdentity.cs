using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType{none,data,selection,loop,function}
public enum DataType {none, intT, doubleT,stringT,boolT}
public enum FunctionType {none, cpp, python, visual}
public class NodeIdentity : MonoBehaviour
{
    //Node settings;
    string id = "";
    NodeType nodeType = NodeType.none;
    DataType dataType = DataType.none;
    FunctionType funcType = FunctionType.none;

    public void setID(string id)
    {
        this.id = id;
    }

    public void clearNode() {}

    public void updateNodeState(Dictionary<string, string> inputs)
    {
        //Perform variable changes
        foreach (KeyValuePair<string, string> argument in inputs)
        {
            //look through arguments see if valid inputs update all values
            


        }
        //Perform apearences 

        //determine the outputs posible (if any)
            //ad them to the all possible values
    }
}
