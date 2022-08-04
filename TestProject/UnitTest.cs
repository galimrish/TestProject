using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TestProject
{
	public class UnitTest
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
		public void Randomize_ItemsAmountNot10_ThrowsError(int length)
		{
			var list = new List<Item>(length);
			list.AddRange(Enumerable.Repeat(new Item(), length));
			var testlet = new Testlet("1", list);
			
			var exception = Assert.Throws<Exception>(() => testlet.Randomize());
			Assert.AreEqual("Testlet items amout should be 10", exception?.Message);
		}

		[Test]
		public void Randomize_SevenOperationalItems_ThrowsError()
		{
			var list = new List<Item>(10);
			list.AddRange(Enumerable.Repeat(new Item { ItemType = ItemTypeEnum.Operational }, 7));
			list.AddRange(Enumerable.Repeat(new Item { ItemType = ItemTypeEnum.Pretest }, 3));
			var testlet = new Testlet("1", list);

			var exception = Assert.Throws<Exception>(() => testlet.Randomize());
			Assert.AreEqual("Incorrect Testlet set of items", exception?.Message);
		}

		[Test]
		public void Randomize_FirstTwoItemsArePretest_ReturnsTrue()
		{
			var items = _testlet.Randomize();
			Assert.AreEqual(ItemTypeEnum.Pretest, items[0].ItemType);
			Assert.AreEqual(ItemTypeEnum.Pretest, items[1].ItemType);
		}

		[Test]
		public void Randomize_LastEightItemsContainTwoPretest_ReturnsTrue()
		{
			var items = _testlet.Randomize().Skip(2).Where(i => i.ItemType == ItemTypeEnum.Pretest);

			Assert.IsTrue(items.Count() == 2);
		}

		[Test]
		public void Randomize_ItemsHaveDublicates_ReturnsFalse()
		{
			var duplicates = _testlet.Randomize().GroupBy(i => i.ItemId).Where(g => g.Count() > 1);

			Assert.IsFalse(duplicates.Any());
		}
	}
}
