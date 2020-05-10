using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes2D;

public class OutgoingInfo
{
    public ConnectionLink sourceIdentity;
    public ConnectionLink sinkIdentity;
    public string sinkParameter;
    public LineRenderer connectionArrow;

    public OutgoingInfo(ConnectionLink sourceIdentity, ConnectionLink sinkIdentity, string sinkParameter, LineRenderer connectionArrow)
    {
        this.sourceIdentity = sourceIdentity;
        this.sinkIdentity = sinkIdentity;
        this.sinkParameter = sinkParameter;
        this.connectionArrow = connectionArrow;
    }
}

public class ConnectionLink : MonoBehaviour
{
    //UI Reference
    public Shape shape;
    public bool isOutput;
    public bool isSelected;
    public NodeIdentity parent;
    public Dictionary<string, OutgoingInfo> outputs = new Dictionary<string, OutgoingInfo>();

    private void Start()
    {
        isSelected = false;
    }

    public OutgoingInfo AddConnection(ConnectionLink newChild)
    {
        //output node can only have one parent
        OutgoingInfo outInfo = new OutgoingInfo(this, newChild, "NA", makeArrow(transform, newChild.transform));
        outputs.Add(newChild.parent.id, outInfo);
        parent.connections.Add(outInfo);
        newChild.parent.connections.Add(outInfo);
        return outInfo;
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
