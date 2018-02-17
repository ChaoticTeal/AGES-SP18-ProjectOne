using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDiscoTest : MonoBehaviour 
{
    // Color shift speed
    [SerializeField]
    float colorSpeed = .01f;

    Light light;

	// Use this for initialization
	void Start () 
	{
        light = GetComponent<Light>();
        StartCoroutine(DiscoModeCoroutine());
	}

    private IEnumerator DiscoModeCoroutine()
    {
        float h, s, v;
        while (true)
        {
            Color.RGBToHSV(light.color, out h, out s, out v);
            h += colorSpeed;
            if (h == 1f)
                h = 0f;
            light.color = Color.HSVToRGB(h, s, v);
            yield return null;
        }
    }

}
