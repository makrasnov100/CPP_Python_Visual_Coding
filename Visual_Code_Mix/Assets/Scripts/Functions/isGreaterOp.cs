using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Argument List:
//INPUTS: (if any on left is greater than any on right)
// left - the side that if greater will lead to output being true
// right - the side that if greater will lead to output being false
//OUTPUTS:
// out - the selected output of the given two

//Performs > operation
public class IsGreaterValue : BaseFunction
{
    public override bool performAction(NodeIdentity node)
    {
        //Check if cannot get input arguments (exit if so)
        if (!node ||
            !node.connectionsIn.ContainsKey("left") ||
            !node.connectionsIn.ContainsKey("right"))
        {
            IDE_Output_Controller.instance.AddOutput("All input links in isGreater node (" + node.id + ") need to be populated!", OutputMessageType.error);
            return false;
        }

        string finalValueTrue = "true";
        foreach (OutgoingInfo argL in node.connectionsIn["left"])
        {
            if (!argL.isComputed)
            {
                IDE_Output_Controller.instance.AddOutput("condition parameter in select node (" + node.id + ") used before it was computed!", OutputMessageType.error);
                return false;
            }


            double curTermL;
            if (!double.TryParse(argL.outputVal, out curTermL))
            {
                IDE_Output_Controller.instance.AddOutput("IsGreater node (" + node.id + ") was given a non numerical value!", OutputMessageType.error);
                return false;
            }

            foreach (OutgoingInfo argR in node.connectionsIn["right"])
            {
                if (!argR.isComputed)
                {
                    IDE_Output_Controller.instance.AddOutput("condition parameter in select node (" + node.id + ") used before it was computed!", OutputMessageType.error);
                    return false;
                }

                double curTermR;
                if (!double.TryParse(argR.outputVal, out curTermR))
                {
                    IDE_Output_Controller.instance.AddOutput("IsGreater node (" + node.id + ") was given a non numerical value!", OutputMessageType.error);
                    return false;
                }

                if (curTermL < curTermR)
                {
                    finalValueTrue = "false";
                    break;
                }
            }
        }

        setOutLinkValue(node, "out", finalValueTrue);

        return true;
    }

    public override void getConnectionInfo(out List<string> inputs, out List<string> outputs)
    {
        inputs = new List<string>() { "left", "right"};
        outputs = new List<string>() { "out" };
    }
}
