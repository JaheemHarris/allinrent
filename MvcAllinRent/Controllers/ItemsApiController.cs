using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcAllinRent.Models;
using MvcAllinRent.Repositories;

namespace MvcAllinRent.Controllers
{
    [Route("api/items")]
    [ApiController]
    public class ItemsApiController : ControllerBase
    {
        private readonly ItemRepository _itemRepository;

        public ItemsApiController(ItemRepository itemRepository) 
        {
            _itemRepository = itemRepository;
        }

        /// <summary>
        /// Get paginated items
        /// </summary>
        /// <returns>PaginatedResult<Item></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResult<Item>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPaginatedItems(int? pageNumber = 1, int? pageSize = 6, string? q = null)
        {
            string? searchTerm = string.IsNullOrWhiteSpace(q) ? null : q;

            var paginatedItems = await _itemRepository.GetAll(
                pageNumber: pageNumber.GetValueOrDefault(1),
                pageSize: pageSize.GetValueOrDefault(6),
                searchCriteria: searchTerm
            );

            return Ok( paginatedItems );
        }
    }
}
