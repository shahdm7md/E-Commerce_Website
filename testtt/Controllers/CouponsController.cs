using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testtt.Data;
using testtt.Models;

namespace testtt.Controllers
{
    //[Authorize(Roles = "Admin")]
    //[Area("Admin")]
    public class CouponsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CouponsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Coupons
        public async Task<IActionResult> Index()
        {
            return View(await _context.Coupons.ToListAsync());
        }

        // GET: Coupons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Coupons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,DiscountAmount,IsPercentage,ExpiryDate,IsActive")] Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                // تحقق من أن التاريخ المدخل أكبر من التاريخ الحالي
                if (coupon.ExpiryDate <= DateTime.Now)
                {
                    ModelState.AddModelError("ExpiryDate", "Expiry date must be in the future.");
                    return View(coupon);
                }

                // تحقق من وجود الكود مسبقًا
                var existingCoupon = await _context.Coupons.FirstOrDefaultAsync(c => c.Code == coupon.Code);
                if (existingCoupon != null)
                {
                    ModelState.AddModelError("Code", "Code already exists. Please use a different code.");
                    return View(coupon);
                }

                _context.Add(coupon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coupon);
        }

        // GET: Coupons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coupon = await _context.Coupons.FindAsync(id);
            if (coupon == null)
            {
                return NotFound();
            }
            return View(coupon);
        }

        // POST: Coupons/Edit/5
        // POST: Coupons/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,DiscountAmount,IsPercentage,ExpiryDate,IsActive")] Coupon coupon)
        {
            if (id != coupon.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // تحقق من أن التاريخ المدخل أكبر من التاريخ الحالي
                if (coupon.ExpiryDate <= DateTime.Now)
                {
                    ModelState.AddModelError("ExpiryDate", "Expiry date must be in the future.");
                    return View(coupon);
                }

                // تحقق من وجود الكود مسبقًا باستثناء الكود المحدد
                var existingCoupon = await _context.Coupons.FirstOrDefaultAsync(c => c.Code == coupon.Code && c.Id != coupon.Id);
                if (existingCoupon != null)
                {
                    ModelState.AddModelError("Code", "Code already exists. Please use a different code.");
                    return View(coupon);
                }

                try
                {
                    _context.Update(coupon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CouponExists(coupon.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(coupon);
        }


        private bool CouponExists(int id)
        {
            return _context.Coupons.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var coupon = await _context.Coupons.FindAsync(id);
            _context.Coupons.Remove(coupon);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
