using System.ComponentModel.DataAnnotations;

using CafeManager.Core.Common.Interfaces;

namespace CafeManager.Core.Models.MenuItems;

public class MenuItem(
    string name) : IEntity<long>
{
    [Required]
    public long Id { get; }

    [Required, MinLength(1)]
    public string Name { get; set; } = name;
}