using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDiscoTest : MonoBehaviour 
{
    Light light;
	// Use this for initialization
	void Start () 
	{
        light = GetComponent<Light>();
        StartCoroutine(DiscoModeCoroutine());
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

    private IEnumerator DiscoModeCoroutine()
    {
        float h, s, v;
        while (true)
        {
            Color.RGBToHSV(light.color, out h, out s, out v);
            h += .01f;
            if (h == 1f)
                h = 0f;
            light.color = Color.HSVToRGB(h, s, v);
            yield return null;
        }
    }

}
