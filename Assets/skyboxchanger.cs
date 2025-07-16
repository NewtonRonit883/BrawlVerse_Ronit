using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skyboxchanger : MonoBehaviour
{
    // Start is called before the first frame update
    public Material skyboxmap;
    void Start()
    {
        RenderSettings.skybox = skyboxmap;
    }

    // Update is called once per frame
    void OnPreRender()
    {
        if (skyboxmap != null)
            RenderSettings.skybox = skyboxmap;
    }
}
