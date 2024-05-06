using Azure;
using TestD.Data;
using TestD.Models;
using TestD.Services.IServices;

namespace TestD.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly ILogger<MenuItemService> _logger;
        private readonly AppDbContext _db;

        public MenuItemService(ILogger<MenuItemService> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }
        public async Task CreateMenuItem(string menuItemName)
        {
            _logger.LogInformation($"Adding MenuItem {menuItemName}");
            var menuItem = new MenuItem { Name = menuItemName };
            _db.MenuItems.Add(menuItem);
            await Task.Delay(5000);
            await _db.SaveChangesAsync();
            _logger.LogInformation($"Added the Menu Item {menuItemName}");
        }
    }
}
