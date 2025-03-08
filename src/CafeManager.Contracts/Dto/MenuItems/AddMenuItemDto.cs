using System.ComponentModel.DataAnnotations;

namespace CafeManager.Contracts.Dto.MenuItems;

public record AddMenuItemDto(
    [Required] string Name
);