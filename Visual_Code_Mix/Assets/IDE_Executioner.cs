using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDE_Executioner : MonoBehaviour
{
    //Singleton
    public static IDE_Executioner instance;
    private void Awake()
    {
        if (instance)
            Destroy(gameObject);
        else
            instance = this;
    }

    public void executeCode()
    {
        //Find all sources

        //Execute until need additional info move to next source

        //Execute program until all nodes visited
    }
}
