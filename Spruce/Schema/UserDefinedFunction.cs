namespace Spruce.Schema
{
	public abstract class UserDefinedFunction : ScriptedObject
	{
		protected string GetSqlReference(params object[] parameters)
		{
			return string.Format(Name + "(" + string.Join(",", parameters) + ")");
		}

		public override string DeleteScript
		{
			get
			{
				return string.Format(@"/****** Object:  UserDefinedFunction [dbo].{0}     ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'{0}') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION {0}", Name);
			}
		}
	}
}