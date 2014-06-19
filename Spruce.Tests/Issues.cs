using System;
using System.ComponentModel.DataAnnotations.Schema;
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
		}

		private class ClassWithColumnName
		{
			[AutoIncrement]
			public int Id { get; set; }
			[Column("OldName")]
			public string NewName { get; set; }
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
	}
}
