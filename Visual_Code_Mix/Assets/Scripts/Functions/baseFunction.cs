using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSource 
{
    string functionParameter = "";
    string inputMemberName = "";
    public InputSource(string functionParameter, string inputMemberName)
    {
        this.functionParameter = functionParameter;
        this.inputMemberName = inputMemberName;
    }
}

public abstract class BaseFunction
{
    protected Dictionary<InputSource, NodeIdentity> inputNodes = new Dictionary<InputSource, NodeIdentity>();

    public abstract void performAction();

}
