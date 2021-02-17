using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RejectHumanity : MonoBehaviour
{
    public SerialController serialController;


    void Start()
    {
        serialController = FindObjectOfType<SerialController>();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            serialController.SendSerialMessage("1");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            serialController.SendSerialMessage("2");
        }

    }

    // Invoked when a connect/disconnect event occurs. The parameter 'success'
    // will be 'true' upon connection, and 'false' upon disconnection or
    // failure to connect.
    void OnConnectionEvent(bool success)
    {
        
    }
}
