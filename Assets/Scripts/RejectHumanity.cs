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

    private bool isConnected;
    private string recievedString;
    public string[] charArray;

    [SerializeField] private TMPro.TextMeshProUGUI micState;
    [SerializeField] private TMPro.TextMeshProUGUI piezoState;
    [SerializeField] private TMPro.TextMeshProUGUI touchState;
    [SerializeField] private TMPro.TextMeshProUGUI arduinoStatus;

    private Slider sliderBar;
    private string piezoString = "Piezo Value: ";
    private string micString = "Mic Value: ";
    private string touchString = "Touch Value: ";
    private string active = "Active";
    private string inactive = "Inactive";

    private string colorStringRed = "<color=red> ";
    private string colorStringYellow = "<color=yellow> ";
    private string colorStringGreen = "<color=green> ";

    [SerializeField] private int multiplier = 0;

    private float monkeyLevel;
    private float drainRate = 0.0008f;
    

    void Start()
    {
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
        sliderBar = GameObject.Find("Monkey Meter").GetComponent<Slider>();
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

        if(charArray.Length == 1)
        {
            if (charArray[0] == "_Connected_")
            {
               arduinoStatus.text = (colorStringYellow + "Connected");
            }
            else if(charArray[0].Contains("Mic Average: "))
            {
                arduinoStatus.text = (colorStringYellow + "Calibrating");
            }
            else if(charArray[0] == "_Disconnected_")
            {
                arduinoStatus.text = (colorStringRed + "Disconnected");
            }
        }

        // Check we're not calibrating and we're recieving the comma separated values
        if (charArray.Length == 3)
        {
            arduinoStatus.text = (colorStringGreen + "Ready");
            if (debugText)
            {
                DebugText();
            }
            else
            {
                micState.gameObject.SetActive(false);
                piezoState.gameObject.SetActive(false);
                touchState.gameObject.SetActive(false);
            }

            for (int i = 0; i < charArray.Length; i++)
            {
                if (charArray[i].Contains("1"))
                {
                    multiplier = 0;
                }
            }

            for (int i = 0; i < charArray.Length; i++)
            {
                if (charArray[i] == "1")
                {
                    multiplier++;
                } else {
                    multiplier = 0;
                }
            }
            HandleInput();
        }       
    }

    // Checks if the mic state is active and adjust the fill rate of the humanity bar
    void HandleInput()
    {
        if(charArray[0] != "0")
        {
            sliderBar.value += fillRate * multiplier;
        }

        if (charArray[1] != "0")
        {
            sliderBar.value += 0.2f * multiplier;
        }

        if (charArray[2] != "0")
        {
            sliderBar.value += fillRate * multiplier;
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
            micState.text = micString + colorStringGreen + active;
        else
            micState.text = micString + colorStringRed + inactive;

        if (charArray[1] == "1")
            piezoState.text = piezoString + colorStringGreen + active;
        else
            piezoState.text = piezoString + colorStringRed + inactive;

        if (charArray[2] == "1")
            touchState.text = touchString + colorStringGreen + active;
        else
            touchState.text = touchString + colorStringRed + inactive;
    }
}
