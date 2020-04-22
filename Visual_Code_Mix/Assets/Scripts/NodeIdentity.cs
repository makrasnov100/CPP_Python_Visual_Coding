using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType{none,data,selection,loop,function}
public enum DataType {none, intT, doubleT,stringT,boolT}
public enum FunctionType {none, cpp, python, visual}
public class NodeIdentity : MonoBehaviour
{


    //Node settings;
    string id = "";
    NodeType nodeType = NodeType.none;
    DataType dataType = DataType.none;
    FunctionType funcType = FunctionType.none;

    //NODE SETUP
    public void setupNode(string id)
    {
        this.id = id;
        beginSetup();
    }


    void beginSetup()
    {
        //Display type of node selection
        displaySelectionOptions(
            new List<string>() {"Data", "Selection", "Loop", "Function"},
            new List<Color>() { }
        );
    }

    public void onSelectedNodeType(NodeType typeSelected)
    {
        nodeType = typeSelected;
        //Display type of node selection
        displaySelectionOptions(
            new List<string>() { "Data", "Selection", "Loop", "Function" },
            new List<Color>() {Color.magenta, Color.cyan, Color.yellow, Color.green}
        );
    }

    public void onSelectedDataType(DataType typeSelected)
    {
        dataType = typeSelected;
        clearSelections();
    }


    //[UI RELATED]
    float optionPadding = .2f;
    float outsidePadding = .2f;
    List<NodeOptionSetup> nodeOptions = new List<NodeOptionSetup>();
    public GameObject containerRow;
    public GameObject nodeOption;
    void displaySelectionOptions(List<string> text, List<Color> colors)
    {
        //Check if can display
        if (text.Count != colors.Count)
        {
            return;
        }

        //Calculate the dimensions of options
        float parentWidth = gameObject.transform.localScale.x;
        float optionWidth = (parentWidth - (2 * outsidePadding) - (optionPadding * (text.Count-1))) / text.Count;
        float parentHeight = gameObject.transform.localScale.y;
        float optionHeight = parentHeight - (2 * outsidePadding);

        //Populate the Node
        for (int i = 0; i < text.Count; i++)
        {
            //Instnatiete
        }
    }

    void clearSelections()
    {
        for (int i = 0; i < nodeOptions.Count; i++)
        {
            Destroy(nodeOptions[i]);
        }
        nodeOptions.Clear();
    }
}
