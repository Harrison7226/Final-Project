using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadGlow : MonoBehaviour
{
    private Light currentLight;

    public float speed;
    public float maxIntensity = 10f;

    // Start is called before the first frame update
    void Start()
    {
        currentLight = GetComponent<Light>();
        currentLight.color = new Color(78f/255, 230f/255, 18f/255);
    }

    // Update is called once per frame
    void Update()
    {
        float t = Mathf.Sin(Time.time * speed);
        t += 1;
        t /= 2;
        currentLight.intensity = Mathf.Lerp(0, maxIntensity, t);
    }
}
