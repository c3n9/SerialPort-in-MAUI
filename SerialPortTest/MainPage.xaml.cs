using RJCP.IO.Ports;

namespace SerialPortTest
{
	public partial class MainPage : ContentPage
	{
		SerialPortStream _serialPort;
		public MainPage()
		{
			InitializeComponent();
		}

		private void BSend_Clicked(System.Object sender, System.EventArgs e)
		{
			if(_serialPort != null && _serialPort.IsOpen)
			{
				_serialPort.Write(ENumber.Text);
			}
		}

		private async void BConnect_Clicked(System.Object sender, System.EventArgs e)
		{
			if (PComPorts.SelectedItem == null)
			{
				await DisplayAlert("Ошибка", "Выберите COM порт", "OK");
				return;
			}
			_serialPort = new SerialPortStream(PComPorts.SelectedItem as string, 9600);
			try
			{
				_serialPort.Open();
				await DisplayAlert("Успешно", "Вы подключились к COM порту", "OK");

			}
			catch(Exception ex)
			{
				await DisplayAlert("Ошибка", ex.Message, "OK");
			}
		}

		private async void BGetPorts_Clicked(object sender, EventArgs e)
		{
			try
			{
				var ports = SerialPortStream.GetPortNames();
				PComPorts.ItemsSource = ports;
			}
			catch(Exception ex)
			{
				await DisplayAlert("Ошибка", ex.Message, "OK");
			}
		}
	}

}
