using Android.App;
using Android.Content;
using Android.Hardware.Usb;
using Hoho.Android.UsbSerial.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: UsesFeature("android.hardware.usb.host")]

namespace SerialPortTest
{
	public class UsbServiceAndroid : IUsbService
	{
		private UsbManager _usbManager;
		private UsbSerialPort _connectedPort;

		public UsbServiceAndroid()
		{
			_usbManager = (UsbManager)Android.App.Application.Context.GetSystemService(Context.UsbService);
		}

		public async Task<IEnumerable<string>> GetAvailablePortsAsync()
		{
			var deviceList = _usbManager.DeviceList.Values;
			return deviceList.Select(device => device.DeviceName).ToList();
		}

		public async Task<bool> ConnectAsync(string portName)
		{
			var deviceList = _usbManager.DeviceList.Values;
			var device = deviceList.FirstOrDefault(d => d.DeviceName == portName);
			if (device != null)
			{
				// Проверяем наличие разрешения на использование устройства
				var permissionGranted = _usbManager.HasPermission(device);
				if (!permissionGranted)
				{
					// Запрашиваем разрешение
					var permissionIntent = PendingIntent.GetBroadcast(Android.App.Application.Context, 0, new Intent("com.example.USB_PERMISSION"), PendingIntentFlags.UpdateCurrent);
					_usbManager.RequestPermission(device, permissionIntent);

					// Ожидание разрешения
					await Task.Delay(1000);
				}

				var drivers = UsbSerialProber.GetDefaultProber().FindAllDrivers(_usbManager);
				var driver = drivers.FirstOrDefault(d => d.Device.Equals(device));
				if (driver != null)
				{
					_connectedPort = driver.Ports.FirstOrDefault();
					if (_connectedPort != null)
					{
						var connection = _usbManager.OpenDevice(device);
						if (connection != null)
						{
							_connectedPort.Open(connection);
							// Настройка параметров порта
							_connectedPort.SetParameters(9600, 8, StopBits.One, Parity.None);
							return true;
						}
					}
				}
			}
			return false;
		}

		public async Task SendMessageAsync(string message)
		{
			if (_connectedPort != null)
			{
				try
				{
					// Конвертируем сообщение в массив байт
					byte[] messageBytes = System.Text.Encoding.ASCII.GetBytes(message);
					// Отправляем сообщение
					await Task.Run(() => _connectedPort.Write(messageBytes, 1000)); // 1000 мс таймаут
				}
				catch (Exception ex)
				{
					// Обработка ошибок
					System.Diagnostics.Debug.WriteLine($"Ошибка отправки сообщения: {ex.Message}");
				}
			}
		}
        public async Task<string> ReadMessageAsync()
        {
            if (_connectedPort != null)
            {
                try
                {
                    // Буфер для чтения данных
                    byte[] buffer = new byte[1024];
                    // Чтение данных из порта
                    int numBytesRead = await Task.Run(() => _connectedPort.Read(buffer, 1000)); // Таймаут 1000 мс
                    if (numBytesRead > 0)
                    {
                        // Конвертация прочитанных байт в строку
                        return Encoding.ASCII.GetString(buffer, 0, numBytesRead);
                    }
                }
                catch (Exception ex)
                {
                    // Обработка ошибок
                    System.Diagnostics.Debug.WriteLine($"Ошибка чтения сообщения: {ex.Message}");
                }
            }
            return string.Empty; // Возвращаем пустую строку, если данные не были прочитаны
        }
    }
}