package com.example.bluetoothlibary;

import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothSocket;

import java.io.IOException;
import java.io.InputStream;
import java.util.Set;
import java.util.UUID;

public class AndroidBluetooth {

    private final UUID PORT_UUID = UUID.fromString("00001101-0000-1000-8000-00805f9b34fb");
    private BluetoothAdapter phoneBtAdapter;
    private BluetoothDevice selectedBtDevice;
    private BluetoothSocket btSocket;
    private InputStream btInputStream;
    private String stringToSendToUnity;
    private boolean stopThread;

    public int connectToDevice(String deviceName) {
        phoneBtAdapter = BluetoothAdapter.getDefaultAdapter();
        if (!checkForBluetoothAdapter())
            return 0;
        else if (!checkIfBluetoothOn())
            return 1;
        Set<BluetoothDevice> pairedDevices = phoneBtAdapter.getBondedDevices();
        if (!checkIfDevicePaired(pairedDevices, deviceName))
            return 2;
        else {
            if (!connectToDevice())
                return 3;
            else {
                beginListeningForData();
                return 4;
            }
        }
    }

    private boolean checkForBluetoothAdapter() {
        return phoneBtAdapter != null;
    }

    private boolean checkIfBluetoothOn() {
        return phoneBtAdapter.isEnabled();
    }

    private boolean checkIfDevicePaired(Set<BluetoothDevice> pairedDevices, String desiredDevice) {
        if (pairedDevices.isEmpty())
            return false;
        else {
            for (BluetoothDevice device : pairedDevices) {
                if (device.getName().equals(desiredDevice)) {
                    selectedBtDevice = device;
                    return true;
                }
            }
        }
        return false;
    }

    private boolean connectToDevice() {
        try {
            btSocket = selectedBtDevice.createRfcommSocketToServiceRecord(PORT_UUID);
            btSocket.connect();
        } catch (IOException e) {
            e.printStackTrace();
            return false;
        }
        if (btSocket.isConnected()) {
            try {
                btInputStream = btSocket.getInputStream();
                return true;
            } catch (IOException e) {
                e.printStackTrace();
                return false;
            }
        }
        else {
            return false;
        }
    }

    private void beginListeningForData() {
        Thread btListeningThread = new Thread(new Runnable() {
            @Override
            public void run() {
                int byteCount;
                String btReadData;
                byte[] rawBytes;
                stopThread = false;
                while (!stopThread) {
                    try {
                        byteCount = btInputStream.available();
                        if (byteCount > 0) {
                            rawBytes = new byte[byteCount];
                            btInputStream.read(rawBytes);
                            btReadData = new String(rawBytes, "UTF-8");
                            stringToSendToUnity = btReadData;
                        }
                    } catch (IOException e) {
                        e.printStackTrace();
                        stopThread = true;
                    }
                }
            }
        });
        btListeningThread.start();
    }

    public String getReadData() {
        return stringToSendToUnity;
    }

    public void closeBtConnection() throws IOException {
        stopThread = true;
        btInputStream.close();
        btSocket.close();
    }
}
