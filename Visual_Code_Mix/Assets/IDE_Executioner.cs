using Boo.Lang;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using UnityEngine;

public class IDE_Executioner : MonoBehaviour
{
    public Dictionary<string, Dictionary<string,OutgoingInfo>> connectionsOut = new Dictionary<string, Dictionary<string, OutgoingInfo>>();
    public Dictionary<string, Dictionary<string,OutgoingInfo>> connectionsIn = new Dictionary<string, Dictionary<string, OutgoingInfo>>();

    //Singleton
    public static IDE_Executioner instance;
    private void Awake()
    {
        if (instance)
            Destroy(gameObject);
        else
            instance = this;
    }

    //[BEFORE EXECUTION]
    public void addConnection(NodeIdentity source, OutgoingInfo destination)
    {
        string sinkId = destination.sinkIdentity.parent.id;
        //Adds an outgoing connection
        if (!connectionsOut.ContainsKey(source.id))
        {
            connectionsOut.Add(source.id, new Dictionary<string, OutgoingInfo>());
        }
        if (!connectionsOut[source.id].ContainsKey(sinkId))
        {
            connectionsOut[source.id].Add(sinkId, destination);
        }

        //Adds an incoming connection
        if (!connectionsOut.ContainsKey(sinkId))
        {
            connectionsOut.Add(sinkId, new Dictionary<string, OutgoingInfo>());
        }
        if (!connectionsOut[sinkId].ContainsKey(source.id))
        {
            connectionsOut[sinkId].Add(source.id, destination);
        }
    }

    //[DURING EXECUTION]
    public void executeCode()
    {
        //Find all sources

        //Execute until need additional info move to next source

        //Execute program until all nodes visited
    }


    //[DEBUG]
}
