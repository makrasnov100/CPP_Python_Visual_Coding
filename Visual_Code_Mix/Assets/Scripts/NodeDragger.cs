using Shapes2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NodeDragger : MonoBehaviour
{
    Camera mainCam;
    public SpriteRenderer spriteRender;
    int sortingOrder = 0;
    int sortingOrderText = 0;
    int sortingOrderConnectionPoints = 0;


    Vector3 startDragPos = Vector3.zero;
    bool isBeingDragged = false;

    private void Start()
    {
        mainCam = Camera.main;
    }

    //[MOUSE EVENTS FUNCTIONS]
    //UI References for sorting order
    public MeshRenderer nodeText;
    public SpriteRenderer nodeInput;
    public SpriteRenderer nodeOutput;
    private void OnMouseDown()
    {
        startDragPos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        //Making all node content apear on top of other nodes
        sortingOrder = spriteRender.sortingOrder;
        sortingOrderText = nodeText.sortingOrder;
        sortingOrderConnectionPoints = nodeInput.sortingOrder;
        spriteRender.sortingOrder = 100;
        nodeText.sortingOrder = 101;
        nodeInput.sortingOrder = 102;
        nodeOutput.sortingOrder = 102;

        isBeingDragged = true;
        selectNode();
    }

    private void OnMouseUp()
    {
        //Placing node wher it was before in terms of sortin
        spriteRender.sortingOrder = sortingOrder;
        nodeText.sortingOrder = sortingOrderText;
        nodeInput.sortingOrder = sortingOrderConnectionPoints;
        nodeOutput.sortingOrder = sortingOrderConnectionPoints;
        spriteRender.sortingOrder = sortingOrder;

        isBeingDragged = false;
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
        foreach (OutgoingInfo connection in nodeIdentity.connections)
        {
            bool isSourceOfConnection = connection.sourceIdentity.parent.id == nodeIdentity.id;
            if (isSourceOfConnection)
            {
                Vector3 newPos = new Vector3(connection.sourceIdentity.transform.position.x, connection.sourceIdentity.transform.position.y, -1);
                connection.connectionArrow.SetPosition(1, newPos);
            }
            else
            {
                Vector3 newPos = new Vector3(connection.sinkIdentity.transform.position.x, connection.sinkIdentity.transform.position.y, -1);
                connection.connectionArrow.SetPosition(0, newPos);
            }
        }


        //if (IDE_Executioner.instance)
        //{
        //    if (IDE_Executioner.instance.connectionsOut.ContainsKey(nodeIdentity.id))
        //    {
        //        foreach (KeyValuePair<string, OutgoingInfo> connection in IDE_Executioner.instance.connectionsOut[nodeIdentity.id])
        //        {
        //            connection.Value.
        //        }
        //    }
        //    if (IDE_Executioner.instance.connectionsIn.ContainsKey(nodeIdentity.id))
        //    {
        //        foreach (KeyValuePair<string, OutgoingInfo> connection in IDE_Executioner.instance.connectionsIn[nodeIdentity.id])
        //        {

        //        }
        //    }
        //}
        //else
        //{

        //}
    }


}
