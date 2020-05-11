using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Stores references to objects and allows for effcient auto complete options
public class Library : MonoBehaviour
{
    public static Library instance;
    public Dictionary<string, BaseFunction> functions;

    void Awake()
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

    private void Start()
    {
        functions = new Dictionary<string, BaseFunction>();
        functions.Add("print", new PrintValue());
    }

}
