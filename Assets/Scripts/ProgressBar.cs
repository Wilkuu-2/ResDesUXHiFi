using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ProgressBar : MonoBehaviour
{
    public float min;
    public float max;
    public float status;

    Image mask;

    // Start is called before the first frame update
    void Start()
    {
        mask = GetComponent<Image>(); 
    }

    // Update is called once per frame
    void Update()
    {
        setBar();
    }

    void setBar()
    {
        float fillAmt = Mathf.Clamp01((status - min) / (max - min));
        mask.fillAmount = fillAmt; 
       
    }
}
