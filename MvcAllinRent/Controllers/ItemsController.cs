using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcAllinRent.Repositories;

namespace MvcAllinRent.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ItemRepository _itemRepository;

        public ItemsController(ItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        // GET: ItemsController
        public async Task<IActionResult> Index(int? pageNumber = 1, string? q = null)
        {
            string? searchTerm = string.IsNullOrWhiteSpace(q) ? null : q;

            var paginatedItems = await _itemRepository.GetAll(
                pageNumber: pageNumber.GetValueOrDefault(1),
                pageSize: 10,
                searchCriteria: searchTerm
            );

            return View(paginatedItems);
        }

        // GET: ItemsController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            return View(await _itemRepository.GetById(id));
        }

        // GET: ItemsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ItemsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ItemsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ItemsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ItemsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ItemsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
