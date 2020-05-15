using System.Collections;
using System.Collections.Generic;

//Argument List:
//INPUTS:
// add - takes in any number of connections and sums them up (need to be numerical values)
//OUTPUTS:
// sumFlt - the sum with decimals saved
// sumInt - the sum cast to an int at the end

public class SumValue : BaseFunction
{
    public override bool performAction(NodeIdentity node)
    {
        //Check if cannot get input arguments (exit if so)
        if (!node || !node.connectionsIn.ContainsKey("add"))
            return false;

        double sumOutDouble = 0;
        int sumOutInt = 0;
        foreach (OutgoingInfo arg in node.connectionsIn["add"])
        {
            if (!arg.isComputed)
            {
                IDE_Output_Controller.instance.AddOutput("add parameter in sum node (" + node.id + ") used before it was computed!", OutputMessageType.error);
                return false;
            }

            double curTerm;
            if (!double.TryParse(arg.outputVal, out curTerm))
            {
                IDE_Output_Controller.instance.AddOutput("Sum node ("+node.id+") was given a non numerical value!", OutputMessageType.error);
                return false;
            }
            sumOutDouble += curTerm;
        }

        sumOutInt = (int)sumOutDouble;
        setOutLinkValue(node, "sumFlt", sumOutDouble.ToString());
        setOutLinkValue(node, "sumInt", sumOutInt.ToString());

        return true;
    }

    public override void getConnectionInfo(out List<string> inputs, out List<string> outputs)
    {
        inputs = new List<string>() { "add" };
        outputs = new List<string>() { "sumFlt", "sumInt"};
    }
}
