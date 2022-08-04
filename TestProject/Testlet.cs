namespace TestProject
{
	public class Testlet
	{
		private const int ItemsAmount = 10;
		private const int PretestAmount = 4;
		private const int OperationalAmount = 6;

		private readonly Random rand = new();

		public string TestletId;
		private readonly List<Item> Items;
		public Testlet(string testletId, List<Item> items)
		{
			TestletId = testletId;
			Items = items;
		}
		public List<Item> Randomize()
		{
			if (Items?.Count != ItemsAmount)
			{
				throw new Exception($"Testlet items amout should be {ItemsAmount}");
			}

			var pretests = Items.Where(i => i.ItemType == ItemTypeEnum.Pretest).ToList();
			var operationals = Items.Where(i => i.ItemType == ItemTypeEnum.Operational).ToList();
			if (pretests.Count != PretestAmount || operationals.Count != OperationalAmount)
			{
				throw new Exception("Incorrect Testlet set of items");
			}

			var randomList = new List<Item>();

			var shufflePretests = pretests.OrderBy(p => rand.Next()).ToList();
			randomList.AddRange(shufflePretests.Take(2));
			var shuffleOperationals = operationals.Concat(shufflePretests.Skip(2)).OrderBy(p => rand.Next()).ToList();
			randomList.AddRange(shuffleOperationals);

			return randomList;
		}
	}
	public class Item
	{
		public string ItemId;
		public ItemTypeEnum ItemType;
	}
	public enum ItemTypeEnum
	{
		Pretest = 0,
		Operational = 1
	}
}