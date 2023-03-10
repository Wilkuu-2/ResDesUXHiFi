using System;
using System.Collections;
using Mediapipe;
using UnityEngine;
using Mediapipe.Unity;


[RequireComponent(typeof(WebcamToImageFrameManager))]
public class BodyTrackingSkeleton : MonoBehaviour
{
    [SerializeField] WebcamToImageFrameManager manager;
    [SerializeField] GameObject skeletonMarkerPrefab;
    [SerializeField] TextAsset _config;
    [SerializeField] float skeletonScale;
    [SerializeField] Vector3 skeletonOffset;


#nullable enable
    public GameObject? leftWrist { 
        get { 
            try { return markers[15]; } 
            catch (NullReferenceException) { return null; } 
            } 
        }
    public GameObject? rightWrist { 
        get { 
            try { return markers[15]; } 
            catch (NullReferenceException) { return null; } 
            } 
        }
    public GameObject? leftKnee { 
        get { 
            try { return markers[15]; } 
            catch (NullReferenceException) { return null; } 
            } 
        }
    public GameObject? rightKnee {  
        get { 
            try { return markers[15]; } 
            catch (NullReferenceException) { return null; } 
            } 
        }
    private static ResourceManager? _resourceManager = null;
#nullable disable


    CalculatorGraph graph;
    System.Diagnostics.Stopwatch stopwatch;
    LandmarkList landmarks;
    OutputStream<LandmarkListPacket, LandmarkList> landmark_stream;
    GameObject[] markers;
    public bool hasLandmarks = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        InitSkeleton();
        Debug.Log("Loading graph Assets");
        
        if (_resourceManager == null) {
            _resourceManager = new StreamingAssetsResourceManager();
            yield return _resourceManager.PrepareAssetAsync("pose_landmark_full.bytes");
        }


        Debug.Log("Creating Graphs");
        graph = new CalculatorGraph(_config.text);


        SidePacket sp = new SidePacket();

        sp.Emplace("model_complexity", new IntPacket(1));
        sp.Emplace("use_previous_landmarks", new BoolPacket(true));
        sp.Emplace("smooth_landmarks", new BoolPacket(true));

        landmark_stream = new OutputStream<LandmarkListPacket,LandmarkList>(graph, "landmarks");
        landmark_stream.StartPolling().AssertOk();
        
        graph.StartRun(sp).AssertOk();

        stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        while (true)
        {
            manager.getImageFrame(out var frame);

            var currentTimestampMicrosec = stopwatch.ElapsedTicks / (TimeSpan.TicksPerMillisecond / 1000);
            graph.AddPacketToInputStream("input_video", new ImageFramePacket(frame, new Timestamp(currentTimestampMicrosec))).AssertOk();

            yield return new WaitForEndOfFrame();

            var landmarks_list = new LandmarkList();
            //if (landmark_poller.Next(landmarks_packet))
            if(landmark_stream.TryGetNext(out landmarks))
            {
                processLandmarks();
            }


        }

    }

    private void InitSkeleton()
    {
        markers = new GameObject[33];
        for (int i = 0; i < 33; i++)
        {
            GameObject gameobject = Instantiate(skeletonMarkerPrefab, transform.position + new Vector3(0,i,0), Quaternion.identity);
            gameobject.name = string.Format("Skeleton: [{0}]", i);
            gameobject.transform.parent = transform;
            markers[i] = gameobject;
        }

        
      
    }

    // Update is called once per frame
    void Update()
    { }

    void processLandmarks()
    {
        for (int i = 0; i < 33; i++)
        {
            var marker = markers[i];
            var landmark = landmarks.Landmark[i];

            if (landmark != null && marker != null && landmark.HasX && landmark.HasY && landmark.HasZ)
            {
                marker.transform.localPosition = new Vector3(landmark.X, landmark.Y, landmark.Z) * skeletonScale + skeletonOffset;
                hasLandmarks = true;
            }
            else 
            {
                hasLandmarks = false;
            }
            
        }
    }

    void OnDestroy()
    {
        if (graph != null)
        {
            try
            {
                graph.CloseInputStream("input_video").AssertOk();
                graph.WaitUntilDone().AssertOk();
                
            }
            finally
            {
                graph.Dispose();
            }
        }
    }


}

