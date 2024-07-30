#  Serial Port in MAUI (Android)

## Getting start

1. Reference the [library](https://github.com/anotherlab/UsbSerialForAndroid?tab=readme-ov-file) to your project. We will use the source code, since there is no such library on nuget.
   
2. Copy the [device_filter.xml](https://github.com/anotherlab/UsbSerialForAndroid/blob/main/UsbSerialExampleApp/Resources/xml/device_filter.xml) from the example app to your Platforms/Android/Resources folder. Make sure that the Build Action is set to AndroidResource.

3. Add the following attribute to the main activity to enable the USB Host.

```
[assembly: UsesFeature("android.hardware.usb.host")]
```

4. Add the following IntentFilter to the main activity to receive USB device attached notifications.

```
[IntentFilter(new[] { UsbManager.ActionUsbDeviceAttached })]
```

5. Add the MetaData attribute to associate the device_filter with the USB attached event to only see the devices that we are looking for.

```
[MetaData(UsbManager.ActionUsbDeviceAttached, Resource = "@xml/device_filter")]
```

6. Add a global [interface](https://github.com/c3n9/SerialPortTestInAndroid/blob/master/SerialPortTest/IUsbService.cs) to implement an action with our controller.

```
public interface IUsbService
{
  Task<IEnumerable<string>> GetAvailablePortsAsync();
  Task<bool> ConnectAsync(string portName);
  Task SendMessageAsync(string message);
}
```

7. Add the [UsbServiceAndroid](https://github.com/c3n9/SerialPort-in-MAUI/blob/master/SerialPortTest/Platforms/Android/UsbServiceAndroid.cs) class to the Android folder, which will implement the created interface.
   
8. With the help of [DependencyService](https://github.com/c3n9/SerialPortTestInAndroid/blob/master/SerialPortTest/MainPage.xaml.cs), we will turn to platform-dependent code and you can work safely with our controller.


## Arduino Firmware Code

```
char commandValue; // Data received from the serial port
int ledPin = 13; // Built-in LED pin

void setup() {
  pinMode(ledPin, OUTPUT); // Set the pin mode to output
  Serial.begin(9600); // Initialize serial communication at 9600 bits per second
}

void loop() {
  if (Serial.available()) { // Check if data is available to read
    commandValue = Serial.read(); // Read the incoming data

    if (commandValue == '1') { // If the received data is '1'
      Serial.println("1"); // Send '1' to serial monitor
      digitalWrite(ledPin, HIGH); // Turn on the LED
    }
    else if (commandValue == '0') { // If the received data is '0'
      Serial.println("2"); // Send '2' to serial monitor
      digitalWrite(ledPin, LOW); // Turn off the LED
    }
  }
  delay(10); // Wait for 10 milliseconds before the next loop iteration
}
```

### Everything has been tested using an Arduino MEGA 2560 and a Google Pixel 7a smartphone.
