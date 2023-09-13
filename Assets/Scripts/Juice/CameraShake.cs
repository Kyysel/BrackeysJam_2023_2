using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public static CameraShake instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There's more than one CameraShake! " + transform + " - " + instance);
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void ShakeOnce(float magnitude, float duration, float fadeOutTime)
    {
        StartCoroutine(ShakeOnceCoroutine(magnitude, duration, fadeOutTime));
    }

    private IEnumerator ShakeOnceCoroutine(float magnitude, float duration, float fadeOutTime)
    {
        Vector3 originalPos = transform.localPosition;

        float timeElapsed = 0f;
        float totalTime = duration + fadeOutTime;
        float curMagnitude = magnitude;

        while (timeElapsed < totalTime)
        {

            float x = Random.Range(-1f, 1f) * curMagnitude;
            float y = Random.Range(-1f, 1f) * curMagnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            timeElapsed += Time.deltaTime;

            if (timeElapsed > duration) 
            {
                curMagnitude = Mathf.Lerp(0f, magnitude, (totalTime - timeElapsed) / fadeOutTime);
            }

            yield return null;

        }

        transform.localPosition = originalPos;

    }


}
