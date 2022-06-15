	[CreateNodeMenu(MENU_PARENT_NAME + "Max")]
	public class MaxNode : AbsTwoModNode
	{
		public override Generator GetGenerator()
		{
			if (!HasBothGenerators())
			{
				return null;
			}

			return new Max(GetGenerator1(), GetGenerator2());
		}

		public override string GetTitle()
		{
			return "Max";
		}
	}
