using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;
using HoloToolkit.Unity.InputModule;

public class CreateWires : MonoBehaviour, IInputClickHandler, IFocusable
{
    GameObject SelectedObject;

    public static int CorrectWireCount = 0;
    public static int IncorrectWireCount = 0;

    public int SwitchesSnapped = 0;
    public int ConnectorsSnapped = 0;
    bool WireCollidersEnabled = false;

    public class Argument
    {
        public string name;
        public int index;


        public Argument(string _name, int _index)
        {
            name = _name;
            index = _index;
        }
    }

    public static string[,] ConnectionArray =
    {
            {"SnapSwitch1","SnapConnector1","cb5e0335-aebe-4f87-a21e-6629236facb3","0"},
            {"SnapSwitch1","SnapSwitch3","79a410e6-e927-492f-8e9c-cb5eca15a2e3","0"},
            {"SnapSwitch2","SnapConnector1","4ad9e691-73d6-4777-af10-8c84db4e0a2d","0"},
            {"SnapSwitch2","SnapConnector2","c38d2bfb-f735-4cfb-b255-8d5323cbad0b","0"},
            {"SnapSwitch3","SnapConnector2","dedb4e8c-c51b-49af-af87-536fbde737cc","0"},
            //Incorrect Combinations
            {"SnapSwitch1","SnapSwitch2","fb443109-c1ce-4aa7-86b1-60f3da772dc8","0"},
            {"SnapSwitch1","SnapConnector2","34eb3285-30f9-4a51-bfe5-82162a0850d2","0"},
            {"SnapSwitch2","SnapSwitch3","91b5db76-98da-4246-b68b-10b1c2c2b2af","0"},
            {"SnapSwitch3","SnapConnector1","64338977-af0f-4a5c-804e-bf2991b5bad8","0"},
            {"SnapConnector1","SnapConnector2","60468c0f-8f4a-487a-95cc-8481627c8fd7","0"}
    };

    public void OnFocusEnter()
    {
        InputManager.Instance.PushModalInputHandler(gameObject);
    }

    public void OnFocusExit()
    {
        InputManager.Instance.PopModalInputHandler();
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        SelectedObject = eventData.selectedObject.transform.parent.gameObject;
        //SelectedObject = eventData.selectedObject.gameObject;
        Debug.Log(SelectedObject);


        AddToComparison();
        eventData.Use();
    }

    void RecurrsionParent(Transform PotentialParent)
    {
        if(PotentialParent.parent.name == "SceneLink")
        {
            SelectedObject = PotentialParent.gameObject;
        }
        else
        {
            RecurrsionParent(PotentialParent.parent.transform);
        }
    }

    void AddToComparison()
    {
        if (!CollabVertxObjectHandler.ObjectComparisonList.Contains(SelectedObject))
        {
            CollabVertxObjectHandler.ObjectComparisonList.Add(SelectedObject);
            if (CollabVertxObjectHandler.ObjectComparisonList.Count == 2)
            {
                //do things
                Comparison();
                //ValidateWiring();
                CollabVertxObjectHandler.ObjectComparisonList.Clear();
            }
        }
    }

    void Comparison()
    {
        NodeLink CurrentNodeLink = GetComponent<NodeLink>();

        bool isInCorrectWireIndex = false;

        var ComparisonArray = CollabVertxObjectHandler.ObjectComparisonList.ToArray();
        GameObject firstGameObject = ComparisonArray[0];
        GameObject secondGameObject = ComparisonArray[1];
        for (int i = 0; i < (ConnectionArray.Length/4); i++)
        {
            string name = ConnectionArray[i, 0].ToString() + ConnectionArray[i, 1].ToString();
            Argument args = new Argument(name, i);
            if (firstGameObject.name == ConnectionArray[i,0] && secondGameObject.name == ConnectionArray[i, 1] || secondGameObject.name == ConnectionArray[i, 0] && firstGameObject.name == ConnectionArray[i, 1])
            {
                if(i <= 4)
                {
                    isInCorrectWireIndex = true;
                }

                switch(ConnectionArray[i,3])
                {
                    case "0":
                        CurrentNodeLink.Fire("LoadWire", args);
                        if(isInCorrectWireIndex)
                        {
                            CorrectWireCount++;
                        }
                        else
                        {
                            IncorrectWireCount++;
                        }
                        break;
                    case "1":
                        CurrentNodeLink.Fire("DeleteWire", args);
                        if (isInCorrectWireIndex)
                        {
                            CorrectWireCount--;
                        }
                        else
                        {
                            IncorrectWireCount--;
                        }
                        break;
                }
                break;
            }
        }
    }

