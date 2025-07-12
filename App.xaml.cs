// Dockish

namespace Dockish;

using System.Windows;

public partial class App : Application
{
	private readonly MainWindow window = new();

	protected override void OnStartup(StartupEventArgs e)
	{
		this.window.Show();
		base.OnStartup(e);
	}
}