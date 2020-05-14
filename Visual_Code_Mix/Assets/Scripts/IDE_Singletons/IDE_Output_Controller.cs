using Shapes2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDE_Output_Controller : MonoBehaviour
{
    public static IDE_Output_Controller instance;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }


    //UI References
    public GameObject allOutputUI;
    public GameObject messageHolder;
    public GameObject messagePrefab;

    //Instance Variables
    List<GameObject> messages = new List<GameObject>();


    //Message Methods
    public void AddOutput(string msg, OutputMessageType type = OutputMessageType.info)
    {
        //Create message add to list of other outputs
        GameObject curMsgGO = Instantiate(messagePrefab);
        curMsgGO.transform.SetParent(messageHolder.transform);

        //Setup the message text and apearence
        OutputMessageIdentity curMsg = curMsgGO.GetComponent<OutputMessageIdentity>();
        curMsg.SetupMessage(msg, type);

        messages.Add(curMsgGO);
    }

    public void ClearOutput()
    {
        foreach (GameObject msg in messages)
        {
            Destroy(msg);
        }
        messages.Clear();
    }

    public void OpenOutput()
    {
        allOutputUI.SetActive(true);
    }

    public void CloseOutput()
    {
        ClearOutput();
        allOutputUI.SetActive(false);
    }

}
