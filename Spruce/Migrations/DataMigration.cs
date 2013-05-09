using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Spruce.Schema.Attributes;

namespace Spruce.Migrations
{
	[Table("_Migrations")]
	public class DataMigration
	{
		[AutoIncrement]
		public int Id { get; set; }

		[StringLength(100)]
		public string Name { get; set; }

		public DateTime? CreatedOn { get; set; }
	}
}