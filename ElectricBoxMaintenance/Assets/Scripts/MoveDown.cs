using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDown : MonoBehaviour {

    Vector3 startPosition;
    Vector3 endPosition;
    float MovementDuration = 5.0f;
    float LerpSpeed = 2.0f;

	// Use this for initialization
	void Start () {
        endPosition = transform.position;
        startPosition = new Vector3(endPosition.x, 0.3f, endPosition.z);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator SlideDown(Vector3 startPosition, Vector3 endPosition)
    {
        float startTime = Time.time;
        while(Time.time - startTime <= MovementDuration)
        {
            float percentageComplete = (Time.time - startTime) / MovementDuration;
            Vector3 targetPosition = startPosition + Vector3.down * percentageComplete;
            yield return null;
        }

        yield return null;
    }

    public void beginMovement()
    {
        StartCoroutine(SlideDown(startPosition, endPosition));
    }
}
