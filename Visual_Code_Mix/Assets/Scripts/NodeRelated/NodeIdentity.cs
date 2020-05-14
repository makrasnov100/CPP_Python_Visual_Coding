using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes2D;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEditor.MemoryProfiler;

public enum NodeType{none,data,selection,loop,function}
public enum DataType {noneT, intT, doubleT,stringT,boolT}
public enum FunctionType {none, cpp, python, visual}
public class NodeIdentity : MonoBehaviour
{
    //Instance variables (if no connections then no values here)
    // string - parameter name, list of associated connections
    public Dictionary<string, List<OutgoingInfo>> connectionsIn = new Dictionary<string, List<OutgoingInfo>>();
    public Dictionary<string, List<OutgoingInfo>> connectionsOut = new Dictionary<string, List<OutgoingInfo>>();
    public Dictionary<string, ConnectionLink> linksIn = new Dictionary<string, ConnectionLink>();
    public Dictionary<string, ConnectionLink> linksOut = new Dictionary<string, ConnectionLink>();

    //UI references
    public Shape nodeShape;
    public TMP_Text label;

    //Node settings;
    public string id = "";
    public NodeType nodeType { get; set; }
    public DataType dataType { get; set; }
    public string nodeName { get; set; }
    public string nodeValue { get; set; }
    public FunctionType funcType { get; set; }

    public List<string> inputParameters = new List<string>();
    public List<string> outputParameters = new List<string>();

    private void Start()
    {
        clearNode(0);
        updateNodeState(new List<InputType>(), new List<object>(), new List<string>());
    }

    public void setID(string id)
    {
        this.id = id;
    }

    
    //Level - 1 : clears everything
    //Level - 2 : clears only node type specific items
    public void clearNode(int level) 
    {
        if (level <= 2)
        {
            inputParameters.Clear();
            outputParameters.Clear();
            dataType = DataType.noneT;
            funcType = FunctionType.none;
            nodeName = "";
            nodeValue = "";

            if (level <= 1)
            {
                nodeType = NodeType.none;
            }
        }
    }

    public void updateNodeState(List<InputType> inputType, List<object> inputValue, List<string> inputValueStr)
    {
        if (inputType.Count != inputValue.Count || inputValue.Count != inputValueStr.Count)
        {
            Debug.LogError("Not thesame number of tpyes of nodes ");
            return;
        }

        //Clear node at aproprieate level
        if (nodeType != NodeType.none)
            clearNode(2);   //clear for specific node type
        else
            clearNode(1);   //clear all

        //Get all values
        for (int i = 0; i < inputType.Count; i++)
        {
            if (inputType[i] == InputType.nodeType)
            {
                nodeType = (NodeType)inputValue[i];
            }
            else if (inputType[i] == InputType.dataType)
            {
                dataType = (DataType)inputValue[i];
            }
            else if (inputType[i] == InputType.name)
            {
                if (nodeType != NodeType.function)
                {
                    nodeName = inputValueStr[i];
                }
                else
                {
                    if (Library.instance.functions.ContainsKey(inputValueStr[i]))
                    {
                        nodeName = inputValueStr[i];
                        funcType = FunctionType.visual;
                    }
                }
            }
            else if (inputType[i] == InputType.value)
            {
                nodeValue = inputValueStr[i];
            }
        }

        //Alter the look of the node
        // - node color (based on type)
        if (nodeType == NodeType.data)
            nodeShape.settings.fillColor = new Color(.9f, .7f, 1);
        else if (nodeType == NodeType.function)
            nodeShape.settings.fillColor = Color.cyan;
        else if (nodeType == NodeType.loop)
            nodeShape.settings.fillColor = Color.yellow;
        else if (nodeType == NodeType.selection)
            nodeShape.settings.fillColor = Color.green;
        else if (nodeType == NodeType.none)
        {
            nodeShape.settings.fillColor = new Color(.95f, .95f, .95f);
            clearNode(1);
        }
        else
        {
            Debug.LogError("Node type provided is not supported!");
            nodeShape.settings.fillColor = Color.red;
        }
        // - node text
        string nodeText = " ";
        if (nodeType == NodeType.data)
        {
            if (dataType != DataType.noneT)
            {
                nodeText += dataType.ToString().Substring(0, dataType.ToString().Length-1);
                if (nodeName != "")
                {
                    nodeText += " " + nodeName;
                    if (nodeValue != "")
                    {
                        string visualNodeVal = " " + nodeValue;
                        if (dataType == DataType.stringT)
                            visualNodeVal = " \"" + nodeValue + "\"";

                        nodeText += " = " + visualNodeVal;
                    }
                }
            }
        }
        else if (nodeType == NodeType.function)
        {
            if (nodeName != "")
            {
                nodeText += nodeName;
            }
        }
        label.text = nodeText;

        if (nodeType == NodeType.data && nodeText.Contains(" = "))
        {
            inputParameters.Clear();
            outputParameters.Add("data");
        }
        else if (nodeType == NodeType.function && nodeName != "")
        {
            if(Library.instance && Library.instance.functions.ContainsKey(nodeName))
                Library.instance.functions[nodeName].getConnectionInfo(out inputParameters, out outputParameters);
        }
        UpdateShownConnectionLinks();

        //determine the outputs possible (if any)
        //ad them to the all possible values
    }

    List<ConnectionLink> outputs = new List<ConnectionLink>();
    public void deselectOutputs()
    {
        foreach (ConnectionLink output in outputs)
        {
            output.Deselect();
        }
    }

    //UpdateShownConnectionLinks: shows only those connections that are currently 
    //avaiable for the node type selected

