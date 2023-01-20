using UnityEngine;
using Unity.Collections;
using Mediapipe;
using System;

public class WebcamToImageFrameManager: MonoBehaviour
{
    public int width;
    public int height;
    public int index;

    public MeshRenderer[] cubes_live = new MeshRenderer[1];
    public MeshRenderer[] cubes_frame = new MeshRenderer[1];


    public WebCamTexture webcam;
    Color32[] _input_pixel32;
    ImageFrame frame;
    Texture2D frameTexture;
    static NativeArray<byte> data;

    public void Start()
    {
        Glog.Logtostderr = true; // when true, log will be output to `Editor.log` / `Player.log` 
        //Glog.Initialize("MediaPipeUnityPlugin");

        webcam = new WebCamTexture(getWebcam(index),width, height);
        webcam.Play();

        _input_pixel32 = new Color32[webcam.width * webcam.height];
        frameTexture = new Texture2D(webcam.width, webcam.height, TextureFormat.ARGB32, 1, false);
        
        foreach(var cube in cubes_live)
            cube.material.mainTexture = webcam;

        foreach (var cube in cubes_frame)
            cube.material.mainTexture = frameTexture; 
    }

    private string getWebcam(int index)
    {
        Debug.Assert(WebCamTexture.devices.Length > 0, "No webcam available");

        if (index < WebCamTexture.devices.Length)
        {
            return WebCamTexture.devices[index].name;
        }

        return WebCamTexture.devices[0].name;
    }

    public void getImageFrame(out ImageFrame iframe)
    {
        frameTexture.SetPixels32(webcam.GetPixels32(_input_pixel32));
        var raw_tex = frameTexture.GetRawTextureData<byte>();
        frame = new ImageFrame(ImageFormat.Types.Format.Srgba, webcam.width, webcam.height, 4 * webcam.width, raw_tex);
        iframe = frame; 
    }

    public void OnDestroy()
    {
        webcam.Stop();
    }
}
