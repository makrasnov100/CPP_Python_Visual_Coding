using System.Collections;
using System.Collections.Generic;

//Argument List:
//INPUTS:
// condition - a boolean value if ANY connection is TRUE the first is selected then output1 is selected as final output
// onTrue - output selected as final if the condition is true (only one allowed for now)
// onFalse - output selected as final if the condition is false (only one allowed for now)
//OUTPUTS:
// out - the selected output of the given two

public class SelectValue : BaseFunction
{
    public override bool performAction(NodeIdentity node)
    {
        //Check if cannot get input arguments (exit if so)
        if (!node ||
            !node.connectionsIn.ContainsKey("condition") ||
            !node.connectionsIn.ContainsKey("onTrue") ||
            !node.connectionsIn.ContainsKey("onFalse"))
        {
            IDE_Output_Controller.instance.AddOutput("All input links in select node (" + node.id + ") need to be populated!", OutputMessageType.error);
            return false;
        }

        bool finalValueTrue = false;
        foreach (OutgoingInfo arg in node.connectionsIn["condition"])
        {
            if (!arg.isComputed)
            {
                IDE_Output_Controller.instance.AddOutput("condition parameter in select node (" + node.id + ") used before it was computed!", OutputMessageType.error);
                return false;
            }

            bool curTerm;
            if (!bool.TryParse(arg.outputVal, out curTerm))
            {
                IDE_Output_Controller.instance.AddOutput("Sum node (" + node.id + ") was given a non numerical value!", OutputMessageType.error);
                return false;
            }

            if (curTerm)
            {
                finalValueTrue = true;
                break;  //haha lazy evaluation, check (sort of)
            }
        }

        if (finalValueTrue)
        {
            setOutLinkValue(node, "out", node.connectionsIn["onTrue"][0].outputVal);
        }
        else
        {
            setOutLinkValue(node, "out", node.connectionsIn["onFalse"][0].outputVal);
        }

        return true;
    }

    public override void getConnectionInfo(out List<string> inputs, out List<string> outputs)
    {
        inputs = new List<string>() { "condition", "onTrue", "onFalse"};
        outputs = new List<string>() { "out" };
    }
}
