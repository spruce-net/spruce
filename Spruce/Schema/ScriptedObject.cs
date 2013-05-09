namespace Spruce.Schema
{
	public abstract class ScriptedObject
	{
		public abstract string Name { get; }
		public abstract string CreateScript { get; }
		public abstract string DeleteScript { get; }
	}
}