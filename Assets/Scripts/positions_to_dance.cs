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


class TrackingPoint {
    Vector3 prev = Vector3.zero;
    Vector3 current = Vector3.zero;
    Vector3 furthest = Vector3.zero;
    Vector3 closest = new Vector3(10000,10000,10000); 

    Vector3 position { get { return current; } }
    float magnitude { get { return current.magnitude } }
    Vector3 velocity { get { return (current - prev) * Time.deltaTime } }
    float speed { get { return velocity.magnitude} }
    float relSpeed { get { return prev.magnitude - current.magnitude; } }
    
    void Feed(Transform point)
    {
        prev = current;
        current = point.position;

        if(magnitude < closest.magnitude)
        {
            closest = current; 
        }
        if(magnitude > furthest.magnitude)
        {
            furthest = current;
        }
    }
    bool isMovingInwards { get { return relSpeed < 0; } }
    
}
