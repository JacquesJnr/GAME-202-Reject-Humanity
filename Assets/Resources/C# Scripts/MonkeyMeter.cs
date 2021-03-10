using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonkeyMeter : MonoBehaviour
{
    private RejectHumanity rejectHumanity;
    
    
    private void Start()
    {
        rejectHumanity = FindObjectOfType<RejectHumanity>();
        
    }

    private void Update()
    {
        GetComponent<Image>().fillAmount = rejectHumanity.meterValue;
    }
}
