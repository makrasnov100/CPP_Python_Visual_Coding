using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using System.Xml.XPath;
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
        if (!connectionsIn.ContainsKey(sinkId))
        {
            connectionsIn.Add(sinkId, new Dictionary<string, OutgoingInfo>());
        }
        if (!connectionsIn[sinkId].ContainsKey(source.id))
        {
            connectionsIn[sinkId].Add(source.id, destination);
        }
    }

    //[DURING EXECUTION]
    public void executeCode()
    {
        //Debug adjacency list
        //printGraph();

        //Find all sources (nodes with an outgoing connection but no incoming ones)
        HashSet<string> completeNodes = new HashSet<string>();
        Queue<string> sourceNodes = new Queue<string>();
        foreach(KeyValuePair<string, Dictionary<string, OutgoingInfo>> node in connectionsOut)
        {
            if(!connectionsIn.ContainsKey(node.Key))
            {
                sourceNodes.Enqueue(node.Key);
            }
        }

        //Execute program until all nodes visited
        while (sourceNodes.Count != 0)
        {
            //Execute until need additional info move to next source
            string curNode = sourceNodes.Dequeue();
            //Skip if node was already executed skip it
            if (completeNodes.Contains(curNode))
                continue;
            //Get node output (will fail if not enough inputs are avaliable)
            string curNodeOutput;
            bool isNodeSuccess = IDE_Input_Controller.instance.nodes[curNode].computeNodeOutput(out curNodeOutput);
            if (isNodeSuccess)  //Node output complete
            {
                completeNodes.Add(curNode);

                //If sink node (no outgoing connections) then skip propagating output
                if (!connectionsOut.ContainsKey(curNode)) 
                    continue;

                foreach (KeyValuePair<string, OutgoingInfo> connection in connectionsOut[curNode])
                {
                    connection.Value.outputVal = curNodeOutput;
                    connection.Value.isComputed = true;

                    if (!completeNodes.Contains(connection.Key) && isNodeInputCompleted(connection.Key))
                    {
                        IDE_Input_Controller.instance.nodes[connection.Key].setArguments(getAllInputs(connection.Key));
                        sourceNodes.Enqueue(connection.Key);
                    }
                }
            }
        }
    }

    // isNodeInputCompleted: TRUE if all input nodes connected to this node have their output complete
    bool isNodeInputCompleted(string nodeId)
    {
        if (!connectionsIn.ContainsKey(nodeId))
            return false;

        foreach (KeyValuePair<string, OutgoingInfo> connection in connectionsIn[nodeId])
        {
            if(!connection.Value.isComputed)
            {
                return false;
            }
        }

        return true;
    }

    List<string> getAllInputs(string nodeId)
    {
        //TODO: Error handling here

        List<string> result = new List<string>();

        foreach (KeyValuePair<string, OutgoingInfo> connection in connectionsIn[nodeId])
        {
            result.Add(connection.Value.outputVal);
        }

        return result;
    }

    //[DEBUG]
    void printGraph()
    {
        string graphConnections = "Graph Connections:\n";

        foreach (KeyValuePair<string, Dictionary<string, OutgoingInfo>> node in connectionsOut)
        {
            graphConnections += node.Key + " connected to - \n     ";
            foreach (KeyValuePair<string, OutgoingInfo> connection in node.Value)
            {
                graphConnections += connection + ",";
            }
        }

        Debug.Log(graphConnections);
    }
}
