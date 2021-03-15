using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchInterface : MonoBehaviour
{
    private SerialController serialController;   // The prefab in the scene recieving signals from com ports
    private AudioManager audioManager;
    private string recievedString;
    public string[] charArray;

    public bool touched;
    public int sceneIndex;
    // Start is called before the first frame update
    void Start()
    {
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        recievedString = serialController.ReadSerialMessage();

        if (recievedString == null)
            return;

        if (ReferenceEquals(recievedString, SerialController.SERIAL_DEVICE_CONNECTED))
            Debug.Log("Connection established");

        charArray = recievedString.Split(',');

        if(charArray.Length > 1)
        {
            if(charArray[6] == "1")
            {
                touched = true;
                
            }
            else
            {
                touched = false;
            }
        }

        if (touched)
        {
            SceneManager.LoadScene(sceneIndex);
            audioManager.Play("StartGame");
        }
    }
}
