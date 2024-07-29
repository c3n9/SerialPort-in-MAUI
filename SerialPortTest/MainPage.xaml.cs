using System;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace SerialPortTest
{
	public partial class MainPage : ContentPage
	{
		private IUsbService _usbService;

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
				await DisplayAlert("Ошибка", "Выберите COM порт", "OK");
				return;
			}
			var success = await _usbService.ConnectAsync(PComPorts.SelectedItem.ToString());
			if (success)
			{
				await DisplayAlert("Успешно", "Вы подключились к COM порту", "OK");
			}
			else
			{
				await DisplayAlert("Ошибка", "Не удалось подключиться к COM порту", "OK");
			}
		}

		private async void BGetPorts_Clicked(object sender, EventArgs e)
		{
			try
			{
				var ports = await _usbService.GetAvailablePortsAsync();
				PComPorts.ItemsSource = ports.ToList();
			}
			catch (Exception ex)
			{
				await DisplayAlert("Ошибка", ex.Message, "OK");
			}
		}
	}
}