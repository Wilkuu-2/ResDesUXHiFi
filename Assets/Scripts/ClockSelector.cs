using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ClockSelector : MonoBehaviour
{
    public static System.DateTime time;
    [SerializeField] Vector2 _set_time;

    TextMeshProUGUI _time_text; 

    // Start is called before the first frame update
    void Start()
    {
        ChangeTime(0,false);
        _time_text = GetComponent<TextMeshProUGUI>();


    }

    // Update is called once per frame
    void Update()
    {
        _time_text.text = string.Format("{0:00}:{1:00}",time.Hour, time.Minute);
    }

    public void ChangeHours(int n)
    {
        ChangeTime(n, false);
    }
    
    public void ChangeMinutes(int n)
    {
        ChangeTime(n, true);
    }

    public void ChangeTime(int n,bool min_or_hours)
    {

        int mod = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? 5 : 1;
        int change = n * mod;

        if (min_or_hours)
        {
            _set_time.y += change;
        }
        else
        {
            _set_time.x += change;
        }

        if( _set_time.y > 59)
        {
                _set_time.y -= 60;
                _set_time.x += 1;
        }
        
        if(_set_time.y < 0)
        {
            _set_time.y += 60;
            _set_time.x += 1;
        }

        if(_set_time.x > 23)
        {
            _set_time.x -= 24;
        }

        if(_set_time.x < 0)
        {
            _set_time.x += 24; 
        }
        
       
        
        var now = System.DateTime.Now;

        var set_hour = (int) Mathf.Round(_set_time.x);
        var set_min = (int) Mathf.Round(_set_time.y);

        time = new System.DateTime(now.Year, now.Month, now.Day, set_hour, set_min, 0);

        if (time < now)
        {
            time = time.AddDays(1);
        }

        Debug.Log(time.ToLongDateString());
        Debug.Log(time.ToLongTimeString());
    }

    public void Arm()
    {
        SceneManager.LoadScene(2);
    }
}
