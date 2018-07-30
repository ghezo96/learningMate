using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityAnim : MonoBehaviour {

    Material matt3;
    float offset = 0.5f;
    public float speed = 0.01f;
    /*public bool BlueWire1;
    public bool BlueWire2;
    public bool BlueWire3;
    public bool BlueWire4;
    public bool BrownWire1;
    public bool BrownWire2;
    public bool BrownWire3;
    public bool BrownWire4;
    public bool OrangeWire1;
    public bool OrangeWire2;
    public bool OrangeWire3;
    public bool ThinCables;*/

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameObject child;
        for (int i = 0; i<transform.childCount;i++)
        {
            
                matt3 = transform.GetChild(i).GetComponent<Renderer>().material;

                matt3.SetTextureOffset("_MainTex", new Vector2(0, offset));
                offset += speed / 10;
                if (offset >= 1f)
                {
                    offset = 0f;

                }
        }
    }
}
