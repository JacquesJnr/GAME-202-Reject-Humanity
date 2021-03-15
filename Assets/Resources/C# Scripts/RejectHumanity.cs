using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class RejectHumanity : MonoBehaviour
{
    private SerialController serialController;   // The prefab in the scene recieving signals from com ports
    private AudioManager audioManager;
    [Range(0f, 1f)] public float meterValue;
    [SerializeField] private bool debugTouch;
    [SerializeField] private bool debugMic;
    [SerializeField] private bool debugPiezo;
    [SerializeField] private float touchFill = 0.02f;
    [SerializeField] private float drainRate = 0.0002f;
    [SerializeField] private List<GameObject> particles;

    private string recievedString;
    public string[] charArray;
    public bool left;
    public bool right;

    [SerializeField] private TMPro.TextMeshProUGUI arduinoStatus;
    [SerializeField] private GameObject bangFX;
    [SerializeField] private GameObject whamFX;

    public GameObject phase1, phase2, phase3;
    public Texture step1, step2, step3;

    private Image fillMask;
    private GameObject comicFX;
    public float minValue = 0;

    const int micOn = 1;
    const int micIndex = 2;
    const int piezoIndex = 4;
    const int sideIndex = 5;
    const int bangIndex = 6;

    private string colorStringRed = "<color=red> ";
    private string colorStringYellow = "<color=yellow> ";
    private string colorStringGreen = "<color=green> ";

    // The phases of becoming a monkey
    public enum Phases
    {
        Bang,
        Scream,
        Stomp,
        Complete
    };
    public Phases barPhase;

    void Start()
    {
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
        //fillMask = GameObject.Find("Fill").GetComponent<Image>();
        comicFX = GameObject.Find("Bang Zone");
        audioManager = FindObjectOfType<AudioManager>();
    }


    void Update()
    {
        // Constantly drains the monkey meter
        MonkeyMeter();

        recievedString = serialController.ReadSerialMessage();

        if (recievedString == null)
            return;

        if (ReferenceEquals(recievedString, SerialController.SERIAL_DEVICE_CONNECTED))
            Debug.Log("Connection established");

        charArray = recievedString.Split(',');


        // Checks the status of the arduino i.e. if it's calibrating, connected to disconected
        if (charArray.Length == 1)
        {
            ShowArduinoState();
        }

        // Check we're not calibrating and we're recieving the comma separated values
        if (charArray.Length == 7)
        {
            arduinoStatus.text = (colorStringGreen + "Ready");

            switch (barPhase)
            {
                case Phases.Bang:
                    HandleTouch();
                    break;
                case Phases.Scream:
                    HandleMic();
                    break;
                case Phases.Stomp:
                    HandlePiezo();
                    break;
            }
        }

        if (debugTouch && charArray.Length == 2)
        {
            arduinoStatus.text = (colorStringGreen + "Touch Test");
            HandleTouch();
        }

        if (debugMic)
        {
            HandleMic();
        }

        if (debugPiezo)
        {
            HandlePiezo();
        }

        ManageBangParticles();
    }


    void HandleMic()
    {
        // Check mic is above threshold
        if (charArray[micOn] == "1")
        {
            // Convert char array string to a float
            float volume = float.Parse(charArray[micIndex]);

            meterValue += volume * 2.5f;
        }
    }
    void HandlePiezo()
    {
        float hitStrength = float.Parse(charArray[piezoIndex]);

        Vector3 FXArea = new Vector3(0f, Random.Range(-2f, -2.2f), 0);

        if (hitStrength != 0)
        {
            meterValue += hitStrength / 10000;
            GameObject wham = Instantiate(whamFX, FXArea, Quaternion.identity, comicFX.transform);
            particles.Add(wham);
        }
        ManageBangParticles();
    }

    // Code used to test functionality of the touch sensors
    void HandleTouch()
    {
        // Check a sensor is active from serial communication
        if (charArray[bangIndex] == "1")
        {
            // Instantiate "Bang" effect at a random height on the either the left or right side of the screen.

            Vector3 leftArea = new Vector3(-5f, Random.Range(3.5f, 3.8f), 0);
            Vector3 rightArea = new Vector3(5f, Random.Range(3.5f, 3.8f), 0);


            // Left Sensor - Instantiate the particle effect on the left side of the screen
            if (charArray[sideIndex] == "L")
            {
                if (!left && meterValue < 1)
                {
                    meterValue += touchFill;
                    GameObject leftBang = Instantiate(bangFX, leftArea, Quaternion.identity, comicFX.transform);
                    audioManager.sounds[2].pitch = Random.Range(1.0f, 1.5f);
                    audioManager.Play("BangSound");
                    particles.Add(leftBang);
                }

                left = true;
            }
            else
                left = false;

            // Right Sensor- Instantiate the particle effect on the right side of the screen
            if (charArray[sideIndex] == "R")
            {
                if (!right && meterValue < 1)
                {
                    meterValue += touchFill;
                    GameObject rightBang = Instantiate(bangFX, rightArea, Quaternion.identity, comicFX.transform);
                    audioManager.sounds[2].pitch = Random.Range(1.0f, 1.5f);
                    audioManager.Play("BangSound");
                    particles.Add(rightBang);
                }

                right = true;
            }
            else
                right = false;
        }
    }

    void MonkeyMeter()
    {
        // Set the meter state based on the value of the monkey meter
        // Check the monkey meter valus is not 0
        if (meterValue != 0)
        {

            // 0.333f - 0.666f = Scream State
            if (meterValue > 1.0f / 3.0f && meterValue < 0.667f)
            {
                ScreamPhase();
            }

            // 0.666f - 1.0f = Stomp State            
            if (meterValue > 0.666f && meterValue < 1.0f)
            {
                StompPhase();
            }

            // 1f = Complete State
            if (meterValue >= 1.0f) 
            {
                barPhase = Phases.Complete;
                CompletePhase();
            }
               

            // Drain the meter as long as it is above the minimum value
            if (meterValue > minValue)
            {
                // .. Or as long as it is not filled
                if (meterValue <= 1.0f)
                {
                    meterValue -= drainRate * Time.deltaTime;
                }
            }
        }
        else
            // 0f - 0.333f = Bang State
            BangPhase();
    }

    float BangPhase()
    {
        barPhase = Phases.Bang;
        phase1.SetActive(true);
        minValue = 0;
        return minValue;
    }

    float ScreamPhase()
    {
        barPhase = Phases.Scream;

        if (GameObject.Find("Phase 1"))
        {
            Destroy(phase1);
        }

        if (!phase2.activeSelf)
        {
            phase2.SetActive(true);
        }       
        minValue = 1.0f / 3.0f;
        return minValue;
    }

    float StompPhase()
    {
        barPhase = Phases.Stomp;        
        
        if (!phase3.activeSelf)
        {
            phase3.SetActive(true);
        }       
        minValue = 2.0f / 3.0f;
        return minValue;
    }

    float CompletePhase()
    {
        Destroy(phase3);
        minValue = 1;
        HideMonkeyMeter();
        return minValue;        
    }

    public void HideMonkeyMeter()
    {
        LeanTween.scale(GameObject.Find("Title"), new Vector3(0, 0, 0), 0.5f).setDelay(0.5f).setOnComplete(() => SceneManager.LoadScene(2));
        LeanTween.scale(GameObject.Find("Monkey Meter 2.0"), new Vector3(0, 0, 0), 0.5f).setDelay(0.5f).setOnComplete(() => meterValue = 0);
    }

    // Makes sure there are never more than 3 Bang particles being rendered at once
    void ManageBangParticles()
    {
        if (particles.Count > 3)
        {
            Destroy(particles[0]);
            particles.RemoveAt(0);
        }
    }

    // Sets the string and color of the "arduino state" text
    void ShowArduinoState()
    {
        if (charArray[0] == "__Connected__")
        {
            arduinoStatus.text = (colorStringYellow + "Connected");
        }
        else if (charArray[0].Contains("Mic Average: "))
        {
            arduinoStatus.text = (colorStringYellow + "Calibrating");
        }
        else if (charArray[0] == "__Disconnected__")
        {
            arduinoStatus.text = (colorStringRed + "Disconnected");
        }
        else if (charArray[0] == "")
        {
            arduinoStatus.text = (colorStringRed + "Disconnected");
        }
    }

    // Checks if the mic state is active and adjust the fill rate of the humanity bar
    void HandleInput()
    {
        if (charArray[0] != "0")
        {
            // sliderBar.value += (fillRate * 1.5f) * multiplier;
        }

        else if (charArray[1] != "0")
        {
            // sliderBar.value += (fillRate * 1.5f) * multiplier;
        }

        else if (charArray[2] != "0")
        {
            // sliderBar.value += fillRate * multiplier;
        }
    }

}