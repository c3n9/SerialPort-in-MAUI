using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Hardware.Usb;
using Android.Net.Nsd;
using Android.OS;
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
        ListView listView;

        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			usbManager = GetSystemService(Context.UsbService) as UsbManager;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			UnregisterReceiver(_usbReceiver);
		}
	}
}
