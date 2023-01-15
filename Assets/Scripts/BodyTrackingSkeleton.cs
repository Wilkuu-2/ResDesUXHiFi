<<<<<<< Updated upstream
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mediapipe;

public class BodyTrackingSkeleton : MonoBehaviour
{
    [SerializeField] WebcamToImageFrameManager manager;
    [SerializeField] Transform[] nodes;

    

    // Start is called before the first frame update
    void Start()
    {
        Protobuf.SetLogHandler(Protobuf.DefaultLogHandler);
        
        // TODO: Get the Pose_Landmarks  graph up and running
        // TODO: INPUT: The Image frame from the webcam
        // TODO: OUTPUT: LandmarkList to transpose to the skeleton
        // TODO: INITIALIZE: The skeleton
=======
using System;
using System.Collections;
using System.Collections.Generic;
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

    private ResourceManager _resourceManager;

    CalculatorGraph graph;
    System.Diagnostics.Stopwatch stopwatch;
    LandmarkList landmarks;
    OutputStream<LandmarkListPacket, LandmarkList> landmark_stream;
    GameObject[] markers;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        Debug.Log("Loading graph Assets");
        _resourceManager = new LocalResourceManager();
        yield return _resourceManager.PrepareAssetAsync("pose_landmark_full.bytes");

        Debug.Log("Creating Graphs");
        graph = new CalculatorGraph(_config.text);


        SidePacket sp = new SidePacket();

        sp.Emplace("model_complexity", new IntPacket(1));
        sp.Emplace("use_previous_landmarks", new BoolPacket(true));

        landmark_stream = new OutputStream<LandmarkListPacket,LandmarkList>(graph, "landmarks");
        landmark_stream.StartPolling().AssertOk();
        
        graph.StartRun(sp).AssertOk();

        stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        InitSkeleton();

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
            var gameobject = Instantiate(skeletonMarkerPrefab, transform.position + new Vector3(0,i,0), Quaternion.identity);
            gameobject.name = string.Format("Skeleton: [{0}]", i);
            markers[i] = gameobject;
        }
      
>>>>>>> Stashed changes
    }

    // Update is called once per frame
    void Update()
<<<<<<< Updated upstream
    {
        if (Input.anyKeyDown && !manager.checkImageFrame())
        {
            manager.updateImageFrame();
        }

        if (manager.consumeImageFrame())
        {
            Debug.Log(string.Format("Image frame consumed {0}", manager.getimageFrame()));
        }
        // TODO: Move the skeleton according to the 3d landmarks
        // TODO: Loop the graph
    }


    static string Mediapipe_Graph = 
@"

TODO: WRITE THE GRAPH

";
    }
=======
    { }

    void processLandmarks()
    {
        for (int i = 0; i < 33; i++)
        {
            var marker = markers[i];
            var landmark = landmarks.Landmark[i];

            if (landmark != null && marker != null && landmark.HasX && landmark.HasY && landmark.HasZ)
            {
                marker.transform.position = transform.position + new Vector3(landmark.X, landmark.Y, landmark.Z) * skeletonScale + skeletonOffset;
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
>>>>>>> Stashed changes

