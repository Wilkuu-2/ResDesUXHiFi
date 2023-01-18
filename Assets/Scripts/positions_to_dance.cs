using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class positions_to_dance : MonoBehaviour
{
    [SerializeField] BodyTrackingSkeleton skel;
    [SerializeField] float[] weights = new float[5]; 
    [SerializeField] float[] tresholds = new float[5]; 


    Transform leftArm_tr;
    Transform rightArm_tr;
    Transform leftKnee_tr;
    Transform rightKnee_tr;

    float averageMovementValue = float.NegativeInfinity;
    public bool ready = false;

    public float movementScore = 0;
    public int currentMovementScoreState = 0;

    public TrackingPoint leftArm;
    public TrackingPoint rightArm;
    public TrackingPoint leftKnee;
    public TrackingPoint rightKnee;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        while(skel.rightKnee == null)
        {
            Debug.Log("Waiting");
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Ready");
        leftArm_tr = skel.leftWrist.transform;
        rightArm_tr = skel.rightWrist.transform;
        leftKnee_tr = skel.leftKnee.transform;
        rightKnee_tr = skel.rightKnee.transform;

        leftArm = new TrackingPoint();
        rightArm = new TrackingPoint();
        leftKnee = new TrackingPoint();
        rightKnee = new TrackingPoint();

        while (true) {

            leftArm.Feed(leftArm_tr);
            rightArm.Feed(rightArm_tr);

            leftKnee.Feed(leftKnee_tr);
            rightKnee.Feed(rightKnee_tr);

            // TODO: Score berekenen

            float MovementValue = leftArm.speed * weights[0] + rightArm.speed * weights[1] + leftKnee.speed * weights[2] + rightKnee.speed * weights[3];

            if (float.IsInfinity(averageMovementValue))
            {
                averageMovementValue = MovementValue;
            }
            else
            {
                averageMovementValue = averageMovementValue * 0.9f + MovementValue * 0.1f;
            }

            movementScore += MovementValue - weights[4] * averageMovementValue;

            currentMovementScoreState = 0;

            foreach (float t in tresholds)
            {
                currentMovementScoreState += movementScore > t ? 1 : 0;
            }
            yield return new WaitForEndOfFrame();
        }
        
 
    }

}


public class TrackingPoint {
    Vector3 prev = Vector3.zero;
    Vector3 current = Vector3.zero;
    public Vector3 furthest = Vector3.zero;
    public Vector3 closest = new Vector3(10000,10000,10000); 

    public Vector3 position { get { return current; } }
    public float magnitude { get { return current.magnitude; } }
    public Vector3 velocity { get { return (current - prev) * Time.deltaTime; } }
    public float speed { get { return velocity.magnitude; } }
    public float relSpeed { get { return prev.magnitude - current.magnitude; } }
    
    public void Feed(Transform point)
    {
        prev = current;
        current = point.localPosition;

        if(magnitude < closest.magnitude)
        {
            closest = current; 
        }
        if(magnitude > furthest.magnitude)
        {
            furthest = current;
        }
    }
    public bool isMovingInwards { get { return relSpeed < 0; } }
    
}
