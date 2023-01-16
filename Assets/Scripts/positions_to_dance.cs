using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class positions_to_dance : MonoBehaviour
{
    [SerializeField] BodyTrackingSkeleton skel;

    Transform leftArm;
    Transform rightArm;
    Transform leftKnee;
    Transform rightKnee;



    // Start is called before the first frame update
    void Start()
    {
        leftArm = skel.leftWrist.transform;
        rightArm = skel.rightWrist.transform;
        leftKnee = skel.leftKnee.transform;
        rightKnee = skel.rightKnee.transform;



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
