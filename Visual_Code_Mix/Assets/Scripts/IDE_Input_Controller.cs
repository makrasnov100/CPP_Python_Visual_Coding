using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

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
    public Dictionary<string, NodeIdentity> nodes = new Dictionary<string, NodeIdentity>();


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

    public GameObject debugTarget;
    IEnumerator doubleClickCooldown()
    {
        currentClicks += 1;
        if (currentClicks >= 2)
        {
            Vector3 groundPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
            Vector3 mouseFloatPos = new Vector3(groundPos.x, groundPos.y, -10);
            Ray ray = new Ray(mouseFloatPos, Vector3.forward);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, 50);

            //Draw debug position (if needed)
            //Instantiate(debugTarget, groundPos, Quaternion.identity, gameObject.transform);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "NodeConnectIn")
                {
                    addConnection(hit.collider.gameObject.GetComponent<ConnectionLink>());
                }
            }
            else //didn't hit anything (creating new node)
            {
                createNewNode(groundPos);
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
    void createNewNode(Vector3 pos)
    {
        GameObject curNode = Instantiate(genericNode, pos, Quaternion.identity, gameObject.transform);
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
        
        //Add connection to the nodes theselves
        sourceNode.AddConnection(sinkNode);
        //Add interconnected node to the computed node list
        if (IDE_Executioner.instance)
        {
            IDE_Executioner.instance.updateComputedNodesOnAddedConnection(sourceNode, sinkNode);
        }


        sourceNode.Deselect();
        sourceNode = null;
    }
}
