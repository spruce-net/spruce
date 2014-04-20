using System.Data;
using NUnit.Framework;
using Spruce.Schema;
using Spruce.Tests.Infrastructure;
using StackExchange.Profiling;
using StructureMap;

namespace Spruce.Tests
{
	[SetUpFixture]
	public class TestFixtureSetup
	{
		[SetUp]
		public void Setup()
		{
			MiniProfiler.Settings.ProfilerProvider = new SingletonProfilerProvider();
			MiniProfiler.Start();
			ObjectFactory.Initialize(x => x.AddRegistry(new IocRegistry()));

			// Start with a fresh, empty db
			var db = ObjectFactory.GetInstance<IDbConnection>();
			db.DropAllScriptedObjects();
			db.DropAllTables();
		}
	}
}
