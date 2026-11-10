using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        // Tiêm DbContext và PasswordHasher qua Constructor
        public AccountController(ApplicationDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        // =========================================================================
        // 1. CHỨC NĂNG ĐĂNG NHẬP (LOGIN) - THEO ĐÚNG QUY TRÌNH 8 BƯỚC
        // =========================================================================

        [HttpGet]
        public IActionResult Login()
        {
            // Nếu người dùng đã có Cookie đăng nhập hợp lệ -> chuyển thẳng vào trang chủ
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // BƯỚC 2: Validate dữ liệu đầu vào (Kiểm tra rỗng, độ dài qua DataAnnotations)
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // BƯỚC 4: Server tìm tài khoản trong SQL Server thông qua EF Core
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.ToLower() == model.Username.Trim().ToLower());
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không chính xác!");
                return View(model);
            }   

            // BƯỚC 5: Kiểm tra thông tin xác thực
            // 5.1. Kiểm tra trạng thái tài khoản
            if (!user.IsActive)
            {
                ModelState.AddModelError(string.Empty, "Tài khoản của bạn đã bị khóa. Vui lòng liên hệ Admin!");
                return View(model);
            }

            // 5.2. So khớp Password Hash với mật khẩu người dùng vừa nhập
            var verifyResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            if (verifyResult == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không chính xác!");
                return View(model);
            }

            // BƯỚC 6 & 7: Xác định Role và Tạo phiên đăng nhập (Authentication Cookie chứa Claims)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("FullName", user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role) // Gắn Role: GiangVien, TruongBoMon, PhongKhaoThi, Admin...
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe, // Tích ghi nhớ đăng nhập
                ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(14) : DateTimeOffset.UtcNow.AddHours(8)
            };

            // Cấp vé Cookie cho trình duyệt
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // BƯỚC 8: Chuyển vào ứng dụng theo phân quyền (Role-based Navigation)
            if (user.Role == "Admin")
            {
                return RedirectToAction("Index", "Home"); // Hoặc trang Admin Dashboard sau này
            }
            else if (user.Role == "TruongBoMon")
            {
                return RedirectToAction("Index", "Home"); // Hoặc trang Duyệt câu hỏi sau này
            }

            // Mặc định cho Giảng viên
            return RedirectToAction("Index", "Home");
        }

        // =========================================================================
        // 2. CHỨC NĂNG ĐĂNG KÝ (REGISTER)
        // =========================================================================

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Kiểm tra dữ liệu đầu vào
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 1. Kiểm tra trùng Username
            var isUsernameTaken = await _context.Users.AnyAsync(u => u.Username.ToLower() == model.Username.Trim().ToLower());
            if (isUsernameTaken)
            {
                ModelState.AddModelError("Username", "Tên đăng nhập này đã có người sử dụng!");
                return View(model);
            }

            // 2. Kiểm tra trùng Email
            var isEmailTaken = await _context.Users.AnyAsync(u => u.Email.ToLower() == model.Email.Trim().ToLower());
            if (isEmailTaken)
            {
                ModelState.AddModelError("Email", "Email này đã được đăng ký trên hệ thống!");
                return View(model);
            }

            // 3. Khởi tạo User mới với vai trò mặc định Giảng viên
            var newUser = new User
            {
                FullName = model.FullName.Trim(),
                Username = model.Username.Trim(),
                Email = model.Email.Trim(),
                Role = "GiangVien", // Quyền mặc định
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            // 4. Băm mật khẩu (Hash Password)
            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, model.Password);

            // 5. Lưu vào SQL Server
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // Chuyển hướng sang trang Đăng nhập kèm thông báo thành công
            TempData["SuccessMessage"] = "Đăng ký tài khoản thành công! Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }

        // =========================================================================
        // 3. CHỨC NĂNG ĐĂNG XUẤT (LOGOUT)
        // =========================================================================

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Xóa sạch Cookie trên trình duyệt
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // Trang thông báo khi cố tình truy cập vào trang không đủ quyền
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
