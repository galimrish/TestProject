namespace TestProject
{
	public class Testlet
	{
		private const int ItemsAmount = 10;
		private const int PretestAmount = 4;
		private const int OperationalAmount = 6;

		private readonly Random rand = new();

		public string TestletId;
		private readonly IEnumerable<Item> Items;
		public Testlet(string testletId, IEnumerable<Item> items)
		{
			TestletId = testletId;
			Items = items;
			InitialValidation();
		}

		public List<Item> Randomize()
		{
			var randomList = new List<Item>();

			var shufflePretests = ShuffleItems(GetPretestItems());
			randomList.AddRange(shufflePretests.Take(2));
			var shuffleOperationals = ShuffleItems(GetOperationalItems().Concat(shufflePretests.Skip(2)));
			randomList.AddRange(shuffleOperationals);

			return randomList;
		}

		private void InitialValidation()
		{
			if (Items?.Count() != ItemsAmount)
			{
				throw new Exception($"Testlet items amout should be {ItemsAmount}");
			}

			if (GetOperationalItems().Count != OperationalAmount || GetPretestItems().Count != PretestAmount)
			{
				throw new Exception("Incorrect Testlet set of items");
			}
		}

		private List<Item> GetOperationalItems() => Items.Where(i => i.ItemType == ItemTypeEnum.Operational).ToList();
		private List<Item> GetPretestItems() => Items.Where(i => i.ItemType == ItemTypeEnum.Pretest).ToList();
		private List<Item> ShuffleItems(IEnumerable<Item> items) => items.OrderBy(p => rand.Next()).ToList();

	}
}