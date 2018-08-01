using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snapping : MonoBehaviour {

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("<== COLLISION DETECTED ==>");       
    }
}
