using Microsoft.AspNetCore.Mvc;
using Datenlotsen.InventoryManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Datenlotsen.InventoryManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            SearchInventoryItemModel model,
            CancellationToken cancellationToken)
        {
           return Ok(await _inventoryService.SearchAsync(
               name: model?.Name,
               stockStatus: model?.StockStatus,
               categoryId: model.CategoryId,
               cancellationToken: cancellationToken));
        }

        [HttpGet("{id:guid}")]
        [ActionName(nameof(GetById))]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var item = await _inventoryService.GetByIdAsync(id, cancellationToken);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateInventoryItemModel item, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _inventoryService.CreateAsync(item.Name, item.StockQuantity, item.CategoryId,
                    cancellationToken);
                return Ok(result);
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { error = "please make sure the category exists and name not duplicated." });
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateInventoryItemModel item, CancellationToken cancellationToken)
        {
            var existingItem = await _inventoryService.GetByIdAsync(id, cancellationToken);
            if (existingItem == null) return NotFound();

            await _inventoryService.UpdateAsync(id, item.Name, item.StockQuantity, item.CategoryId, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var existingItem = await _inventoryService.GetByIdAsync(id, cancellationToken);
            if (existingItem == null) return NotFound();

            await _inventoryService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
