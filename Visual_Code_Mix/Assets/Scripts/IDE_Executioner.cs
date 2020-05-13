using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using System.Xml.XPath;
using UnityEngine;

public class IDE_Executioner : MonoBehaviour
{
    //Keeps track of nodes that need to be computed on execution (by node id)
    //This helps to determine if the graph is atleast initially runnable (atleast one source node)
    // - doesnt count nodes that dont have connections
    // - doesnt count nodes that rely on some input
    HashSet<string> startingNodes = new HashSet<string>();

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
    public void updateComputedNodesOnAddedConnection(ConnectionLink source, ConnectionLink sink)
    {
        if (!startingNodes.Contains(source.parent.id))
        {
            startingNodes.Add(source.parent.id);
        }

        if (startingNodes.Contains(sink.parent.id))
        {
            startingNodes.Remove(source.parent.id);
        }
    }

    //[DURING EXECUTION]
    public void executeCode()
    {
        //Debug Graph Info
        Debug.Log("Starting with " + startingNodes.Count + " out of " + IDE_Input_Controller.instance.nodes.Count + " nodes!");
        printConnections("Connections at start - ", startingNodes);

        ////Find all sources (nodes with an outgoing connection but no incoming ones)
        //HashSet<string> completeNodes = new HashSet<string>();
        //Queue<string> sourceNodes = new Queue<string>();
        //foreach(KeyValuePair<string, Dictionary<string, OutgoingInfo>> node in connectionsOut)
        //{
        //    if(!connectionsIn.ContainsKey(node.Key))
        //    {
        //        sourceNodes.Enqueue(node.Key);
        //    }
        //}

        ////Execute program until all nodes visited
        //while (sourceNodes.Count != 0)
        //{
        //    //Execute until need additional info move to next source
        //    string curNode = sourceNodes.Dequeue();
        //    //Skip if node was already executed skip it
        //    if (completeNodes.Contains(curNode))
        //        continue;
        //    //Get node output (will fail if not enough inputs are avaliable)
        //    string curNodeOutput;
        //    bool isNodeSuccess = IDE_Input_Controller.instance.nodes[curNode].computeNodeOutput(out curNodeOutput);
        //    if (isNodeSuccess)  //Node output complete
        //    {
        //        completeNodes.Add(curNode);

        //        //If sink node (no outgoing connections) then skip propagating output
        //        if (!connectionsOut.ContainsKey(curNode)) 
        //            continue;

        //        foreach (KeyValuePair<string, OutgoingInfo> connection in connectionsOut[curNode])
        //        {
        //            connection.Value.outputVal = curNodeOutput;
        //            connection.Value.isComputed = true;

        //            if (!completeNodes.Contains(connection.Key) && isNodeInputCompleted(connection.Key))
        //            {
        //                IDE_Input_Controller.instance.nodes[connection.Key].setArguments(getAllInputs(connection.Key));
        //                sourceNodes.Enqueue(connection.Key);
        //            }
        //        }
        //    }
        //}
    }

    //// isNodeInputCompleted: TRUE if all input nodes connected to this node have their output complete
    //bool isNodeInputCompleted(string nodeId)
    //{
    //    if (!connectionsIn.ContainsKey(nodeId))
    //        return false;

    //    foreach (KeyValuePair<string, OutgoingInfo> connection in connectionsIn[nodeId])
    //    {
    //        if(!connection.Value.isComputed)
    //        {
    //            return false;
    //        }
    //    }

    //    return true;
    //}

    //List<string> getAllInputs(string nodeId)
    //{
    //    //TODO: Error handling here

    //    List<string> result = new List<string>();

    //    foreach (KeyValuePair<string, OutgoingInfo> connection in connectionsIn[nodeId])
    //    {
    //        result.Add(connection.Value.outputVal);
    //    }

    //    return result;
    //}

    //[DEBUG]
    void printConnections(string message, IEnumerable<string> nodes)
    {
        string graphConnections = message + "\n";

        foreach (string node in nodes)
        {
            NodeIdentity curNode = IDE_Input_Controller.instance.nodes[node];
            graphConnections += curNode.id + " outputs - \n";
            foreach (KeyValuePair<string, List<OutgoingInfo>> link in curNode.connectionsOut)
            {
                graphConnections += "     " + link.Key + " -  \n";

                foreach (OutgoingInfo connection in link.Value)
                {
                    graphConnections += "          " + connection.sinkIdentity.paramName + " of " + connection.sinkIdentity.parent.id + "\n";
                }
            }
        }

        Debug.Log(graphConnections);
    }
}