    //Settings for layout
    public float spacingBetweenLinks;
    public float paddingSides;
    //Required input & output link prefabs
    public GameObject inputLink;
    public GameObject outputLink;
    void UpdateShownConnectionLinks()
    {
        //get dimensions
        float startY = .3f;

        //update shown inputs (left side)
        for (int i = 0; i < inputParameters.Count; i++)
        {
            //Modify the position of shown input link
            Vector3 newLinkPos = -Vector3.one;
            newLinkPos.x = -.5f + paddingSides;
            newLinkPos.y = startY - (i * spacingBetweenLinks);

            //input already created
            if (connectionsIn.ContainsKey(inputParameters[i]))
            {
                linksIn[inputParameters[i]].transform.localPosition = newLinkPos;

                //Modify the visibility of link
                linksIn[inputParameters[i]].gameObject.SetActive(true);

                //Modify visibility of all lines attached to the link
                foreach (OutgoingInfo info in connectionsIn[inputParameters[i]])
                {
                    info.connectionArrow.gameObject.SetActive(true);
                }
            }
            else //input not yet created
            {
                //Create input object
                GameObject curInputLink = Instantiate(inputLink);

                //Add to correct parent
                curInputLink.transform.SetParent(gameObject.transform);

                //Setup all variables and position
                ConnectionLink curLinkScript = curInputLink.GetComponent<ConnectionLink>();
                curLinkScript.ConnectionLinkSetup(this, inputParameters[i]);
                curInputLink.transform.localPosition = newLinkPos;

                //Add finished node to list of created parameters (no connections so far)
                connectionsIn.Add(inputParameters[i], new List<OutgoingInfo>());
                linksIn.Add(inputParameters[i], curLinkScript);
            }
        }

        //update shown outputs (right side)
        for (int i = 0; i < outputParameters.Count; i++)
        {
            //Modify the position of shown output link
            Vector3 newLinkPos = -Vector3.one;
            newLinkPos.x = .5f - paddingSides;
            newLinkPos.y = startY - (i * spacingBetweenLinks);

            //output already created
            if (connectionsOut.ContainsKey(outputParameters[i]))
            {
                linksOut[outputParameters[i]].transform.localPosition = newLinkPos;

                //Modify the visibility of link
                linksOut[outputParameters[i]].gameObject.SetActive(true);

                //Modify visibility of all lines attached to the link
                foreach (OutgoingInfo info in connectionsOut[outputParameters[i]])
                {
                    info.connectionArrow.gameObject.SetActive(true);
                }
            }
            else //input not yet created
            {
                //Create input object
                GameObject curOutputLink = Instantiate(outputLink);

                //Add to correct parent
                curOutputLink.transform.SetParent(gameObject.transform);

                //Setup all variables and position
                ConnectionLink curLinkScript = curOutputLink.GetComponent<ConnectionLink>();
                curLinkScript.ConnectionLinkSetup(this, outputParameters[i]);
                curOutputLink.transform.localPosition = newLinkPos;

                //Add finished node to list of created parameters (no connections so far)
                connectionsOut.Add(outputParameters[i], new List<OutgoingInfo>());
                linksOut.Add(outputParameters[i], curLinkScript);
            }
        }

        //Hide all unused input links (TODO make more efficient)
        foreach (KeyValuePair<string, List<OutgoingInfo>> link in connectionsIn)
        {
            if (!inputParameters.Contains(link.Key))
            {
                linksIn[link.Key].gameObject.SetActive(false);
                foreach (OutgoingInfo connection in connectionsIn[link.Key])
                {
                    connection.connectionArrow.gameObject.SetActive(false);
                }
            }
        }

        //Hide all unused output links (TODO make more efficient)
        foreach (KeyValuePair<string, List<OutgoingInfo>> link in connectionsOut)
        {
            if (!outputParameters.Contains(link.Key))
            {
                linksOut[link.Key].gameObject.SetActive(false);
                foreach (OutgoingInfo connection in connectionsOut[link.Key])
                {
                    connection.connectionArrow.gameObject.SetActive(false);
                }
            }
        }
    }

    public override string ToString()
    {
        if (nodeType == NodeType.none)
            return "Empty Node";
        else if (nodeType == NodeType.data && nodeValue != "")
            return nodeValue;
        else 
            return nodeType.ToString().ToUpper()[0] + nodeType.ToString().Substring(1) + " Node";
    }

    public bool computeNodeOutput()
    {
        bool result = false;

        if (Library.instance != null)
        {
            if (nodeType == NodeType.data && Library.instance.functions.ContainsKey("dataNode"))
            {
                result = Library.instance.functions["dataNode"].performAction(this);
            }
            else if (nodeType == NodeType.function && Library.instance.functions.ContainsKey(nodeName))
            {
                result = Library.instance.functions[nodeName].performAction(this);
            }
        }

        return result;
    }

    public bool isReadyForComputaion()
    {
        foreach (string inputLink in inputParameters)
        {
            if (!connectionsIn.ContainsKey(inputLink) ||
                connectionsIn[inputLink] == null)
            {
                return false;
            }
            else
            {
                foreach (OutgoingInfo connection in connectionsIn[inputLink])
                {
                    if (!connection.isComputed)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    public bool isASource()
    {
        foreach (string inputLink in inputParameters)
        {
            if (connectionsIn.ContainsKey(inputLink))
            {
                return false;
            }
        }

        return true;
    }

    public void clearAllComputations()
    {
        foreach (KeyValuePair<string, List<OutgoingInfo>> link in connectionsOut)
        {
            foreach (OutgoingInfo connection in link.Value)
            {
                connection.isComputed = false;
                connection.outputVal = "";
            }
        }
    }
}
