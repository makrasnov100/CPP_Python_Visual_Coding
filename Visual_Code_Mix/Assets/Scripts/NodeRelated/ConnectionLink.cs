using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes2D;
using JetBrains.Annotations;
using UnityEngine.UI;
using TMPro;

public class OutgoingInfo
{
    public ConnectionLink sourceIdentity;
    public ConnectionLink sinkIdentity;

    public LineRenderer connectionArrow;

    public string outputVal;
    public bool isComputed = false;

    public OutgoingInfo(ConnectionLink sourceIdentity, ConnectionLink sinkIdentity, LineRenderer connectionArrow)
    {
        this.sourceIdentity = sourceIdentity;
        this.sinkIdentity = sinkIdentity;
        this.connectionArrow = connectionArrow;
    }
}

public class ConnectionLink : MonoBehaviour
{
    //UI Reference
    public Shape shape;
    public TMP_Text linkName;

    //Instance variables
    public bool isOutput;
    public bool isSelected;
    public string paramName;
    public NodeIdentity parent;

    private void Start()
    {
        isSelected = false;
    }

    public void ConnectionLinkSetup(NodeIdentity parent, string paramName)
    {
        this.parent = parent;
        this.paramName = paramName;
        linkName.text = paramName;
    }

    public void AddConnection(ConnectionLink newChild)
    {
        //Create and store outgoing information
        OutgoingInfo outInfo = new OutgoingInfo(this, newChild, makeArrow(transform, newChild.transform));

        parent.connectionsOut[paramName].Add(outInfo);
        newChild.parent.connectionsIn[newChild.paramName].Add(outInfo);

        //Update child node connections since a conditional output may apear if this connection is hooked up
        newChild.parent.UpdateShownConnectionLinks();
    }

    public void OnMouseDown()
    {
        if (IDE_Input_Controller.instance != null && isOutput)
        {
            if (IDE_Input_Controller.instance.sourceNode != null && IDE_Input_Controller.instance.sourceNode != this)
            {
                IDE_Input_Controller.instance.sourceNode.Deselect();
            }

            if (isSelected)
            {
                IDE_Input_Controller.instance.sourceNode = null;
                Deselect();
            }
            else
            {
                IDE_Input_Controller.instance.sourceNode = this;
                Select();
            }
        }
    }

    public void Deselect()
    {
        isSelected = false;
        shape.settings.fillColor = new Color(.8f, .18f, .21f, 1f);
    }
    public void Select()
    {
        isSelected = true;
        shape.settings.fillColor = new Color(.18f, .24f, .8f, 1f);
    }

    public Material lineMaterial;
    public LineRenderer makeArrow(Transform outLink, Transform inLink)
    {
        GameObject line = new GameObject("Connection");
        line.transform.SetParent(this.transform);
        LineRenderer lr = line.AddComponent<LineRenderer>();
        Vector3 endPos = new Vector3(inLink.position.x, inLink.position.y, -1);
        Vector3 startPos = new Vector3(outLink.position.x, outLink.position.y, -1);
        lr.SetPositions(new Vector3[] { endPos, startPos });
        lr.startColor = Color.red;
        lr.endColor = Color.green;
        lr.startWidth = .15f;
        lr.endWidth = .15f;
        lr.material = lineMaterial;
        lr.sortingOrder = 10000;
        return lr;
    }

    //Create coroutine that draws a line between mouse and selected output node?
}
