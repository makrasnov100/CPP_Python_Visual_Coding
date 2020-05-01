using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes2D;

public class OutgoingInfo
{
    NodeIdentity sinkIdentity;
    string sinkParameter;
    GameObject connectionArrow;

    public OutgoingInfo(NodeIdentity sinkIdentity, string sinkParameter, GameObject connectionArrow)
    {
        this.sinkIdentity = sinkIdentity;
        this.sinkParameter = sinkParameter;
        this.connectionArrow = connectionArrow;
    }
}

public class ConnectionLink : MonoBehaviour
{
    //UI Reference
    public Shape shape;

    public bool isSelected;
    public NodeIdentity parent;
    public Dictionary<string, OutgoingInfo> outputs = new Dictionary<string, OutgoingInfo>();

    private void Start()
    {
        isSelected = false;
    }

    public void AddConnection(NodeIdentity newChild)
    {
        //output node can only have one parent
        outputs.Add(newChild.id, new OutgoingInfo(newChild, "NA", makeArrow(parent.transform, newChild.transform)));
    }

    public void OnMouseDown()
    {
        if (IDE_Input_Controller.instance != null)
        {
            if (IDE_Input_Controller.instance.sourceNode != null)
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

    public GameObject makeArrow(Transform outLink, Transform inLink)
    {
        GameObject result = Instantiate(new GameObject());
        result.transform.parent = outLink;
        LineRenderer lr = result.AddComponent<LineRenderer>();
        lr.SetPositions(new Vector3[] { outLink.position, inLink.position });
        lr.startColor = Color.red;
        lr.endColor = Color.green;
        lr.startWidth = 8;
        lr.endWidth = 8;
        return result;
    }

    //Create coroutine that draws line between mouse and selcted output node?
}
