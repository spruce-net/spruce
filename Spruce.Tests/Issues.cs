using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using NUnit.Framework;
using Should.Fluent;
using Spruce.Schema;
using Spruce.Schema.Attributes;
using StackExchange.Profiling;

namespace Spruce.Tests
{
	public class Issues : TestBase
	{
		public override void SetupFixture()
		{
			base.SetupFixture();

			Db.CreateTable<ClassWithColumnName>();
			Db.CreateTable<ClassWithQueryExplicit>();
			Db.CreateTable<ClassWithGuidId>();
		}

		private class ClassWithColumnName
		{
			[AutoIncrement]
			public int Id { get; set; }
			[Column("OldName")]
			public string NewName { get; set; }
		}
		private class ClassWithGuidId
		{
			[PrimaryKey]
			[Default("newSequentialId()")]
			public Guid Id { get; set; }
			public string Name { get; set; }
		}

		[Test]
		public void Three_GivenColumnNameAttribute_UsesExplicitColumnQuery()
		{
			var item = new ClassWithColumnName
				{
					NewName = "Funk"
				};
			Db.Save(item); // Test save command
			item.NewName = "Dr";
			Db.Save(item); // Test update command

			var getItem = Db.SingleOrDefault<ClassWithColumnName>(new { item.Id }); // Test select single
			getItem.Should().Not.Be.Null();
			getItem.NewName.Should().Equal(item.NewName);

			var getItems = Db.Query<ClassWithColumnName>(new { NewName = "Dr" }); // Test select multiple
			getItems.Should().Not.Be.Empty();
			getItems.Should().Contain.One(x => x.Id == item.Id);

			var getPagedItems = Db.PagedList<ClassWithColumnName>(1, 50, "OldName=@Name", null, new { Name = item.NewName }); // Test paged select
			getPagedItems.Should().Not.Be.Empty();
			getPagedItems.Should().Contain.One(x => x.Id == item.Id);
		}

		[QueryExplicitColumns]
		private class ClassWithQueryExplicit
		{
			[AutoIncrement]
			public int Id { get; set; }
			public string NewName { get; set; }
		}

		[Test]
		public void Three_GivenQueryExplicitAttribute_UsesExplicitColumnQuery()
		{
			var item = new ClassWithQueryExplicit
				{
					NewName = "Funk"
				};
			Db.Save(item);

			var getItem = Db.SingleOrDefault<ClassWithQueryExplicit>(new { item.Id });
			getItem.Should().Not.Be.Null();
			getItem.NewName.Should().Equal(item.NewName);

			MiniProfiler.Current.HasSqlTimings.Should().Be.True();
			var timings = MiniProfiler.Current.GetSqlTimings();
			foreach (var timing in timings)
			{
				timing.CommandString.Should().Not.Contain(string.Format("select top 1 * from [{0}]", Db.GetTableName<ClassWithQueryExplicit>()));
			}
		}

		[Table("ClassWithColumnNames")]
		private class InheritedClassWithColumnName : ClassWithColumnName
		{
			public ClassWithQueryExplicit AnotherClass { get; set; }
		}

		[Test]
		public void GetColumnsUsingPassedObjectTypeInsteadOfUnderlyingObjectType()
		{
			var originalItem = new ClassWithColumnName
				{
					NewName = "Funk"
				};
			Db.Save(originalItem);

			var item = new InheritedClassWithColumnName
				{
					Id = originalItem.Id,
					NewName = "Dr",
					AnotherClass = new ClassWithQueryExplicit
					{
						NewName = "asdf"
					}
				};
			Db.Save((ClassWithColumnName)item);

			var updatedItem = Db.SingleOrDefault<ClassWithColumnName>(new { originalItem.Id });
			updatedItem.Should().Not.Be.Null();
			updatedItem.NewName.Should().Equal(item.NewName);
		}

		[Test]
		public void PagedListWithEmptyWhereDoesNotApplyWhereClause()
		{
			var testItem = new ClassWithColumnName {NewName = "PagedListWithEmptyWhere"};
			Db.Save(testItem);

			var result = Db.PagedList<ClassWithColumnName>(1, 1, "");
			result.Should().Not.Be.Null();
			result.Should().Not.Be.Empty();
		}
		[Test]
		public void PagedListWithNullWhereDoesNotApplyWhereClause()
		{
			var testItem = new ClassWithColumnName {NewName = "PagedListWithNullWhere"};
			Db.Save(testItem);

			var result = Db.PagedList<ClassWithColumnName>(1, 1, null);
			result.Should().Not.Be.Null();
			result.Should().Not.Be.Empty();
		}

		[Test]
		public void BulkInsertWithNewSequentialIdSetsIdToNewGuid()
		{
			var item1 = new ClassWithGuidId {Name = "ClassWithGuidId1"};
			var item2 = new ClassWithGuidId {Name = "ClassWithGuidId2"};

			var result = Db.BulkInsert(new[] {item1, item2});
			result.Should().Equal(2);

			var dbItem1 = Db.Query<ClassWithGuidId>(new {item1.Name});
			dbItem1.Should().Not.Be.Empty();
			dbItem1.FirstOrDefault().Id.Should().Not.Equal(Guid.Empty);
			var dbItem2 = Db.Query<ClassWithGuidId>(new {item2.Name});
			dbItem2.Should().Not.Be.Empty();
			dbItem2.FirstOrDefault().Id.Should().Not.Equal(Guid.Empty);
		}
	}
}
