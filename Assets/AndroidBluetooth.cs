using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidBluetooth : MonoBehaviour
{
    private string device;
    private AndroidJavaObject androidBluetoothJavaClass;

    public AndroidBluetooth(string _device) {
        device = _device;
    }

    public int connectToDevice() {
        androidBluetoothJavaClass = new AndroidJavaObject("com.example.bluetoothlibary.AndroidBluetooth");
        int connectionState = androidBluetoothJavaClass.Call<int>("connectToDevice", device);
        return connectionState;
    }

    public string getData() {
        return androidBluetoothJavaClass.Call<string>("getReadData");
    }

    public void disconnectFromDevice() {
        androidBluetoothJavaClass.Call("closeBtConnection");
    }
}
