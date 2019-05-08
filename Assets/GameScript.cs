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
                connectionResult = "Tu dispositivo no soporta comunicaciones por Bluetooth.";
                break;
            case 1:
                connectionResult = "Necesitas prender el Bluetooth en tu celular.";
                break;
            case 2:
                connectionResult = "Necesitas vincular el dispositivo antes de poder utilizarlo.";
                break;
            case 3:
                connectionResult = "No se pudo conectar al dispositivo, vuelve a intentarlo.";
                break;
            case 4:
                connectionResult = "Conexión exitosa.";
                break;
            default:
                connectionResult = "Resultado inesperado.";
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
