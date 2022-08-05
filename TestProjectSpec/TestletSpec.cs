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
		public void Testlet_ItemsAmountNot10_ThrowsException(int length)
		{
			var list = new List<Item>(length);
			list.AddRange(Enumerable.Repeat(new Item(), length));

			var exception = Assert.Throws<Exception>(() => new Testlet("1", list));
			Assert.That(exception?.Message, Is.EqualTo("Testlet items amount should be 10"));
		}

		[Test]
		public void Testlet_SevenOperationalItems_ThrowsException()
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
		public void Randomize_LastEightItemsContainTwoPretestsAndSixOperationals_ReturnsTrue()
		{
			var items = _testlet.Randomize().Skip(2);

			var pretests = items.Where(i => i.ItemType == ItemTypeEnum.Pretest);
			var operationals = items.Where(i => i.ItemType == ItemTypeEnum.Operational);

			Assert.Multiple(() =>
			{
				Assert.That(pretests.Count(), Is.EqualTo(2));
				Assert.That(operationals.Count(), Is.EqualTo(6));
			});
		}

		[Test]
		public void Randomize_ItemsHaveDublicates_ReturnsFalse()
		{
			var duplicates = _testlet.Randomize().GroupBy(i => i.ItemId).Where(g => g.Count() > 1);

			Assert.That(duplicates.Any(), Is.False);
		}

		// I would like to comment on why I didn't commit test like that earlier.
		// Testing random output is a tricky thing to do.
		// Usualy you test some function against expected output, wich is not the case with randomness.
		// Besides that, it's the best practice to avoid any logic in unit test, and I don't see how to do that in this particular case.
		// In contradiction with the above, that is the test I came up with ))
		[TestCase(50, 2)]
		public void Randomize_RandomSequencesCollisionsLessThanOrEqualToCollisionRate_ReturnsTrue(int iterations, int collisionRate)
		{
			var itemIdList = new List<List<string>>();
			for (int i = 0; i < iterations; i++)
			{
				itemIdList.Add(_testlet.Randomize().Select(p => p.ItemId).ToList());
			}

			int collisions = 0;
			for (int i = 0; i < iterations - 1; i++)
			{
				var j = i + 1;
				while (j < iterations)
				{
					if (itemIdList[i].SequenceEqual(itemIdList[j]))
					{
						collisions++;
					}
					j++;
				}
			}

			Assert.That(collisions, Is.LessThanOrEqualTo(collisionRate));
		}
	}
}