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
    }

    // Update is called once per frame
    void Update()
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

