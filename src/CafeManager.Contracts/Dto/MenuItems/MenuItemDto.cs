using System.ComponentModel.DataAnnotations;

namespace CafeManager.Contracts.Dto.MenuItems;

public record MenuItemDto(
    [Required] long Id,
    [Required] string Name
);