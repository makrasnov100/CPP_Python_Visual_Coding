using Shapes2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputConsoleController : MonoBehaviour
{

    //UI References
    GameObject allOutputUI;
    GameObject messageHolder;
    GameObject messagePrefab;

    //Instance Variables
    List<GameObject> messages = new List<GameObject>();


    //Message Methods
    void AddOutput(string msg, OutputMessageType type)
    {
        //Create message add to list of other outputs
        GameObject curMsgGO = Instantiate(messagePrefab);
        curMsgGO.transform.SetParent(messageHolder.transform);

        //Setup the message text and apearence
        OutputMessageIdentity curMsg = curMsgGO.GetComponent<OutputMessageIdentity>();
        curMsg.SetupMessage(msg, type);

        messages.Add(curMsgGO);
    }

    void ClearOutput()
    {
        foreach (GameObject msg in messages)
        {
            Destroy(msg);
        }
        messages.Clear();
    }

    void OpenOutput()
    {
        allOutputUI.SetActive(true);
    }

    void CloseOutput()
    {
        allOutputUI.SetActive(false);
    }

}
