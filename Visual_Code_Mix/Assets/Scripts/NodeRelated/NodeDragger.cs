using Shapes2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NodeDragger : MonoBehaviour
{
    Camera mainCam;
    public SpriteRenderer spriteRender;
    int sortingOrderFG = 0;
    int sortingOrderBG = 0;

    Vector3 startDragPos = Vector3.zero;
    bool isBeingDragged = false;

    private void Start()
    {
        mainCam = Camera.main;
    }

    //[MOUSE EVENTS FUNCTIONS]
    //UI References for sorting order
    public MeshRenderer nodeText;
    private void OnMouseDown()
    {
        startDragPos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        //Making all node content apear on top of other nodes
        sortingOrderBG = spriteRender.sortingOrder;
        sortingOrderFG = nodeText.sortingOrder;
        spriteRender.sortingOrder = 100;
        nodeText.sortingOrder = 101;
        setLinksOrdering(101);
        isBeingDragged = true;
        selectNode();
    }

    private void OnMouseUp()
    {
        //Placing node where it was before in terms of sorting
        spriteRender.sortingOrder = sortingOrderBG;
        nodeText.sortingOrder = sortingOrderFG;
        setLinksOrdering(sortingOrderFG);
        isBeingDragged = false;
    }

    void setLinksOrdering(int orderingNum)
    {
        foreach (ConnectionLink link in nodeIdentity.linksIn.Values)
        {
            link.gameObject.GetComponent<SpriteRenderer>().sortingOrder = orderingNum;
        }
        foreach (ConnectionLink link in nodeIdentity.linksOut.Values)
        {
            link.gameObject.GetComponent<SpriteRenderer>().sortingOrder = orderingNum;
        }
    }

    //[NODE SELECTION]
    public NodeIdentity nodeIdentity;
    public Shape nodeShape;
    Color oldOutline;
    Color oldFill;
    public void selectNode()
    {
        //deselect old node if there is one
        if (IDE_Coding_Controller.instance && IDE_Coding_Controller.instance.targetNode)
            IDE_Coding_Controller.instance.targetNode.GetComponent<NodeDragger>().deselectNode();

        oldOutline = nodeShape.settings.outlineColor;
        oldFill = nodeShape.settings.fillColor;
        IDE_Coding_Controller.instance.updateTarget(nodeIdentity);

        //Chage to selected state
        nodeShape.settings.outlineColor = Color.green;
    }

    public void deselectNode()
    {
        //Change to desected visual state
        nodeShape.settings.outlineColor = oldOutline;
    }


    //[UPDATE POSITION FUNCOTIONS]
    private void Update()
    {
        if (isBeingDragged)
            UpdateNodePosition();
    }

    //Translates node while it is being dragged (mouse pressed on)
    private void UpdateNodePosition()
    {
        transform.position = transform.position + (mainCam.ScreenToWorldPoint(Input.mousePosition) - startDragPos);
        startDragPos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        //Update the attached lines of dragged nodes
        foreach (KeyValuePair<string, List<OutgoingInfo>> link in nodeIdentity.connectionsIn)
        {
            foreach (OutgoingInfo connection in link.Value)
            {
                Vector3 newPos = new Vector3(connection.sinkIdentity.transform.position.x, connection.sinkIdentity.transform.position.y, -1);
                connection.connectionArrow.SetPosition(0, newPos);
            }
        }

        foreach (KeyValuePair<string, List<OutgoingInfo>> link in nodeIdentity.connectionsOut)
        {
            foreach (OutgoingInfo connection in link.Value)
            {
                Vector3 newPos = new Vector3(connection.sourceIdentity.transform.position.x, connection.sourceIdentity.transform.position.y, -1);
                connection.connectionArrow.SetPosition(1, newPos);
            }
        }
    }
}
