using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Speech.Synthesis;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game;

namespace WintereenmasDelve2012
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : NavigationWindow
	{
		private MasterController _masterController;

		public MainWindow()
		{
			InitializeComponent();
			
			Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, EventArgs args)
		{
			Loaded -= OnLoaded;

			_masterController = new MasterController(NavigationService);
			_masterController.TakeOverExperience();
		}
	}
}
