using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

//Argument List:
//INPUTS:
// show - takes in any number of connections and prints them seperated by a comma


//This function prints all node arguments that are passed in combined and seperated by commans
//Example Arg1: a, Arg2: b, Arg3: c -> a,b,c is printed

public class PrintValue : BaseFunction
{
    public override bool performAction(NodeIdentity node)
    {
        //Check if cannot get input arguments (exit if so)
        if (!node || !node.connectionsIn.ContainsKey("show"))
            return false;

        //TODO: implement on screen output
        string message = "";
        foreach (OutgoingInfo arg in node.connectionsIn["show"])
        {
            if (!arg.isComputed)
            {
                IDE_Output_Controller.instance.AddOutput("show parameter in print node (" + node.id + ") used before it was computed!", OutputMessageType.error);
                return false;
            }
            message += arg.outputVal + ",";
        }

        message = message.Substring(0, message.Length - 1);

        IDE_Output_Controller.instance.AddOutput(message);

        return true;
    }

    public override void getConnectionInfo(out List<string> inputs, out List<string> outputs)
    {
        inputs = new List<string>() {"show"};
        outputs = new List<string>();
    }
}
