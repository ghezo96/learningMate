using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityAnim : MonoBehaviour
{

    Material material;
    Transform child;
    float offset1;
    float offset2;
    float offset3;
    float offset4;
    float offset5;
    float offset6;
    float offset7;
    float offset8;
    float offset9;
    float offset10;
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
            child = transform.GetChild(i);

            if (child.name == "BlueWire1")
            {
                AnimateMaterial(BlueWire1, offset1);
            }

            if (child.name == "BlueWire2")
            {
                AnimateMaterial(BlueWire2, offset2);
            }

            if (child.name == "BlueWire3")
            {
                AnimateMaterial(BlueWire3, offset3);
            }

            if (child.name == "BlueWire4")
            {
                AnimateMaterial(BlueWire4, offset4);
            }

            if (child.name == "BlueWire5")
            {
                AnimateMaterial(BlueWire5, offset5);
            }

            if (child.name == "BrownWire1")
            {
                AnimateMaterial(BrownWire1, offset6);
            }

            if (child.name == "BrownWire2")
            {
                AnimateMaterial(BrownWire2, offset7);
            }

            if (child.name == "BrownWire3")
            {
                AnimateMaterial(BrownWire3, offset8);
            }

            if (child.name == "BrownWire4")
            {
                AnimateMaterial(BrownWire4, offset9);
            }

            if (child.name == "BrownWire5")
            {
                AnimateMaterial(BrownWire5, offset10);
            }

        }
    }

    private void AnimateMaterial(bool wire, float offset)
    {
        material = child.GetComponent<Renderer>().material;
        material.SetTextureOffset("_MainTex", new Vector2(0, offset));

        if (wire)
        {
            offset8 += speed;
            if (offset >= 1f)
            {
                offset = 0f;
            }
        }
        else
        {
            offset = 0f;
        }
    }
}
