using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class GameScript : MonoBehaviour
{
    public Text connectionText;
    public Text dataText;
    private AndroidBluetooth androidBluetooth;

    // Start is called before the first frame update
    void Start()
    {
        androidBluetooth = new AndroidBluetooth("HC-05");
        int connectionState = androidBluetooth.connectToDevice();
        string connectionResult;
        switch (connectionState)
        {
            case 0:
                connectionResult = "Your device doesn't support Bluetooth communications.";
                break;
            case 1:
                connectionResult = "Your Bluetooth is off.";
                break;
            case 2:
                connectionResult = "You must pair the desired device before attempting to connect to it.";
                break;
            case 3:
                connectionResult = "The device you want to connect to is currently unavailable.";
                break;
            case 4:
                connectionResult = "Connection successful.";
                break;
            default:
                connectionResult = "Unexpected Result.";
                break;
        }
        connectionText.text = connectionResult;
    }

    // Update is called once per frame
    void Update()
    {
        string btData = androidBluetooth.getData();
        dataText.text = btData;
    }

    void OnApplicationQuit()
    {
        androidBluetooth.disconnectFromDevice();
    }
}
