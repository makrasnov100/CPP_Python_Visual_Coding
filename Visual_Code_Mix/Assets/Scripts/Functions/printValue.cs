using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

//Argument List:
//INPUTS:
// prefix - words to put before the body of concatonated inputs
// body - takes in any number of connections and prints them seperated by a comma
// suffix - words to put after the body of concatonated inputs

//This function prints all node arguments that are passed in combined and seperated by commans
//Example Arg1: a, Arg2: b, Arg3: c -> a,b,c is printed

public class PrintValue : BaseFunction
{
    public override bool performAction(NodeIdentity node)
    {
        //Check if cannot get input arguments (exit if so)
        if (!node || !node.connectionsIn.ContainsKey("body") || node.connectionsIn["body"].Count == 0)
        {
            IDE_Output_Controller.instance.AddOutput("No body parameter in print node (" + node.id + ")", OutputMessageType.error);
            return false;
        }

        //TODO: implement on screen output
        string message = "";
        foreach (OutgoingInfo arg in node.connectionsIn["body"])
        {
            if (!arg.isComputed)
            {
                IDE_Output_Controller.instance.AddOutput("body parameter in print node (" + node.id + ") used before it was computed!", OutputMessageType.error);
                return false;
            }
            message += arg.outputVal + ",";
        }

        //perform final triming and additions
        message = message.Substring(0, message.Length - 1);

        if (node.connectionsIn.ContainsKey("prefix") && node.connectionsIn["prefix"].Count != 0 && node.connectionsIn["prefix"][0].isComputed)
            message = node.connectionsIn["prefix"][0].outputVal + message;

        if (node.connectionsIn.ContainsKey("suffix") && node.connectionsIn["suffix"].Count != 0 && node.connectionsIn["suffix"][0].isComputed)
            message = message + node.connectionsIn["suffix"][0].outputVal;


        IDE_Output_Controller.instance.AddOutput(message);

        return true;
    }

    public override void getConnectionInfo(out List<string> inputs, out List<string> outputs)
    {
        inputs = new List<string>() { "prefix","body","suffix"};
        outputs = new List<string>();
    }
}
