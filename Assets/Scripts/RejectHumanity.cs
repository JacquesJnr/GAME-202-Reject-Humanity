using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RejectHumanity : MonoBehaviour
{
    public SerialController serialController;
    [SerializeField] private TMPro.TextMeshProUGUI micRead;
    [SerializeField] private TMPro.TextMeshProUGUI piezoRead;

    private string recievedString;
    public string[] charArray;

    private Slider sliderBar;
    private string piezoString = "Piezo Value: ";
    private string micString = "Mic Value: ";

    private float monkeyLevel;
    private float humanityRate = 0.001f;

    void Start()
    {
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
        sliderBar = GameObject.Find("Slider").GetComponent<Slider>();
    }

    
    void Update()
    {
        monkeyLevel = sliderBar.value;
        sliderBar.value -= humanityRate;

        recievedString = serialController.ReadSerialMessage();

        if (recievedString == null)
            return;

        if (ReferenceEquals(recievedString, SerialController.SERIAL_DEVICE_CONNECTED))
            Debug.Log("Connection established");
        //else
           // Debug.Log("Message arrived: " + recievedString);

        charArray = recievedString.Split(',');
        
       if(charArray.Length == 3)
        {
            if (charArray[0] == "1")
                Debug.Log("Scream!");

            if (charArray[1] == "1")
                Debug.Log("Stomp!");

            if (charArray[2] == "1")
                Debug.Log("Touched!");
        }
    }
}
