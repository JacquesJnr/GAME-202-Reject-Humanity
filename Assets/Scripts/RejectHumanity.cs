using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RejectHumanity : MonoBehaviour
{
    public SerialController serialController;   
    [SerializeField] private bool debugText;
    [SerializeField] private float fillRate = 0.02f;

    private string recievedString;
    public string[] charArray;

    private TMPro.TextMeshProUGUI micState;
    private TMPro.TextMeshProUGUI piezoState;
    private TMPro.TextMeshProUGUI touchState;

    private Slider sliderBar;
    private string piezoString = "Piezo Value: ";
    private string micString = "Mic Value: ";
    private string touchString = "Touch Value: ";
    private string active = "Active";
    private string inactive = "Inactive";

    private float monkeyLevel;
    private float drainRate = 0.001f;
    

    void Start()
    {
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
        sliderBar = GameObject.Find("Slider").GetComponent<Slider>();

        micState = GameObject.Find("Mic State").GetComponent<TMPro.TextMeshProUGUI>();
        piezoState = GameObject.Find("Piezo State").GetComponent<TMPro.TextMeshProUGUI>();
        touchState = GameObject.Find("Touch State").GetComponent<TMPro.TextMeshProUGUI>();

        if (!debugText)
        {
            micState.gameObject.SetActive(false);
            piezoState.gameObject.SetActive(false);
            touchState.gameObject.SetActive(false);
        }
    }
    
    void Update()
    {
        monkeyLevel = sliderBar.value;
        sliderBar.value -= drainRate;

        recievedString = serialController.ReadSerialMessage();

        if (recievedString == null)
            return;

        if (ReferenceEquals(recievedString, SerialController.SERIAL_DEVICE_CONNECTED))
            Debug.Log("Connection established");

        charArray = recievedString.Split(',');
        
        // Check we're not calibrating and we're recieving the comma separated values
       if(charArray.Length == 3)
       {
            if (debugText)
            {
                DebugText();
            }
       }
    }

    // Checks if the mic state is active and adjust the fill rate of the humanity bar
    void HandleMicInput()
    {
        if(charArray[0] != "0")
        {
           
        }

    }

    // Changes the color and string of the debug text if the relative sensor is sending input.
    void DebugText()
    {
        // Make the text visible.
        micState.gameObject.SetActive(true);
        piezoState.gameObject.SetActive(true);
        touchState.gameObject.SetActive(true);

        // Set the text color and string
        if (charArray[0] == "1")
            micState.text = micString + "<color=green> " + active;
        else
            micState.text = micString + "<color=red> " + inactive;

        if (charArray[1] == "1")
            piezoState.text = piezoString + "<color=green> " + active;
        else
            piezoState.text = piezoString + "<color=red> " + inactive;

        if (charArray[2] == "1")
            touchState.text = touchString + "<color=green> " + active;
        else
            touchState.text = touchString + "<color=red> " + inactive;
    }
}
