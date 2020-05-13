using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataNode : BaseFunction
{
    public override bool performAction(NodeIdentity node)
    {
        if (!node)
            return false;

        foreach (OutgoingInfo connection in node.connectionsOut["dataVal"])
        {
            connection.isComputed = true;
            connection.outputVal = node.nodeValue;
        }

        return true;
    }

    public override void getConnectionInfo(out List<string> inputs, out List<string> outputs)
    {
        inputs = new List<string>() { "printValues" };
        outputs = new List<string>();
    }
}
