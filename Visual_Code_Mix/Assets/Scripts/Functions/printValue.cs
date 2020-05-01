using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class printValue : BaseFunction
{
    public override void performAction()
    {
        //TODO: implement on screen output
        foreach (KeyValuePair<InputSource, NodeIdentity> node in inputNodes)
        {
            Debug.Log(node.ToString());
        }
    }
}
