using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{

    Renderer[] childRenderers;
    public bool visible;

    // Use this for initialization
    void Start()
    {
        childRenderers = GetComponentsInChildren<Renderer>();
        visible = false;

        foreach (Renderer i in childRenderers)
        {
            i.material.color = new Color(i.material.color.r, i.material.color.g, i.material.color.b, 0);
        }

    }


    public void Fade()
    {
        StartCoroutine(FadeWithWait(0.2f));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutWithWait(0.2f));
    }

    IEnumerator FadeWithWait(float x)
    {
        yield return new WaitForSeconds(x);

        foreach (Renderer i in childRenderers)
        {
            StartCoroutine(FadingIn(i));
        }
    }

    IEnumerator FadeOutWithWait(float x)
    {
        yield return new WaitForSeconds(x);

        foreach (Renderer i in childRenderers)
        {
            StartCoroutine(FadingOut(i));
        }
    }

    IEnumerator FadingIn(Renderer i)
    {

        float alpha = i.material.color.a;
        float t = 0;
        while (t < 1f)
        {
            Color color = i.material.color;
            color.a = Mathf.Lerp(0, 1, t);
            t += 1f * Time.deltaTime;
            i.material.color = color;

            yield return null;
        }
    }

    IEnumerator FadingOut(Renderer i)
    {
        float alpha = i.material.color.a;
        float t = 0;
        while (t < 1f)
        {
            Color color = i.material.color;
            color.a = Mathf.Lerp(1, 0, t);
            t += 1f * Time.deltaTime;
            i.material.color = color;

            yield return null;
        }
    }



}