using Android.Content;
using Android.Hardware.Usb;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortTest.Platforms.Android
{
	public class UsbReceiver : BroadcastReceiver
	{
		public override void OnReceive(Context context, Intent intent)
		{
			var usbDevice = (UsbDevice)intent.GetParcelableExtra(UsbManager.ExtraDevice);
			if (intent.Action == UsbManager.ActionUsbDeviceAttached)
			{
				Toast.MakeText(context, "USB устройство подключено", ToastLength.Short).Show();
			}
			else if (intent.Action == UsbManager.ActionUsbDeviceDetached)
			{
				Toast.MakeText(context, "USB устройство отключено", ToastLength.Short).Show();
			}
		}
	}
}
