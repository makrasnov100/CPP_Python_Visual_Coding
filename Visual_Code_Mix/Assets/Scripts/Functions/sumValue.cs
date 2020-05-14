﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

//Argument List:
//INPUTS:
// sumValues - takes in any number of connections and sums them up (need to be numerical values)
//OUTPUTS:
// sumOutFloat - the sum with decimals saved
// sumOutInt - the sum cast to an int at the end

public class SumValue : BaseFunction
{
    public override bool performAction(NodeIdentity node)
    {
        //Check if cannot get input arguments (exit if so)
        if (!node || !node.connectionsIn.ContainsKey("sumValues"))
            return false;

        double sumOutDouble = 0;
        int sumOutInt = 0;
        foreach (OutgoingInfo arg in node.connectionsIn["sumValues"])
        {
            if (!arg.isComputed)
            {
                Debug.LogError("Connection value used before it was computed!");
                return false;
            }

            double curTerm;
            if (!double.TryParse(arg.outputVal, out curTerm))
            {
                Debug.LogError("Sum node was given a non numerical value!");
                return false;
            }
            sumOutDouble += curTerm;
        }

        sumOutInt = (int)sumOutDouble;
        setOutLinkValue(node, "sumOutputDouble", sumOutDouble.ToString());
        setOutLinkValue(node, "sumOutputInt", sumOutInt.ToString());

        return true;
    }

    public override void getConnectionInfo(out List<string> inputs, out List<string> outputs)
    {
        inputs = new List<string>() { "sumValues" };
        outputs = new List<string>() { "sumOutputDouble", "sumOutputInt"};
    }
}
