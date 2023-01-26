using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;


namespace ResDesUX
{
    public class EndScreenController : MonoBehaviour
    {
        public TextMeshProUGUI wakeuptime;
        public TextMeshProUGUI worktime;
        public TextMeshProUGUI flower_state;
        public TextMeshProUGUI currentTime;

        public int next_scene_index = 1;

        public string[] flower_str = {"The flowers are Extremely disapointed with your dance.",
                                     "The flowers are Saddened by your lack of enthousiasm.",
                                     "The flowers are Satisfied with your dance.",
                                     "The flowers are Happy because of your beautiful dance.",
                                     "The flowers are Thrilled by your performance." };

        // Start is called before the first frame update
        void Start()
        {
            wakeuptime.text = string.Format("It took you {0:0.0} seconds to get out of bed!",positions_to_dance.to_wakeup.TotalSeconds);
            worktime.text = string.Format("It took you {0:mm\\:ss} to move sufficiently!",positions_to_dance.to_end);
            flower_state.text = flower_str[positions_to_dance.lastFlowerState];
        }

        // Update is called once per frame
        void Update()
        {
            currentTime.text = string.Format("It is {0} right now.",DateTime.Now.ToString("t"));
        }

       public void switchOut()
        {
            SceneManager.LoadScene(next_scene_index);
        }
    }
}
