using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScreen : MonoBehaviour
{
    Material _material;
    [SerializeField] int period = 900;
    [SerializeField] int next_scene_index = 0;
    
    System.DateTime stoptime; 

    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<Renderer>().material;
        _material.SetFloat("_Transition_Ticks", 0);
        _material.SetFloat("_Period", period);
        stoptime = ClockSelector.time;

        // stoptime = System.DateTime.Now.AddSeconds(period);
    }

    // Update is called once per frame
    void Update()
    {
        // Get time left
        var dt = stoptime - System.DateTime.Now;
        var seconds = dt.TotalSeconds;

        // Calculate 
        var ticks = Mathf.Clamp(period - (float) seconds,1,period + 1); 
        _material.SetFloat("_Transition_Ticks", ticks);

        if (ticks >= period)
        {
            Debug.Log("Loading next scene");
            SceneManager.LoadScene(next_scene_index);
            
        }

        
    }
}
