using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;
using VertexDataTypes;

public class SwitchAndConnectorNode : MonoBehaviour
{
    List<GameObject> switches = new List<GameObject>();
    List<GameObject> connectors = new List<GameObject>();
   
    void Start()
    {
        //StartCoroutine(OnStart());
    }

    IEnumerator OnStart()
    {
       
        yield return new WaitForSeconds(1.0f);
        RecurrsionSearch();
    }

    void RecurrsionSearch()
    {
        foreach (NodeLink x in SceneLink.Instance.GetComponentsInChildren<NodeLink>())
        { 
                if (x.name == "SWITCH" )
                {
                    switches.Add(x.gameObject);
                }
                if (x.name == "CONNECTOR")
                {
                    connectors.Add(x.gameObject);
                }
        }
       
        for (int i = 0; i < switches.Count; i++)
        {
            AddBoxColliderToSwitches(switches[i]);
        }
        for (int i = 0; i < connectors.Count; i++)
        {
            AddBoxColliderToConnectors(connectors[i]);
        }
    }

    public void AddBoxColliderToConnectors(GameObject go)
    {
        GameObject childObject = go.transform.Find("visual").transform.Find("Root Scene").transform.Find("RootNode").transform.Find("Connectors").transform.Find("Primitive").gameObject;

        var boxCollider = childObject.GetComponent<BoxCollider>();
        var boxColliderSize = boxCollider.size;

        go.layer = UnityEngine.LayerMask.NameToLayer("Component");
        go.AddComponent<BoxCollider>();
        go.GetComponent<BoxCollider>().size = boxCollider.size;
        go.GetComponent<BoxCollider>().isTrigger = true;
        Destroy(childObject.GetComponent<BoxCollider>());
    }

    public void AddBoxColliderToSwitches(GameObject go)
    {
        GameObject childObject = go.transform.Find("visual").transform.Find("Root Scene").transform.Find("RootNode").transform.Find("Switch1").transform.Find("Primitive").gameObject;

        var boxCollider = childObject.GetComponent<BoxCollider>();
        var boxColliderSize = boxCollider.size;

        go.layer = UnityEngine.LayerMask.NameToLayer("Component");
        go.AddComponent<BoxCollider>();
        go.GetComponent<BoxCollider>().size = boxCollider.size;
        go.GetComponent<BoxCollider>().isTrigger = true;
        Destroy(childObject.GetComponent<BoxCollider>());
    }

}
