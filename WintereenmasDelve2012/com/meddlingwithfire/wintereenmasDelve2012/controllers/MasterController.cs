using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game.quests;
using System.Windows.Navigation;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.chance;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.avatarClasses;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.storyTelling;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game
{
	public class MasterController
	{
		private NavigationService _navigationService;
		private ChanceProvider _chanceProvider;
		private StoryTeller _storyTeller;

		private List<AbstractQuest> _quests;

		private List<Hero> _heroes;
		
		public MasterController(NavigationService navigationService)
			: base()
		{
			_navigationService = navigationService;
			_storyTeller = new StoryTeller();
		}

		public void TakeOverExperience()
		{
			//// First, go to the Chance Gatherer
			//ChanceGatherer chanceGatherer = new ChanceGatherer();
			//chanceGatherer.Complete += OnChanceGathererComplete;
			//_navigationService.Navigate(chanceGatherer);

			// This skips straight to the QuestView
			List<ChanceSubmission> submissions = new List<ChanceSubmission>();
			submissions.Add(new ChanceSubmission() { SubmitterName = "Jonathon", Value = 1 });
			submissions.Add(new ChanceSubmission() { SubmitterName = "Jonathon", Value = 2 });
			submissions.Add(new ChanceSubmission() { SubmitterName = "Jonathon", Value = 3 });
			_chanceProvider = new ChanceProvider(submissions);
			OnGameIntroductionComplete(null, null);
		}

		private void OnChanceGathererComplete(object sender, EventArgs args)
		{
			if (sender is ChanceGatherer)
			{
				ChanceGatherer chanceGatherer = (ChanceGatherer)sender;
				List<ChanceSubmission> submissions = chanceGatherer.Submissions;
				_chanceProvider = new ChanceProvider(submissions);

				// Go to the introduction
				GameIntroduction gameIntroduction = new GameIntroduction(_storyTeller);
				gameIntroduction.Complete += OnGameIntroductionComplete;
				_navigationService.Navigate(gameIntroduction);
			}
		}

		private void OnGameIntroductionComplete(object sender, EventArgs args)
		{
			// Fire up the first Quest!
			_heroes = new List<Hero>();
			_heroes.Add(new Hero(new AvatarClassBarbarian(_chanceProvider)));
			_heroes.Add(new Hero(new AvatarClassDwarf(_chanceProvider)));
			_heroes.Add(new Hero(new AvatarClassElf(_chanceProvider)));
			_heroes.Add(new Hero(new AvatarClassWizard(_chanceProvider)));

			TheTrial questOne = new TheTrial(_heroes);
			QuestView questView = new QuestView(questOne, _chanceProvider, _storyTeller);
			questView.HeroesWin += OnQuestHeroesWin;
			questView.HeroesLose += OnQuestHeroesLose;
			_navigationService.Navigate(questView);
		}

		private void OnQuestHeroesWin(object sender, EventArgs args)
		{

		}

		private void OnQuestHeroesLose(object sender, EventArgs args)
		{
			HeroesLose heroesLose = new HeroesLose(_storyTeller);
			heroesLose.Complete += OnHeroesLoseComplete;
			_navigationService.Navigate(heroesLose);
		}

		private void OnHeroesLoseComplete(object sender, EventArgs args)
		{
			if (sender is HeroesLose)
			{
				HeroesLose heroesLose = (HeroesLose)sender;
				heroesLose.Complete += OnHeroesLoseComplete;
			}

			ChanceGatherer chanceGatherer = new ChanceGatherer();
			chanceGatherer.Complete += OnChanceGathererComplete;
			_navigationService.Navigate(chanceGatherer);
		}
	}
}
