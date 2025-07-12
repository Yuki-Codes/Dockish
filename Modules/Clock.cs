// Dockish
namespace Dockish.Modules;

using System.Windows.Threading;
using DependencyPropertyGenerator;

[DependencyProperty<DateTime>("DateTime")]
public partial class Clock : ModuleBase
{
	private readonly DispatcherTimer timer;

	public Clock()
	{
		this.timer = new();
		this.timer.Tick += this.OnTick;
		this.timer.Interval = new TimeSpan(0, 0, 1);
		this.timer.Start();
	}

	private void OnTick(object? sender, EventArgs e)
	{
		this.DateTime = DateTime.Now;
	}
}