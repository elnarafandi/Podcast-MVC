using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace Podcast.Controllers
{
    [Authorize(Roles = "Admin,Member")]
    public class PaymentController : Controller
    {
        private readonly IPackageService _packageService;
        private readonly UserManager<AppUser> _userManager;
        public PaymentController(IPackageService packageService,
                                 UserManager<AppUser> userManager)
        {
            _packageService = packageService;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CheckOut()
        {
            var package = await _packageService.GetByIdAsync(3);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var domain = "http://localhost:5134/";
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + "payment/success?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = domain + "payment/cancel",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                ClientReferenceId = userId
            };

            var sessionListItem = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (int)(package.Price * 100), // qəpik cinsindən
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Premium Package",
                    },
                },
                Quantity = 1
            };

            options.LineItems.Add(sessionListItem);

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            Response.Headers.Add("Location", session.Url);

            return new StatusCodeResult(303);
        }

        public async Task<IActionResult> Success(string session_id)
        {
            if (string.IsNullOrEmpty(session_id))
            {
                return RedirectToAction("Index", "Home");
            }

            var service = new SessionService();
            var session = await service.GetAsync(session_id);

            if (session.PaymentStatus == "paid")
            {
                var userId = session.ClientReferenceId;

                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    user.PackageId = 3;
                    user.PurchasedAt = DateTime.UtcNow.AddHours(4);
                    await _userManager.UpdateAsync(user);
                }

                return RedirectToAction("Index", "Home");  // Müvəffəqiyyət səhifəsi
            }
            else
            {
                return RedirectToAction("Index", "Home");  // Ödəniş uğursuz olduqda
            }
        }


    }
}
