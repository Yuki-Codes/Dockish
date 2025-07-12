// Dockish
namespace Dockish;

using System.Diagnostics;
using System.Windows.Controls;
using DependencyPropertyGenerator;
using Windows.Win32;
using Windows.Win32.Foundation;

[DependencyProperty<bool>("ShowBackground")]
[DependencyProperty<bool>("ShowDock")]
public partial class Dock : ContentControl
{
	public Dock()
	{
		Task.Run(this.Run);
	}

	private async Task Run()
	{
		while (true)
		{
			await Task.Delay(100);

			try
			{
				bool isOverlaped = false;

				foreach (Process pList in Process.GetProcesses())
				{
					if (pList.MainWindowTitle == ""
						|| pList.MainWindowTitle == "Settings"
						|| pList.MainWindowTitle == "Windows Input Experience")
						continue;

					RECT r;
					if (PInvoke.GetWindowRect((HWND)pList.MainWindowHandle, out r) == false)
						continue;

					if (r.Y > -30000 && r.Y < 32)
					{
						isOverlaped = true;
					}
				}

				await this.Dispatcher.InvokeAsync(() =>
				{
					this.ShowBackground = isOverlaped;

					this.ShowDock = this.IsMouseOver || !isOverlaped;
				});
			}
			catch (Exception ex)
			{
			}
		}
	}
}