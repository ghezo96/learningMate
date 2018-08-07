using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelAnimation : MonoBehaviour {

    private float FadeDuration = 2f;
    public AnimationCurve MovementCurve;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator FadeIn()
    {
        Renderer panelRenderer = gameObject.GetComponent<Renderer>();
        float startTime = Time.time;
        while(startTime - Time.time <= FadeDuration)
        {
            float percentageComplete = (startTime - Time.time) / FadeDuration;
            float value = MovementCurve.Evaluate(percentageComplete);
            panelRenderer.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f * value);
            Debug.Log("Value ==> " + value);
            yield return null;
        }
        panelRenderer.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        yield return null;
    }
}
