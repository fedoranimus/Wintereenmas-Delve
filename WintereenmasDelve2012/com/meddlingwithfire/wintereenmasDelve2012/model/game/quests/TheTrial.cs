using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PathFinding;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.quests.maps.tiles;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.storyTelling;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.avatarClasses;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.game.quests
{
	public class TheTrial : AbstractQuest
	{
		public TheTrial(List<Hero> heroes):base(new Point(1, 14), heroes)
		{
			// Configure the story
			StoryTeller storyTeller = new StoryTeller();
			IntroductionStory = new Story();
			IntroductionStory.Add(storyTeller.NarratorVoice, "Quest one.  The Trial.");
			//IntroductionStory.Add(storyTeller.MentorVoice, "You have learned well, my friends.  Now has come the time of your first trial.  You must first enter the catacombs which contain Fellmarg's Tomb.  You must seek out and destroy Verag, a foul Gargoyle who hides in the catacombs.  This Quest is not easy and you must work together in order to survive.  This is your first step on the road to becoming true Heroes.  Tread carefully my friends.");

			// Prepare the map
			Map.SetBaseAreaWithTile(new Point(0, 0), 26, 1, new FloorBlockedTile());
			Map.SetBaseAreaWithTile(new Point(12, 0), 2, 5, new FloorBlockedTile());
			Map.SetBaseAreaWithTile(new Point(14, 1), 12, 5, new FloorBlockedTile());
			Map.SetBaseAreaWithTile(new Point(17, 6), 9, 7, new FloorBlockedTile());
			Map.SetBaseAreaWithTile(new Point(18, 13), 8, 5, new FloorBlockedTile());
			Map.SetBaseAreaWithTile(new Point(14, 18), 12, 1, new FloorBlockedTile());
			Map.SetBaseAreaWithTile(new Point(5, 10), 4, 3, new FloorBlockedTile());

			Map.SetBaseTile(0, 1, new RockTile());
			Map.SetBaseTile(12, 5, new RockTile());
			Map.SetBaseTile(13, 5, new RockTile());
			Map.SetBaseTile(17, 9, new RockTile());
			Map.SetBaseTile(14, 18, new RockTile());

			List<TwoLinkedPoints> doorLocations = new List<TwoLinkedPoints>();
			doorLocations.Add(new TwoLinkedPoints(3, 4, 3, 3));
			doorLocations.Add(new TwoLinkedPoints(4, 2, 5, 2));
			doorLocations.Add(new TwoLinkedPoints(8, 2, 9, 2));
			doorLocations.Add(new TwoLinkedPoints(3, 8, 3, 9));
			doorLocations.Add(new TwoLinkedPoints(0, 11, 1, 11));
			doorLocations.Add(new TwoLinkedPoints(3, 18, 3, 17));
			doorLocations.Add(new TwoLinkedPoints(7, 17, 7, 18));
			doorLocations.Add(new TwoLinkedPoints(8, 15, 9, 15));
			doorLocations.Add(new TwoLinkedPoints(10, 12, 10, 13));
			doorLocations.Add(new TwoLinkedPoints(13, 15, 14, 15));
			doorLocations.Add(new TwoLinkedPoints(15, 9, 16, 9));

			//Map.RemoveBarriersBetween(1, 17, 1, 18); // Debug, chop a hole to test line of sight.

			foreach (TwoLinkedPoints linkedPoints in doorLocations)
			{ Map.RemoveBarriersBetween(linkedPoints.PointA, linkedPoints.PointB); }

			// Calculate nodes
			Map.CalculatePathfindingGraph();

			foreach (TwoLinkedPoints linkedPoints in doorLocations)
			{ Map.AddDoorBetween(linkedPoints.PointA, linkedPoints.PointB); }

			Map.AddTile(10, 5, new TreasureChestHorizontalTile());
			Map.AddTile(11, 7, new TreasureChestHorizontalTile());
			Map.AddTile(17, 16, new TreasureChestVerticalTile());

			//AddMonster(new Monster(new AvatarClassOrc()), new Point(7, 14));
			//AddMonster(new Monster(new AvatarClassOrc()), new Point(8, 15));
		}
	}
}
