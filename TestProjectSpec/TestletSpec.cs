using TestProject;

namespace TestProjectSpec
{
	public class Tests
	{
		private Testlet _testlet;

		[SetUp]
		public void SetUp()
		{
			_testlet = new Testlet(
				"1",
				new List<Item>
				{
					new Item { ItemId = "1", ItemType = ItemTypeEnum.Pretest },
					new Item { ItemId = "2", ItemType = ItemTypeEnum.Operational },
					new Item { ItemId = "3", ItemType = ItemTypeEnum.Operational },
					new Item { ItemId = "4", ItemType = ItemTypeEnum.Operational },
					new Item { ItemId = "5", ItemType = ItemTypeEnum.Pretest },
					new Item { ItemId = "6", ItemType = ItemTypeEnum.Operational },
					new Item { ItemId = "7", ItemType = ItemTypeEnum.Pretest },
					new Item { ItemId = "8", ItemType = ItemTypeEnum.Pretest },
					new Item { ItemId = "9", ItemType = ItemTypeEnum.Operational },
					new Item { ItemId = "10", ItemType = ItemTypeEnum.Operational }
				});
		}

		[TestCase(9)]
		[TestCase(11)]
		public void Testlet_ItemsAmountNot10_ThrowsError(int length)
		{
			var list = new List<Item>(length);
			list.AddRange(Enumerable.Repeat(new Item(), length));

			var exception = Assert.Throws<Exception>(() => new Testlet("1", list));
			Assert.That(exception?.Message, Is.EqualTo("Testlet items amount should be 10"));
		}

		[Test]
		public void Testlet_SevenOperationalItems_ThrowsError()
		{
			var list = new List<Item>(10);
			list.AddRange(Enumerable.Repeat(new Item { ItemType = ItemTypeEnum.Operational }, 7));
			list.AddRange(Enumerable.Repeat(new Item { ItemType = ItemTypeEnum.Pretest }, 3));

			var exception = Assert.Throws<Exception>(() => new Testlet("1", list));
			Assert.That(exception?.Message, Is.EqualTo("Incorrect Testlet set of items"));
		}

		[Test]
		public void Randomize_FirstTwoItemsArePretest_ReturnsTrue()
		{
			var items = _testlet.Randomize();
			
			Assert.Multiple(() =>
			{
				Assert.That(items[0].ItemType, Is.EqualTo(ItemTypeEnum.Pretest));
				Assert.That(items[1].ItemType, Is.EqualTo(ItemTypeEnum.Pretest));
			});
		}

		[Test]
		public void Randomize_LastEightItemsContainTwoPretest_ReturnsTrue()
		{
			var items = _testlet.Randomize().Skip(2).Where(i => i.ItemType == ItemTypeEnum.Pretest);

			Assert.That(items.Count(), Is.EqualTo(2));
		}

		[Test]
		public void Randomize_ItemsHaveDublicates_ReturnsFalse()
		{
			var duplicates = _testlet.Randomize().GroupBy(i => i.ItemId).Where(g => g.Count() > 1);

			Assert.That(duplicates.Any(), Is.False);
		}
	}
}