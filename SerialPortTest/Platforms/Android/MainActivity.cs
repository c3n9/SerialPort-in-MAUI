using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Hardware.Usb;
using Android.Net.Nsd;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using SerialPortTest.Platforms.Android;

[assembly: UsesFeature("android.hardware.usb.host")]

namespace SerialPortTest
{
	[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
	[IntentFilter(new[] { UsbManager.ActionUsbDeviceAttached, UsbManager.ActionUsbDeviceDetached })]
    [MetaData(UsbManager.ActionUsbDeviceAttached, Resource = "@xml/device_filter")]


    public class MainActivity : MauiAppCompatActivity
	{
		private UsbReceiver _usbReceiver;
        UsbManager usbManager;

        protected override void OnCreate(Bundle savedInstanceState)
		{
            base.OnCreate(savedInstanceState);

            // Проверка разрешения BLUETOOTH_CONNECT
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.BluetoothConnect)
                != (int)Permission.Granted)
            {
                // Запрос разрешения
                ActivityCompat.RequestPermissions(this,
                    new string[] { Manifest.Permission.BluetoothConnect }, 0);
            }

            usbManager = GetSystemService(Context.UsbService) as UsbManager;

            _usbReceiver = new UsbReceiver();
            RegisterReceiver(_usbReceiver, new IntentFilter(UsbManager.ActionUsbDeviceAttached));
            RegisterReceiver(_usbReceiver, new IntentFilter(UsbManager.ActionUsbDeviceDetached));

            DependencyService.Register<IUsbService, UsbServiceAndroid>();
        }

		protected override void OnDestroy()
		{
			base.OnDestroy();
			UnregisterReceiver(_usbReceiver);
		}

	}
}
