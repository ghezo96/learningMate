using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityAnim : MonoBehaviour
{

    Material material;
    Transform child;
    float offset1 = 0f;
    float offset2 = 0.1f;
    float offset3 = 0.1f;
    float offset4 = 0.1f;
    float offset5 = 0f;
    float offset6 = 0f;
    float offset7 = 0f;
    float offset8 = 0f;
    float offset9 = 0f;
    float offset10 = 0f;
    public float speed = 0.01f;
    public bool BlueWire1;
    public bool BlueWire2;
    public bool BlueWire3;
    public bool BlueWire4;
    public bool BlueWire5;
    public bool BrownWire1;
    public bool BrownWire2;
    public bool BrownWire3;
    public bool BrownWire4;
    public bool BrownWire5;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            if (child.name == "BlueWire1")
            {
                material = child.GetComponent<Renderer>().material;
                material.SetTextureOffset("_MainTex", new Vector2(0, offset1));

                if (BlueWire1)
                {
                    offset1 += speed;
                    if (offset1 >= 1f)
                    {
                        offset1 = 0f;
                    }
                }
                else
                {
                    offset1 = 0f;
                }
            }

            if (child.name == "BlueWire2")
            {
                material = child.GetComponent<Renderer>().material;
                material.SetTextureOffset("_MainTex", new Vector2(0, offset2));

                if (BlueWire2)
                {
                    offset2 += speed;
                    if (offset2 >= 1f)
                    {
                        offset2 = 0f;
                    }
                }
                else
                {
                    offset2 = 0f;
                }
            }

            if (child.name == "BlueWire3")
            {
                material = child.GetComponent<Renderer>().material;
                material.SetTextureOffset("_MainTex", new Vector2(0, offset3));

                if (BlueWire3)
                {
                    offset3 += speed;
                    if (offset3 >= 1f)
                    {
                        offset3 = 0f;
                    }
                }
                else
                {
                    offset3 = 0f;
                }
            }

            if (child.name == "BlueWire4")
            {
                material = child.GetComponent<Renderer>().material;
                material.SetTextureOffset("_MainTex", new Vector2(0, offset4));

                if (BlueWire4)
                {
                    offset4 += speed;
                    if (offset4 >= 1f)
                    {
                        offset4 = 0f;
                    }
                }
                else
                {
                    offset4 = 0f;
                }
            }

            if (child.name == "BlueWire5")
            {
                material = child.GetComponent<Renderer>().material;
                material.SetTextureOffset("_MainTex", new Vector2(0, offset5));

                if (BlueWire5)
                {
                    offset5 += speed;
                    if (offset5 >= 1f)
                    {
                        offset5 = 0f;
                    }
                }
                else
                {
                    offset5 = 0f;
                }
            }

            if (child.name == "BrownWire1")
            {
                material = child.GetComponent<Renderer>().material;
                material.SetTextureOffset("_MainTex", new Vector2(0, offset6));

                if (BrownWire1)
                {
                    offset6 += speed;
                    if (offset6 >= 1f)
                    {
                        offset6 = 0f;
                    }
                }
                else
                {
                    offset6 = 0f;
                }
            }

            if (child.name == "BrownWire2")
            {
                material = child.GetComponent<Renderer>().material;
                material.SetTextureOffset("_MainTex", new Vector2(0, offset7));

                if (BrownWire2)
                {
                    offset7 += speed;
                    if (offset7 >= 1f)
                    {
                        offset7 = 0f;
                    }
                }
                else
                {
                    offset7 = 0f;
                }
            }

            if (child.name == "BrownWire3")
            {
                material = child.GetComponent<Renderer>().material;
                material.SetTextureOffset("_MainTex", new Vector2(0, offset8));

                if (BrownWire3)
                {
                    offset8 += speed;
                    if (offset8 >= 1f)
                    {
                        offset8 = 0f;
                    }
                }
                else
                {
                    offset8 = 0f;
                }
            }


            if (child.name == "BrownWire4")
            {
                material = child.GetComponent<Renderer>().material;
                material.SetTextureOffset("_MainTex", new Vector2(0, offset9));

                if (BrownWire4)
                {
                    offset9 += speed;
                    if (offset9 >= 1f)
                    {
                        offset9 = 0f;
                    }
                }
                else
                {
                    offset9 = 0f;
                }
            }

            if (child.name == "BrownWire5")
            {
                material = child.GetComponent<Renderer>().material;
                material.SetTextureOffset("_MainTex", new Vector2(0, offset10));

                if (BrownWire5)
                {
                    offset10 += speed;
                    if (offset10 >= 1f)
                    {
                        offset10 = 0f;
                    }
                }
                else
                {
                    offset10 = 0f;
                }
            }
        }
    }
    
}
