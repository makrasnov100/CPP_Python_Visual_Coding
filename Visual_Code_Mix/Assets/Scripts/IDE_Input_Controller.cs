using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IDE_Input_Controller : MonoBehaviour
{
    static public IDE_Input_Controller instance;


    //Input settings
    public float timeForDoubleClick = 0.2f;



    //Instance variables
    public int currentClicks = 0;

    //Used prefabs/objects
    public GameObject genericNode;
    public Camera mainCamera;


    //Node map
    Dictionary<string, NodeIdentity> nodes = new Dictionary<string, NodeIdentity>();


    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(doubleClickCooldown());
        }
    }

    IEnumerator doubleClickCooldown()
    {
        currentClicks += 1;
        if (currentClicks >= 2)
        {
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), -Vector2.up);
            //Refernce: https://docs.unity3d.com/ScriptReference/Physics2D.Raycast.html
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "NodeConnectIn")
                {
                    addConnection(hit.collider.gameObject.GetComponent<ConnectionLink>());
                }
            }
            else //didn't hit anything (creating new node)
            {
                createNewNode();
            }
            currentClicks = 0;
            StopAllCoroutines();
        }
        else 
        {
            yield return new WaitForSeconds(timeForDoubleClick);
            currentClicks -= 1;
        }
    }



    //[NODE RELATED]
    void createNewNode()
    {
        GameObject curNode = Instantiate(genericNode, mainCamera.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity, gameObject.transform);
        NodeIdentity curIdentity = curNode.GetComponent<NodeIdentity>();
        curNode.transform.position = new Vector3(curNode.transform.position.x, curNode.transform.position.y, 0);
        string nodeId = randomString(8);
        while (nodes.ContainsKey(nodeId))
        {
            nodeId = randomString(8);
        }
        curIdentity.setID(nodeId);

        nodes.Add(nodeId, curIdentity);
    }

    public string randomString(int length)
    {
        var newWord = new char[length];
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        for (int i = 0; i < length; i++)
        {
            newWord[i] = chars[UnityEngine.Random.Range(0, chars.Length)];
        }
        return new string(newWord);
    }

    //[CREATING CONNECTIONS]
    public ConnectionLink sourceNode = null;
    public void addConnection(ConnectionLink sinkNode)
    {
        if (sourceNode == null || sourceNode.parent == null)
            return;
        
        //Keep track of the new connection visual and in code
        OutgoingInfo outInfo = sourceNode.AddConnection(sinkNode);
        if (IDE_Executioner.instance)
            IDE_Executioner.instance.addConnection(sourceNode.parent, outInfo);

        sourceNode.Deselect();
        sourceNode = null;
    }
}
