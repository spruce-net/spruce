using System;
using System.Data;
using NUnit.Framework;
using StructureMap;

namespace Spruce.Tests
{
	public abstract class TestBase
	{
		protected IDbConnection Db { get; set; }

		[TestFixtureSetUp]
		public virtual void SetupFixture()
		{
			Db = ObjectFactory.GetInstance<IDbConnection>();
		}
		[TestFixtureTearDown]
		public virtual void TearDownFixture()
		{
		}

		[SetUp]
		public virtual void Setup()
		{
		}

		[TearDown]
		public virtual void TearDown()
		{
		}
	}
}