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
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.chance;
using System.Windows.Threading;

namespace WintereenmasDelve2012
{
	/// <summary>
	/// Interaction logic for ChanceGatherer.xaml
	/// </summary>
	public partial class ChanceGatherer : Page
	{
		private const int MINIMUM_SUBMISSIONS_TO_PLAY = 5;
		private const int MINIUM_NUMBER_OF_SECONDS_BETWEEN_SUBMISSIONS = 2;

		private List<ChanceSubmission> _submissions;

		private DispatcherTimer _timer;

		public EventHandler Complete;

		public ChanceGatherer()
		{
			InitializeComponent();

			Loaded += OnLoaded;
		}

		public List<ChanceSubmission> Submissions
		{
			get { return _submissions; }
		}

		private void OnLoaded(object sender, EventArgs args)
		{
			_submissions = new List<ChanceSubmission>();
			_timer = new DispatcherTimer();
			_timer.Interval = new TimeSpan(0, 0, 0, MINIUM_NUMBER_OF_SECONDS_BETWEEN_SUBMISSIONS);
			_timer.Tick += OnTimerTick;
		}

		private void OnTimerTick(object sender, EventArgs args)
		{
			// Enable the input again

			xamlAllInput.Visibility = Visibility.Visible;
			xamlInputRequest.Text = "Xargon Commands You To Enter A Number Between 1 and 6";

			_timer.Stop();
		}

		private void OnDoneClick(object sender, RoutedEventArgs e)
		{
			ChanceSubmission submission = null;
			try
			{
				submission = new ChanceSubmission();
				submission.SubmitterName = xamlNameInput.Text;
				submission.Value = Int16.Parse(xamlValueInput.Text);
			}
			catch(Exception exception)
			{ submission = null; }

			if (submission == null || submission.Value < 1 || submission.Value > 6)
			{ xamlInputRequest.Text = "FOOL!  Xargon Commands You To Enter A Number Between One and Six!"; }
			else
			{
				xamlAllInput.Visibility = Visibility.Hidden;
				xamlInputRequest.Text = "\nXargon Accepts Your Pitiful Answer...";
				xamlNameInput.Text = "";
				xamlValueInput.Text = "";

				_timer.Start();

				_submissions.Add(submission);

				if (_submissions.Count >= MINIMUM_SUBMISSIONS_TO_PLAY)
				{
					if (Complete != null)
					{ Complete(this, new EventArgs()); }
				}
			}
		}
	}
}
