using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;
using VertexDataTypes;

public class ModifiedStart : MonoBehaviour
{
    List<Component> colliders = new List<Component>();
   // bool hasLoaded = false;
    
    void NodeLink_Loaded()
    {
        
            RecurrsionSearch(gameObject);
      
    }

    void RecurrsionSearch(GameObject toSearch)
    {
        foreach (UnityEngine.Transform child in toSearch.transform)
        {
            GameObject childObject = child.gameObject;

            if (toSearch.name.Contains("Box") || toSearch.name.Contains("SupportShelf"))
            {
                if (childObject.name == "Primitive")
                {
                    Destroy(childObject.GetComponent<BoxCollider>());
                }
            }
            else if (toSearch.name.Contains("SnapSwitch") || toSearch.name.Contains("SnapConnector"))
            {
                if (childObject.name == "Primitive")
                {
                    var boxCollider = childObject.GetComponent<BoxCollider>();
                    if (boxCollider)
                    {
                        var boxColliderSize = boxCollider.size;
                        BoxCollider newBoxCollider = toSearch.AddComponent<BoxCollider>();
                        newBoxCollider.size = boxCollider.size * 0.1f;
                        newBoxCollider.isTrigger = false;

                        //boxCollider.enabled = false;
                        Destroy(boxCollider);

                    }
                    // gameObject.AddComponent<IsColiding>();
                    toSearch.layer = UnityEngine.LayerMask.NameToLayer("SnapPoints");
                    continue;
                }
            }
            else
            {
                RecurrsionSearch(childObject);
            }
        }
    }
}