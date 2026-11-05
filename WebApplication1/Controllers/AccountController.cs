using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;


namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]// 1. Hiển thị trang đăng nhập (GET)
        public IActionResult login()
        {
            return View();
        }
        [HttpPost]//2. Xử lý khi người dùng ấn nút Đăng nhập(POST)
        public IActionResult login (LoginViewModel model)
        {// Xử lý kiểm tra tài khoản ở đây...
         // Handle login logic here
            if (!ModelState.IsValid)
            {
                return View(model); // Trả lại trang đăng nhập cùng các dòng chữ báo lỗi
            }
            // Xử lý kiểm tra tài khoản...
            return RedirectToAction("Index", "Home");// Chuyển hướng đến trang Home/Index nếu đăng nhập thành công


        }
        // 3. Hiển thị trang Đăng ký (GET)
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["SuccessMessage"] = "Đăng ký tài khoản thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Index");
            }
            // Xử lý đăng ký người dùng ở đây
            return View(model);
        }
    }
}
