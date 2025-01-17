using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcAllinRent.Models;
using MvcAllinRent.Repositories;
using System.Security.Claims;

namespace MvcAllinRent.Controllers
{
    [Authorize]
    public class RentalsController : Controller
    {
        private readonly RentalRepository _rentalRepository;
        private readonly AuthUserRepository _userRepository;
        private readonly ItemRepository _itemRepository;

        public RentalsController(RentalRepository rentalRepository, AuthUserRepository userRepository, ItemRepository itemRepository)
        {
            _rentalRepository = rentalRepository;
            _userRepository = userRepository;
            _itemRepository = itemRepository;
        }

        // GET: RentalsController
        public async Task<IActionResult> Index(int? pageNumber = 1, string? q = null)
        {
            string? searchTerm = string.IsNullOrWhiteSpace(q) ? null : q;

            var paginatedRentals = await _rentalRepository.GetUserRentals(
                userId: 1,
                pageNumber: pageNumber.GetValueOrDefault(1),
                pageSize: 10,
                searchItemName: searchTerm
            );

            return View(paginatedRentals);
        }

        // GET: RentalsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // POST: RentalsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rent(RentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.GetUserByEmail(User.FindFirst(ClaimTypes.Email)?.Value);

                if(user == null)
                {
                    return RedirectToAction("Login", "Auth");
                }

                var item = await _itemRepository.GetById(model.ItemId);

                if (item == null)
                {
                    return NotFound();
                }

                var durationDays = (model.DueDate.ToDateTime(TimeOnly.MinValue) - model.LocationDate.ToDateTime(TimeOnly.MinValue)).Days + 1;
                var due = durationDays * model.Quantity * item.RentalFee;

                var rent = new Rental
                {
                    Id = 0,
                    UserId = user.Id,
                    ItemId = model.ItemId,
                    Quantity = model.Quantity,
                    UnitPrice = item.RentalFee,
                    StartDate = model.LocationDate.ToDateTime(TimeOnly.MinValue),
                    DurationDays = durationDays,
                    Due = due,
                };

                bool result = await _rentalRepository.Save(rent);

                if(result)
                {
                    return RedirectToAction("Index", "Rentals");
                } else
                {
                    return RedirectToAction("Login", "Auth");
                }
            }

            return View(model);
        }

        // GET: RentalsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RentalsController/Edit/5
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

        // GET: RentalsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RentalsController/Delete/5
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
