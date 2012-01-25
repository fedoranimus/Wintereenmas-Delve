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
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.storyTelling;

namespace WintereenmasDelve2012
{
	/// <summary>
	/// Interaction logic for HeroesLose.xaml
	/// </summary>
	public partial class HeroesLose : Page
	{
		private StoryTeller _storyTeller;

		public EventHandler Complete;

		public HeroesLose(StoryTeller storyTeller)
		{
			InitializeComponent();

			_storyTeller = storyTeller;

			Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, EventArgs args)
		{
			Story story = new Story();
			story.Add(_storyTeller.NarratorVoice, "All of the Heroes have died.  Xargon wins!  Better luck next time, Heroes.");

			_storyTeller.StoryComplete += OnStoryComplete;
			_storyTeller.TellStory(story);
		}

		private void OnStoryComplete(object sender, EventArgs args)
		{
			_storyTeller.StoryComplete -= OnStoryComplete;

			if (Complete != null)
			{ Complete(this, new EventArgs()); }
		}
	}
}
