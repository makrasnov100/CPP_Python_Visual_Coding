using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


//This function prints all node arguments that are passed in combined and seperated by commans
//Example Arg1: a, Arg2: b, Arg3: c -> a,b,c is printed
public class PrintValue : BaseFunction
{
    public override bool performAction(NodeIdentity node, out string output)
    {
        output = "";

        //Check if cannot get input arguments (exit if so)
        if (!node)
            return false;

        //TODO: implement on screen output
        string message = "";
        foreach (string arg in node.arguments)
        {
            message += arg + ",";
        }
        message = message.Substring(0, message.Length - 1);

        Debug.Log(message);

        return true;
    }
}
