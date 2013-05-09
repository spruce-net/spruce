namespace Spruce.Schema
{
	public abstract class View : ScriptedObject
	{
		public override string DeleteScript
		{
			get
			{
				return
					string.Format(@"/****** Object:  View {0} ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'{0}'))
DROP VIEW {0}", Name);
			}
		}
	}
}