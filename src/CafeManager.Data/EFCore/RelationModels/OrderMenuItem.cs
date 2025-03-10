namespace CafeManager.Data.EFCore.RelationModels;

public class OrderMenuItem
{
    public required long OrderId { get; set; }
    public required long MenuItemId { get; set; }
}