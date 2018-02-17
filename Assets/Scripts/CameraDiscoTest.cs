using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDiscoTest : MonoBehaviour 
{
    // Speed of color cycle
    [SerializeField]
    float colorSpeed = .005f;

    // Camera
    Camera camera;

	// Use this for initialization
	void Start () 
	{
        camera = GetComponent<Camera>();
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
            Color.RGBToHSV(camera.backgroundColor, out h, out s, out v);
            h += colorSpeed;
            if (h == 1f)
                h = 0f;
            camera.backgroundColor = Color.HSVToRGB(h, s, v);
            yield return null;
        }
    }

}
