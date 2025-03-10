using System.ComponentModel.DataAnnotations;

using CafeManager.Core.Common.Interfaces;

namespace CafeManager.Core.Models.MenuItems;

public class MenuItem : IEntity<long>
{
    public required long Id { get; init; }

    [MinLength(1)]
    public required string Name { get; set; }
}