using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Hardware.Usb;
using Android.OS;
using SerialPortTest.Platforms.Android;

namespace SerialPortTest
{
	[
		Activity(Theme = "@style/Maui.SplashTheme", 
		MainLauncher = true, 
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)
	]
	[IntentFilter(new[] { UsbManager.ActionUsbDeviceAttached, UsbManager.ActionUsbDeviceDetached })]
	public class MainActivity : MauiAppCompatActivity
	{
		private UsbReceiver _usbReceiver;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			_usbReceiver = new UsbReceiver();
			RegisterReceiver(_usbReceiver, new IntentFilter(UsbManager.ActionUsbDeviceAttached));
			RegisterReceiver(_usbReceiver, new IntentFilter(UsbManager.ActionUsbDeviceDetached));
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			UnregisterReceiver(_usbReceiver);
		}
	}
}
