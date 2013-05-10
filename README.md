Spruce
===========

Opinionated set of extension methods representing a lightweight orm and migration framework for use with Dapper.  This is a .net 4.5 designed to work with MS SQL Server 2008+.

Conventions
-----------

* Ids are generated from SQL. This project supports int, long, and guid ids. All objects are expected to have an Id field - composite ids are not supported.
* This project includes a basic data migration framework. Migrations are one way (no rolling back) and are handled all in code. Running the migrations is up to the user.

Example usage
-----------------
* Real world, example usage can be found in the [MvcKickstart project](https://github.com/jgeurts/MvcKickstart).


Advanced configuration options
------------------------------

### How do I set audit fields before an item gets saved or clear cache after an item is saved?
> Spruce has a couple events that you can hook. In particular, you'll be interested in the Saving and Saved events. Saving is fired before the item is inserted/updated, while Saved is fired after the db command has completed.
```csharp
// Somewhere in your application configuration
SpruceSettings.Saving += (item, e) =>
	{
		var entityType = item.GetType();
		int? userId = null;
		UserPrincipal currentUser;
		if (HttpContext.Current != null)
		{
			currentUser = HttpContext.Current.User as UserPrincipal;
		}
		else
		{
			currentUser = Thread.CurrentPrincipal as UserPrincipal;
		}
		if (currentUser != null && currentUser.UserObject != null && currentUser.UserObject.Id != default(int))
			userId = currentUser.UserObject.Id;
		if (userId.HasValue)
		{
			var createdBy = entityType.GetProperty("CreatedBy");
			if (createdBy != null && (int) createdBy.GetValue(item) == default(int))
				createdBy.SetValue(item, userId.Value);
			var modifiedBy = entityType.GetProperty("ModifiedBy");
			if (modifiedBy != null)
				modifiedBy.SetValue(item, userId.Value);
		}
		var createdOn = entityType.GetProperty("CreatedOn");
		if (createdOn != null && (createdOn.GetValue(item) == null || (DateTime) createdOn.GetValue(item) == default(DateTime)))
			createdOn.SetValue(item, DateTime.UtcNow);
		var modifiedOn = entityType.GetProperty("ModifiedOn");
		if (modifiedOn != null)
			modifiedOn.SetValue(item, DateTime.UtcNow);
	};
// Clear cache for an object after it is saved
SpruceSettings.Saved += (item, e) =>
	{
		Cache.ClearFor(item);
	};
```


### How do I change a sql data type used when creating tables/columns?

> SQL types are defined in [SpruceSettings.cs](https://github.com/jgeurts/spruce/blob/1a51840b7a2e35cf45107e04f68908a9eb76af65/Spruce/SpruceSettings.cs#L27-L39). Look near the bottom for the SqlSchemaTypeMap field.  You may add or update the map as needed:
```cscharp
// Somewhere in your application configuration
SpruceSettings.SqlSchemaTypeMap[typeof(TimeSpan)] = "long";
```
> Please keep in mind that this only affects schema information. You may need to adjust Dapper's type mapping for querying.


### I would like my enum to map to an nvarchar field instead of an int. How do I do this?

> You can have any field map to nvarchar by decorating the property with the [StringLength(xx)] attribute
> For example:
```csharp
public class TestTable {
  [AutoIncrement]
  public int Id { get; set; }
  [StringLength(50)]
  public MyEnumType MyEnumProperty { get; set; }
}
```


### How do I specify a primary key?
```csharp
// Auto incrementing primary key Id. The [PrimaryKey] attribute is inferred when using [AutoIncrement]
public class TestTable {
  [AutoIncrement]
  public int Id { get; set; }
}
```
```csharp
// Guid primary key
public class TestTable {
  [PrimaryKey]
  [Default("newSequentialId()")]
  public Guid Id { get; set; }
}
```
### How do I specify a foreign key relationship?
```csharp
public class TestTable {
  [AutoIncrement]
  public int Id { get; set; }
}
public class OtherTestTable {
  [AutoIncrement]
  public int Id { get; set; }
  [References(typeof(TestTable))]
  public int TestTableId { get; set; }
}
```
### How do I control the name of the table?
```csharp
// By default, Spruce will use plural table names, based on the name of the class. 
// You can override this by using the table attribute from the System.ComponentModel.DataAnnotations.Schema namespace
[Table("TestTable")]
public class TestTable {
  [AutoIncrement]
  public int Id { get; set; }
}
```
