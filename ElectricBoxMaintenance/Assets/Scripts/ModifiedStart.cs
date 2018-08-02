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
       

            if (gameObject.name.Contains("SnapSwitch") || gameObject.name.Contains("SnapConnector"))
            {
                if (childObject.name == "Primitive")
                {
                    var boxCollider = childObject.GetComponent<BoxCollider>();
                    var boxColliderSize = boxCollider.size;
                   
                    gameObject.AddComponent<BoxCollider>();
                    gameObject.GetComponent<BoxCollider>().size = boxCollider.size;
                    gameObject.GetComponent<BoxCollider>().isTrigger = true;
                    gameObject.layer = UnityEngine.LayerMask.NameToLayer("SnapPoints");
                    Destroy(childObject.GetComponent<BoxCollider>());
                }
                   
                    //childObject.AddComponent<MoveAndSnap>();
                    //MeshCollider meshCollider = childObject.GetComponent<MeshCollider>();
                    //meshCollider.skinWidth = 0.0001f;
                    //Rigidbody rb = childObject.AddComponent<Rigidbody>();
                    //rb.useGravity = false;
                    //rb.constraints = RigidbodyConstraints.FreezeAll;
                    gameObject.AddComponent<Rigidbody>();
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                
            }
            else
            {
                RecurrsionSearch(childObject);
            }
        }
    }
}