using System.ComponentModel.DataAnnotations;

namespace CafeManager.Contracts.Dto.MenuItems;

public record UpdateMenuItemDto(
    [Required] string Name
);