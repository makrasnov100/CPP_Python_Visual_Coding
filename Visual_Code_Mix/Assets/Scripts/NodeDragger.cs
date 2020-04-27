using Shapes2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeDragger : MonoBehaviour
{
    Camera mainCam;
    public SpriteRenderer spriteRender;
    int sortingOrder = 0;

    Vector3 startDragPos = Vector3.zero;
    bool isBeingDragged = false;

    private void Start()
    {
        mainCam = Camera.main;
    }

    //[MOUSE EVENTS FUNCTIONS]
    private void OnMouseDown()
    {
        startDragPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        sortingOrder = spriteRender.sortingOrder;
        spriteRender.sortingOrder = 100;
        isBeingDragged = true;
        selectNode();
    }



    private void OnMouseUp()
    {
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
        IDE_Coding_Controller.instance.setTarget(nodeIdentity, new List<string> { "type", "name", "value"}, "Node:");

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
    }


}
