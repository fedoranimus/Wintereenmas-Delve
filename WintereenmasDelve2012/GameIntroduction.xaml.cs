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
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game.quests;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.storyTelling;

namespace WintereenmasDelve2012
{
	/// <summary>
	/// Interaction logic for GameIntroduction.xaml
	/// </summary>
	public partial class GameIntroduction : Page
	{
		private StoryTeller _storyTeller;

		public EventHandler Complete;

		public GameIntroduction(StoryTeller storyTeller)
		{
			InitializeComponent();

			_storyTeller = storyTeller;

			Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, EventArgs args)
		{
			Loaded -= OnLoaded;

			Story introduction = new Story();
			introduction.Add(_storyTeller.NarratorVoice, "The fire burns warmly, but casts little light into Mentor's study.  The flickering shadows only hint at the vast number of books and scrolls that fill the many shelves.  Slowly Mentor walks over to the fire.");
			introduction.Add(_storyTeller.MentorVoice, "Well, my friends, your training is complete.  You are not yet true Heroes, you have yet to prove yourselves.  But first, let me tell you of Zargon.");
			introduction.Add(_storyTeller.MentorVoice, "Many centuries ago, Zargon was my apprentice.  He worked hard and learned quickly.  But impatience devoured him.  He wanted to learn more powerful magic.  I told him of the dangers, and that he should be patient, for in time he would become a great sorcerer.  But Zargon could not wait;  each night he broke into my study and read my spell books.  The secrets that were held within them were great indeed.  Once he learned these secrets, Zargon fled.");
			introduction.Add(_storyTeller.MentorVoice, "When I caught up with him, I found him greatly changed.  He had pledged his allegiance to the Great Powers of Chaos.  Fool!  He saw magic only as a short-cut to power and paid no heed to the terrible price he would have to pay.  I tried to reason with him, but to no avail.  He laughed in my face and then unleashed a terrible spell which I was hard-pressed to counter.  For many days we battled, but Zargon had allies stronger even than I, and I could not defeat him.  In the end, as we both weakened, he fled and sought refuge in the Northern Chaos Wastes.  There he licked his wounds and honed his skills, conjuring ancient powers with which to overthrow the Empire.");
			introduction.Add(_storyTeller.MentorVoice, "I must watch Zargon and measure the strength of his magic.  The powers Zargon has called upon will destroy us all if I relax from this vigil.  Zargon's legions threatened us once before.  Then it was Rogar who aided me and defeated them.  Now they are on the march again; already they have assailed the Borderlands.  The Empire must again look for Heroes and to this end have I trained you.");
			introduction.Add(_storyTeller.MentorVoice, "Each of you must complete 14 quests.  If you do this, you will be acclaimed as Champions of the Realm and dubbed Imperial Knights.  Only then will you be on the road to becoming true Heroes.  I shall speak with you again on your return.  If you return.");

			_storyTeller.StoryComplete += OnStoryComplete;
			_storyTeller.TellStory(introduction);
		}

		private void OnStoryComplete(object sender, EventArgs args)
		{
			_storyTeller.StoryComplete -= OnStoryComplete;
			if (Complete != null)
			{ Complete(this, new EventArgs()); }
		}
	}
}
