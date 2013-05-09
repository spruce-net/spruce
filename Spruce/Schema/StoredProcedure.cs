namespace Spruce.Schema
{
	public abstract class StoredProcedure : ScriptedObject
	{
		public override string DeleteScript
		{
			get
			{
				return string.Format(@"/****** Object:  StoredProcedure {0} ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{0}') AND type in (N'P', N'PC'))
DROP PROCEDURE {0}", Name);
			}
		}
	}
}