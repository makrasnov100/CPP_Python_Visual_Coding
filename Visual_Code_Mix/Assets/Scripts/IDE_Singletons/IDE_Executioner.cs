using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using System.Xml.XPath;
using UnityEditor.Experimental.GraphView;
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
        if (!startingNodes.Contains(source.parent.id) && source.parent.isASource())
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

        //Show Output Window on every Run
        IDE_Output_Controller.instance.OpenOutput();

        //Tell user if his her graph is not executable 
        if (startingNodes.Count == 0)
        {
            IDE_Output_Controller.instance.AddOutput("No source nodes, nothing to execute!", OutputMessageType.warning);
            return;
        }

        //Begin by quing all source nodes for compuatation, store completed nodes to prevent recomputing them
        HashSet<string> completeNodes = new HashSet<string>();
        Queue<string> sourceNodes = new Queue<string>(startingNodes);

        //Execute program until all conected nodes visited or a fault reached
        while (sourceNodes.Count != 0)
        {
            //Execute until need additional info move to next source
            string curNodeId = sourceNodes.Dequeue();
            //Skip if node was already executed
            if (completeNodes.Contains(curNodeId))
                continue;

            //Skip if node cannot be found
            NodeIdentity curNode = null;
            if (!IDE_Input_Controller.instance.nodes.ContainsKey(curNodeId))
            {
                IDE_Output_Controller.instance.AddOutput("A Node was not found in all nodes dictionary! What happened to it?", OutputMessageType.warning);
                continue;
            }
            else
                curNode = IDE_Input_Controller.instance.nodes[curNodeId];

            //Get node output (will fail if not enough inputs are avaliable)
            bool isNodeSuccess = curNode.computeNodeOutput();
            if (isNodeSuccess)  //Node output complete
            {
                completeNodes.Add(curNodeId);

                //Check all output nodes,
                // if their incoming connections are satisfied add to queue of source nodes
                HashSet<string> curCheckNodes = new HashSet<string>();
                foreach (string activeOutputArg in curNode.outputParameters)
                {
                    if (curNode.connectionsOut.ContainsKey(activeOutputArg))
                    {
                        foreach (OutgoingInfo connection in curNode.connectionsOut[activeOutputArg])
                        {
                            if (!curCheckNodes.Contains(connection.sinkIdentity.parent.id) &&
                               connection.sinkIdentity.parent.isReadyForComputaion())
                            {
                                curCheckNodes.Add(connection.sinkIdentity.parent.id);
                                sourceNodes.Enqueue(connection.sinkIdentity.parent.id);
                            }
                        }
                    }
                }
            }
            else
            {
                IDE_Output_Controller.instance.AddOutput(curNodeId + " - node has failed to compute!", OutputMessageType.error);
            }
        }

        //Reset all completed nodes output state
        foreach (string nodeId in completeNodes)
        {
            IDE_Input_Controller.instance.nodes[nodeId].clearAllComputations();
        }
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