    // Method to create and return Vertex Node Link Game object 
    private GameObject CreateNode(string name, string id)
    {
        var vertxObject = SceneLink.Instance.transform.Find(name);
        GameObject vertxThing;
        if (vertxObject == null)
        {
            vertxThing = SceneLink.Instance.CreateNode(name,
                SceneLink.Instance.transform.position,
                Quaternion.identity,
                Vector3.one,
                id
           );
        }
        else
        {
            Debug.Log("Node: " + name + " \n already exists");
            vertxThing = vertxObject.gameObject;

        }
        return vertxThing;
    }

    void LoadWire(Argument args)
    {
        //unpack
        string name = args.name;
        int index = args.index;
        var temp = SceneLink.Instance.transform.Find(name);
        if (temp == null)
        {
            GameObject wire = CreateNode(name, ConnectionArray[index, 2]);
            ConnectionArray[index, 3] = "1";
            wire.AddComponent<RemoveCollider>();
        }

    }

    void DeleteWire(Argument args)
    {
        //unpack class
        string name = args.name;
        int index = args.index;

        GameObject temp = SceneLink.Instance.transform.Find(name).gameObject;
        if (temp != null)
        {
            Destroy(temp);
            ConnectionArray[index, 3] = "0";
        }

    }

    public void ValidateWiring()
    {
        Debug.Log("VALIDATING WIRING");
        int counter = 0;

        for (int i = 0; i < (ConnectionArray.Length / 4); i++)
        {
            if (ConnectionArray[i, 3] == "1")
            {
                counter++;
            }
            else
            {
                counter--;
            }
            
        }

    }

    void Update()
    {
        if (!WireCollidersEnabled)
        {
            if (SwitchesSnapped == 3 && ConnectorsSnapped == 2)
            {
                EnableWireColliders(gameObject);
            }
        }
    }

    void EnableWireColliders(GameObject toSearch)
    {
        foreach (UnityEngine.Transform child in toSearch.transform)
        {
            GameObject childObject = child.gameObject;

            if (toSearch.name.Contains("SnapSwitch") || toSearch.name.Contains("SnapConnector"))
            {
                if (childObject.name == "Primitive")
                {
                    var boxCollider = childObject.GetComponent<BoxCollider>();
                    if (boxCollider)
                    {
                        boxCollider.enabled = true;

                    }
                    continue;
                }
            }
            else
            {
                EnableWireColliders(childObject);
            }
        }
    }

    public void CheckIfValidCircuit()
    {
        if (CorrectWireCount == 5 && IncorrectWireCount == 0)
        {
            Debug.Log("VALID WIRE CONFIGURATION");
        }
        else
        {
            //highlight badwires red
            StopCoroutine(ShowBadWires());
            StartCoroutine(ShowBadWires());
        }
    }

    IEnumerator ShowBadWires()
    {
        yield return null;

        for (int i = 5; i < IncorrectWireCount + 5; i++)
        {
            if (ConnectionArray[i, 3] == "1")
            {
                GameObject badWire;
                string name = ConnectionArray[i, 0] + ConnectionArray[i, 1];
                string nameReverse = ConnectionArray[i, 1] + ConnectionArray[i, 0];
                if (SceneLink.Instance.transform.Find(name))
                {
                    badWire = SceneLink.Instance.transform.Find(name).gameObject;
                    RecurrsionSearch(badWire);
                }
                else if (SceneLink.Instance.transform.Find(nameReverse))
                {
                    badWire = SceneLink.Instance.transform.Find(nameReverse).gameObject;
                    RecurrsionSearch(badWire);
                }
                else
                {
                    Debug.Log("bad wire not found");
                }
            }
        }
    }

    void RecurrsionSearch(GameObject gameObject)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject childObject = gameObject.transform.GetChild(i).gameObject;
            if (childObject.name == "Primitive")
            {
                Renderer renderer = childObject.GetComponent<Renderer>();
                Material mat = renderer.material;
                float emision = Mathf.PingPong(Time.time, 1.0f);
                Color baseColour = Color.red;
                Color finalColour = baseColour * Mathf.LinearToGammaSpace(emision);
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", finalColour);
            }
            else
            {
                RecurrsionSearch(childObject);
            }
        }
    }

}
