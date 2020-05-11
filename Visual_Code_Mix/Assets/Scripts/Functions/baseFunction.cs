using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;


public abstract class BaseFunction
{ 
    public abstract bool performAction(NodeIdentity node, out string output);
}
