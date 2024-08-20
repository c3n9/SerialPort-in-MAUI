using System;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core;
using System.Threading;
using CommunityToolkit.Maui.Alerts;

namespace SerialPortTest
{
	public partial class MainPage : ContentPage
	{
		private IUsbService _usbService;
		CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		public MainPage()
		{
			InitializeComponent();
			_usbService = DependencyService.Get<IUsbService>();
		}

		private async void BSend_Clicked(System.Object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(ENumber.Text))
			{
				await _usbService.SendMessageAsync(ENumber.Text);
				await DisplayAlert("Успешно", "Сообщение отправлено", "OK");

				var message = await _usbService.ReadMessageAsync();
				if (!string.IsNullOrWhiteSpace(message))
				{
					await Toast.Make(message, ToastDuration.Long, 16).Show(cancellationTokenSource.Token);
				}
			}
			else
			{
				await DisplayAlert("Ошибка", "Введите сообщение", "OK");
			}

		}

		private async void BConnect_Clicked(System.Object sender, System.EventArgs e)
		{
			if (PComPorts.SelectedItem == null)
			{
				await DisplayAlert("Ошибка", "Выберите COM порт или Bluetooth устройство", "OK");
				return;
			}

			string selectedDevice = PComPorts.SelectedItem.ToString();
			bool success = false;

			// Определяем, является ли выбранное устройство USB или Bluetooth
			var usbPorts = await _usbService.GetAvailablePortsAsync();
			var bluetoothDevices = await _usbService.GetAvailableBluetoothDevicesAsync();

			if (usbPorts.Contains(selectedDevice))
			{
				// Подключение к USB порту
				success = await _usbService.ConnectAsync(selectedDevice);
				if (success)
				{
					await Toast.Make("Вы подключились к COM порту", ToastDuration.Long, 16).Show(cancellationTokenSource.Token);
				}
			}
			else if (bluetoothDevices.Contains(selectedDevice))
			{
				// Подключение к Bluetooth устройству
				success = await _usbService.ConnectBluetoothAsync(selectedDevice);
				if (success)
				{
					await Toast.Make("Вы подключились к Bluetooth устройству", ToastDuration.Long, 16).Show(cancellationTokenSource.Token);
				}
			}

			if (!success)
			{
				await Toast.Make("Не удалось подключиться к устройству", ToastDuration.Long, 16).Show(cancellationTokenSource.Token);
			}

		}

		private async void BGetPorts_Clicked(object sender, EventArgs e)
		{
			try
			{
				var ports = await _usbService.GetAvailablePortsAsync();
				var bluetoothDevices = await _usbService.GetAvailableBluetoothDevicesAsync();

				var allDevices = ports.Concat(bluetoothDevices).ToList();
				PComPorts.ItemsSource = allDevices;
			}
			catch (Exception ex)
			{
				await DisplayAlert("Ошибка", ex.Message, "OK");
			}
		}
	}
}