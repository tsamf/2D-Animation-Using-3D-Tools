using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraImageEffects : MonoBehaviour {


    private Camera cam;
    public Material mat;

    private void Awake()
    {
        //Find the main camera in the scene
        cam = GetComponent<Camera>();
        //Turn the DepthTextureMode flag on to track Depth
        cam.depthTextureMode = DepthTextureMode.Depth;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //Creates Outline on Objects in the depth texture of the scene
        Graphics.Blit(source, destination, mat);
    }
}
