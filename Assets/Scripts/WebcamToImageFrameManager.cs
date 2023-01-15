using UnityEngine;
using Unity.Collections;
using Mediapipe;
using System;

public class WebcamToImageFrameManager: MonoBehaviour
{
    public int width;
    public int height;
    public int index;

    public MeshRenderer[] cubes_live; 
    public MeshRenderer[] cubes_frame; 


<<<<<<< Updated upstream
    WebCamTexture webcam;
    Texture2D tex;
=======
    public WebCamTexture webcam;
    Color32[] _input_pixel32;
>>>>>>> Stashed changes
    ImageFrame frame;
    Texture2D frameTexture;
    static NativeArray<byte> data;

    bool imageFrameAvail = false;
    
    public void Start()
    {
<<<<<<< Updated upstream
        webcam = new WebCamTexture(getWebcam(index),width, height);
        tex = new Texture2D(webcam.width, webcam.height);
        webcam.Play();
        frameTexture = new Texture2D(webcam.width, webcam.height, TextureFormat.ARGB32, 1, false);
=======
        Glog.Logtostderr = true; // when true, log will be output to `Editor.log` / `Player.log` 
        //Glog.Initialize("MediaPipeUnityPlugin");

        webcam = new WebCamTexture(getWebcam(index),width, height);
        webcam.Play();

        _input_pixel32 = new Color32[webcam.width * webcam.height];
        frameTexture = new Texture2D(webcam.width, webcam.height, TextureFormat.ARGB32, 1, false);
        

>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
    public void updateImageFrame()
    {
        Graphics.CopyTexture(webcam, frameTexture);
        var raw_tex = frameTexture.GetRawTextureData<byte>();
        frame = new ImageFrame(ImageFormat.Types.Format.Srgba, webcam.width, webcam.height, 4 * webcam.width, raw_tex);
        imageFrameAvail = true;

    }
    public bool consumeImageFrame()
    {
        if (imageFrameAvail)
        {
            imageFrameAvail = false;
            return true;
        }

        return false;
    }

    public bool checkImageFrame()
    {
        return imageFrameAvail;
    }
    public ImageFrame getimageFrame()
    {
        return frame;
    }

    public static void Deleter(IntPtr _ptr)
    {
        data.Dispose();
=======
    public void getImageFrame(out ImageFrame iframe)
    {
        frameTexture.SetPixels32(webcam.GetPixels32(_input_pixel32));
        var raw_tex = frameTexture.GetRawTextureData<byte>();
        frame = new ImageFrame(ImageFormat.Types.Format.Srgba, webcam.width, webcam.height, 4 * webcam.width, raw_tex);
        iframe = frame; 
>>>>>>> Stashed changes
    }
}
