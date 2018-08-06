using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertexUnityPlayer;
using VertexDataTypes;

public class ModifiedStart : MonoBehaviour {

    void NodeLink_Loaded()
    {
        RecurrsionSearch(gameObject);
    }

    void RecurrsionSearch(GameObject gameObject)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            
            GameObject childObject = gameObject.transform.GetChild(i).gameObject;

            if (gameObject.name.Contains("Box") || gameObject.name.Contains("SupportShelf"))
            {
                if (childObject.name == "Primitive")
                {
                    Destroy(childObject.GetComponent<BoxCollider>());
                }
            }
           
            else if (gameObject.name.Contains("SnapSwitch") || gameObject.name.Contains("SnapConnector"))
            {
                if (childObject.name == "Primitive")
                {
                    var boxCollider = childObject.GetComponent<BoxCollider>();
                    var boxColliderSize = boxCollider.size;
                   
                    gameObject.AddComponent<BoxCollider>();
                    gameObject.GetComponent<BoxCollider>().size = boxCollider.size*0.1f;
                    gameObject.GetComponent<BoxCollider>().isTrigger = true;
                   // gameObject.AddComponent<IsColiding>();
                    gameObject.layer = UnityEngine.LayerMask.NameToLayer("SnapPoints");
                    Destroy(childObject.GetComponent<BoxCollider>());
                }
               
                   
                    //childObject.AddComponent<MoveAndSnap>();
                    //MeshCollider meshCollider = childObject.GetComponent<MeshCollider>();
                    //meshCollider.skinWidth = 0.0001f;
                    //Rigidbody rb = childObject.AddComponent<Rigidbody>();
                    //rb.useGravity = false;
                    //rb.constraints = RigidbodyConstraints.FreezeAll;
               //     gameObject.AddComponent<Rigidbody>();
               //// gameObject.AddComponent<IsColiding>();
               // gameObject.GetComponent<Rigidbody>().useGravity = false;
               // gameObject.GetComponent<Rigidbody>().isKinematic = false;
               // gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            }
            else
            {
                RecurrsionSearch(childObject);
            }
        }
    }
}