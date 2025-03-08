using System.ComponentModel.DataAnnotations;

using CafeManager.Core.Common.Interfaces;

namespace CafeManager.Core.Models.MenuItems;

public class MenuItem : IEntity<long>
{
    public long Id { get; private set; }

    [MinLength(1)]
    public required string Name { get; set; }
}