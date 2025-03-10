namespace CafeManager.Core.Models.MenuItems;

public record MenuItemExistsFilter(List<long>? Ids = null, List<string>? Names = null);