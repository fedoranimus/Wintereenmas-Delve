using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.items;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.movement;
using WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.actionStrategies;

namespace WintereenmasDelve2012.com.meddlingwithfire.wintereenmasDelve2012.model.game.avatarClasses
{
	public class AvatarClass
	{
		public int BaseAttackDice;
		public int BaseDefendDice;

		public int BaseMaximumBodyPoints;
		public int BaseMaximumMindPoints;

		public IAvatarMovementBehavior BaseMovementBehavior;

		public String ImagePath;
		public String Description;

		public List<AbstractActionStrategy> ActionStrategies;

		public AvatarClass(int baseAttackDice, int baseDefendDice, int baseMaximumBodyPoints, int baseMaximumMindPoints, IAvatarMovementBehavior movementBehavior, String imagePath, String description)
			: base()
		{
			BaseAttackDice = baseAttackDice;
			BaseDefendDice = baseDefendDice;
			BaseMaximumBodyPoints = baseMaximumBodyPoints;
			BaseMaximumMindPoints = baseMaximumMindPoints;
			BaseMovementBehavior = movementBehavior;
			ImagePath = imagePath;
			Description = description;

			ActionStrategies = new List<AbstractActionStrategy>();
		}

		public virtual List<Item> GenerateStartingItems()
		{ return new List<Item>(); }
	}
}
