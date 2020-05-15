using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataNode : BaseFunction
{
    public override bool performAction(NodeIdentity node)
    {
        if (!node)
            return false;

        if (node.connectionsIn.ContainsKey("dataIn") && node.connectionsIn["dataIn"].Count > 0)
        {
            if (node.connectionsIn["dataIn"][0].isComputed)
                node.nodeValue = node.connectionsIn["dataIn"][0].outputVal;
            else
            {
                IDE_Output_Controller.instance.AddOutput("dataIn parameter in data node (" + node.id + ") used before it was computed!", OutputMessageType.error);
                return false;
            }
        }

        setOutLinkValue(node, "dataOut", node.nodeValue);

        return true;
    }

    public override void getConnectionInfo(out List<string> inputs, out List<string> outputs)
    {
        inputs = new List<string>() {"dataIn"};
        outputs = new List<string>() {"dataOut"};
    }
}
