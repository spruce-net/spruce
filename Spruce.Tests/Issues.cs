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
			Db.Save(item);

			var getItem = Db.SingleOrDefault<ClassWithColumnName>(new { item.Id });
			getItem.Should().Not.Be.Null();
			getItem.NewName.Should().Equal(item.NewName);
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
	}
}
