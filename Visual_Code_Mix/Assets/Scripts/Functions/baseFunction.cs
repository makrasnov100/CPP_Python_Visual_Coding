using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;


public abstract class BaseFunction
{
    public abstract void getConnectionInfo(out List<string> inputs, out List<string> outputs);

    public abstract bool performAction(NodeIdentity node);

    protected void setOutLinkValue(NodeIdentity node, string parameterName, string value)
    {
        foreach (OutgoingInfo connection in node.connectionsOut[parameterName])
        {
            connection.isComputed = true;
            connection.outputVal = value;
        }
    }
}
