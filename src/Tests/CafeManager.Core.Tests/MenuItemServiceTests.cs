using System.Threading.Tasks;

using CafeManager.Contracts.Dto.MenuItems;
using CafeManager.Core.Exceptions;
using CafeManager.Core.Mapping.Configurations;
using CafeManager.Core.Models.MenuItems;
using CafeManager.Core.Repositories;
using CafeManager.Core.Services;
using CafeManager.Core.Services.Interfaces;

using MapsterMapper;

using Moq;

namespace CafeManager.Core.Tests;

public class MenuItemServiceTests
{
    private Mock<IMenuItemRepository> _menuItemRepositoryMock;
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        _menuItemRepositoryMock = new Mock<IMenuItemRepository>();

        MenuItemConfiguration.Configure();
        _mapper = new Mapper();
    }

    private IMenuItemService GetMenuItemService()
    {
        return new MenuItemService(_menuItemRepositoryMock.Object, _mapper);
    }

    [Test]
    public async Task GetMenuItem_WithValidData_ShouldReturnMenuItem()
    {
        var menuItemId = 1;
        var menuItems = new Dictionary<long, MenuItem> { { menuItemId, new MenuItem { Id = menuItemId, Name = "Test" } } };
        var menuItemDto = new MenuItemDto(menuItemId, "Test");

        _menuItemRepositoryMock.Setup(x => x.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync<long, CancellationToken, IMenuItemRepository, MenuItem>((id, _) => menuItems[id]);

        var menuItemService = GetMenuItemService();

        var menuItem = await menuItemService.GetAsync(menuItemId, CancellationToken.None);

        Assert.That(menuItem, Is.EqualTo(menuItemDto));
    }

    [Test]
    public void GetMenuItem_NotFound_ShouldThrowException()
    {
        _menuItemRepositoryMock.Setup(x => x.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new EntityNotFoundException());

        var menuItemService = GetMenuItemService();

        Assert.ThrowsAsync<EntityNotFoundException>(async () => await menuItemService.GetAsync(1, CancellationToken.None));
    }

    [Test]
    public async Task CreateMenuItem_WithValidData_ShouldCreateAndReturnMenuItem()
    {
        var addMenuItemDto1 = new AddMenuItemDto("Test1");
        var addMenuItemDto2 = new AddMenuItemDto("Test2");
        var counter = 0;

        _menuItemRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<MenuItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync<MenuItem, CancellationToken, IMenuItemRepository, MenuItem>((menuItem, _) => new MenuItem { Id = ++counter, Name = menuItem.Name });

        var menuItemService = GetMenuItemService();

        var createdMenuItem1 = await menuItemService.CreateAsync(addMenuItemDto1, CancellationToken.None);
        var createdMenuItem2 = await menuItemService.CreateAsync(addMenuItemDto2, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(createdMenuItem1.Id, Is.EqualTo(1));
            Assert.That(createdMenuItem2.Id, Is.EqualTo(2));

            Assert.That(createdMenuItem1.Name, Is.EqualTo(addMenuItemDto1.Name));
            Assert.That(createdMenuItem2.Name, Is.EqualTo(addMenuItemDto2.Name));
        });
    }

    [Test]
    public void CreateMenuItem_WithConflict_ShouldThrowException()
    {
        var menuItemDto = new AddMenuItemDto("Test1");
        var menuItems = new Dictionary<long, MenuItem>() { { 1, new MenuItem { Id = 1, Name = "Test1" } } };

        _menuItemRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<MenuItem>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new EntityConflictException());

        var menuItemService = GetMenuItemService();

        Assert.ThrowsAsync<EntityConflictException>(async () => await menuItemService.CreateAsync(menuItemDto, CancellationToken.None));
    }

    [Test]
    public async Task UpdateMenuItem_WithValidData_ShouldUpdateAndReturnMenuItem()
    {
        var updateMenuItemDto = new UpdateMenuItemDto("Test2");
        var menuItems = new Dictionary<long, MenuItem> { { 1, new MenuItem { Id = 1, Name = "Test" } } };

        _menuItemRepositoryMock.Setup(x => x.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync<long, CancellationToken, IMenuItemRepository, MenuItem>((id, _) => menuItems[id]);

        _menuItemRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<MenuItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync<MenuItem, CancellationToken, IMenuItemRepository, MenuItem>((menuItem, _) =>
            {
                menuItems[menuItem.Id] = menuItem;
                return menuItem;
            });

        var menuItemService = GetMenuItemService();

        var updatedMenuItem = await menuItemService.UpdateAsync(1, updateMenuItemDto, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(updatedMenuItem.Id, Is.EqualTo(1));
            Assert.That(updatedMenuItem.Name, Is.EqualTo(updateMenuItemDto.Name));

            Assert.That(menuItems[1].Id, Is.EqualTo(1));
            Assert.That(menuItems[1].Name, Is.EqualTo(updateMenuItemDto.Name));

            Assert.That(menuItems, Has.Count.EqualTo(1));
        });
    }

    [Test]
    public void UpdateMenuItem_WithConflict_ShouldThrowException()
    {
        var updateMenuItemDto = new UpdateMenuItemDto("Test");

        _menuItemRepositoryMock.Setup(x => x.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync<long, CancellationToken, IMenuItemRepository, MenuItem>((id, _) => new MenuItem { Id = id, Name = "Test" });

        _menuItemRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<MenuItem>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new EntityConflictException());

        var menuItemService = GetMenuItemService();

        Assert.ThrowsAsync<EntityConflictException>(async () => await menuItemService.UpdateAsync(2, updateMenuItemDto, CancellationToken.None));
    }

    [Test]
    public void UpdateMenuItem_WithNotFound_ShouldThrowException()
    {
        var updateMenuItemDto = new UpdateMenuItemDto("Test");

        _menuItemRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<MenuItem>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new EntityNotFoundException());

        var menuItemService = GetMenuItemService();

        Assert.ThrowsAsync<EntityNotFoundException>(async () => await menuItemService.UpdateAsync(1, updateMenuItemDto, CancellationToken.None));
    }

    [Test]
    public void DeleteMenuItem_WithValidData_ShouldDeleteMenuItem()
    {
        var menuItems = new Dictionary<long, MenuItem> { { 1, new MenuItem { Id = 1, Name = "Test" } } };
        _menuItemRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .Returns<long, CancellationToken>((id, _) =>
            {
                menuItems.Remove(id);
                return Task.CompletedTask;
            });

        var menuItemService = GetMenuItemService();

        Assert.DoesNotThrowAsync(async () => await menuItemService.DeleteAsync(1, CancellationToken.None));
        Assert.That(menuItems, Has.Count.EqualTo(0));
    }
}