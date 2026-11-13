/**
 * EXAMBANK - DASHBOARD ANIMATION & INTERACTION SYSTEM
 * 
 * 1. Hover Effects (handled primarily in site.css + subtle JS touches)
 * 2. Fade In / Slide Up (Keyframes & Tab switching triggers)
 * 3. Count Up (Smooth number increment animation using requestAnimationFrame)
 * 4. Skeleton Loading (Shimmer placeholders & simulated loaders)
 * 5. Sidebar Transition (Desktop collapse/expand & Mobile off-canvas drawer)
 */

document.addEventListener('DOMContentLoaded', function () {
    initSidebarTransition();
    initCountUpAnimation();
    initTabAnimations();
    initBootstrapTooltips();
});

/* ==========================================================================
   1. COUNT UP ANIMATION ENGINE (HIỆU ỨNG TĂNG DẦN SỐ LIỆU THỐNG KÊ)
   ========================================================================== */
function initCountUpAnimation(container = document) {
    const countElements = container.querySelectorAll('.count-up, [data-countup]');
    if (!countElements.length) return;

    // Helper: phân tích chuỗi số liệu (hỗ trợ số nguyên, số thực %, dấu phẩy/chấm)
    function parseValue(text) {
        text = text.trim();
        let prefix = '';
        let suffix = '';

        // Tách tiền tố nếu có ($ hoặc ký hiệu)
        const prefixMatch = text.match(/^[^\d\.\,\-]+/);
        if (prefixMatch) {
            prefix = prefixMatch[0];
            text = text.slice(prefix.length);
        }

        // Tách hậu tố nếu có (%, câu, đề, ...)
        const suffixMatch = text.match(/[^\d\.\,]+$/);
        if (suffixMatch) {
            suffix = suffixMatch[0];
            text = text.slice(0, -suffix.length);
        }

        // Kiểm tra định dạng có dấu phẩy hay chấm làm phân cách hàng nghìn hay thập phân
        let isDecimal = false;
        let decimals = 0;
        let cleanText = text;

        if (text.includes('.') && text.includes(',')) {
            // Định dạng kiểu 1.250,50 hoặc 1,250.50
            if (text.indexOf(',') > text.indexOf('.')) {
                // Châu Âu / VN: 1.250,50
                cleanText = text.replace(/\./g, '').replace(',', '.');
            } else {
                // Mỹ: 1,250.50
                cleanText = text.replace(/,/g, '');
            }
            isDecimal = true;
            decimals = cleanText.split('.')[1]?.length || 0;
        } else if (text.includes('.')) {
            const parts = text.split('.');
            if (parts[1].length === 3 && parts[0].length <= 3) {
                // Phân cách hàng nghìn kiểu VN: 12.850 -> 12850
                cleanText = text.replace(/\./g, '');
            } else {
                // Số thập phân: 94.5 -> 94.5
                isDecimal = true;
                decimals = parts[1].length;
            }
        } else if (text.includes(',')) {
            const parts = text.split(',');
            if (parts[1].length === 3) {
                // Phân cách hàng nghìn: 1,280 -> 1280
                cleanText = text.replace(/,/g, '');
            } else {
                // Số thập phân kiểu VN: 94,5 -> 94.5
                cleanText = text.replace(',', '.');
                isDecimal = true;
                decimals = parts[1].length;
            }
        }

        const targetNumber = parseFloat(cleanText) || 0;
        const originalFormat = (val) => {
            if (isDecimal) {
                return prefix + val.toFixed(decimals) + suffix;
            } else if (targetNumber >= 1000 && (text.includes(',') || text.includes('.'))) {
                const separator = text.includes('.') ? '.' : ',';
                return prefix + Math.round(val).toString().replace(/\B(?=(\d{3})+(?!\d))/g, separator) + suffix;
            } else {
                return prefix + Math.round(val) + suffix;
            }
        };

        return {
            target: targetNumber,
            format: originalFormat,
            rawText: prefix + text + suffix
        };
    }

    // Hiệu ứng đếm tăng dần
    function animateCounter(element) {
        if (element.dataset.counted === 'true') return;

        const targetData = element.dataset.countup || element.textContent;
        const parsed = parseValue(targetData);
        if (isNaN(parsed.target)) return;

        element.dataset.counted = 'true';

        // Kiểm tra hỗ trợ prefers-reduced-motion (người dùng bật chế độ giảm chuyển động)
        const prefersReducedMotion = window.matchMedia && window.matchMedia('(prefers-reduced-motion: reduce)').matches;
        if (prefersReducedMotion) {
            element.textContent = targetData.trim();
            element.classList.add('counted');
            return;
        }

        const duration = 1400; // 1.4 giây mượt mà
        const startTime = performance.now();

        // Easing: Cubic Ease-Out
        const easeOutCubic = (t) => 1 - Math.pow(1 - t, 3);

        function step(now) {
            const elapsed = now - startTime;
            const progress = Math.min(elapsed / duration, 1);
            const easedProgress = easeOutCubic(progress);
            const currentVal = easedProgress * parsed.target;

            element.textContent = parsed.format(currentVal);

            if (progress < 1) {
                requestAnimationFrame(step);
            } else {
                element.textContent = targetData.trim();
                element.classList.add('counted');
            }
        }

        requestAnimationFrame(step);
    }

    // Sử dụng IntersectionObserver để tự động kích hoạt khi cuộn tới vị trí thẻ
    if ('IntersectionObserver' in window) {
        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    animateCounter(entry.target);
                    observer.unobserve(entry.target);
                }
            });
        }, { threshold: 0.15 });

        countElements.forEach(el => {
            // Lưu giá trị gốc vào data-countup nếu chưa có
            if (!el.dataset.countup) {
                el.dataset.countup = el.textContent.trim();
            }
            observer.observe(el);
        });
    } else {
        // Fallback trực tiếp
        countElements.forEach(el => animateCounter(el));
    }
}

// Hàm chạy lại Count Up ra ngoài window cho các sự kiện Ajax/Tab
window.runCountUp = function (container) {
    const root = container ? (typeof container === 'string' ? document.querySelector(container) : container) : document;
    if (!root) return;
    const elements = root.querySelectorAll('.count-up, [data-countup]');
    elements.forEach(el => {
        el.dataset.counted = 'false';
        el.classList.remove('counted');
    });
    initCountUpAnimation(root);
};

/* ==========================================================================
   2. SIDEBAR TRANSITION (MỞ RỘNG, THU GỌN VÀ DI ĐỘNG DRAWER)
   ========================================================================== */
function initSidebarTransition() {
    const sidebar = document.getElementById('adminSidebar');
    if (!sidebar) return;

    const collapseBtns = document.querySelectorAll('.sidebar-collapse-btn, #sidebarCollapseBtn');
    const toggleButtons = document.querySelectorAll('.sidebar-toggle-btn, [data-toggle="sidebar"], .btn:has(i.fa-bars)');
    const backdrop = document.getElementById('sidebarBackdrop');
    const logoIcons = sidebar.querySelectorAll('.sidebar-logo-icon');

    const STORAGE_KEY = 'exambank_sidebar_collapsed';

    // 1. Phục hồi trạng thái lưu từ localStorage (trên desktop)
    if (window.innerWidth >= 992) {
        const isSavedCollapsed = localStorage.getItem(STORAGE_KEY) === 'true';
        if (isSavedCollapsed) {
            sidebar.classList.add('collapsed');
            updateCollapseIcon(true);
        }
    }

    // Cập nhật icon mũi tên & tooltip
    function updateCollapseIcon(isCollapsed) {
        collapseBtns.forEach(btn => {
            const icon = btn.querySelector('i');
            if (icon) {
                if (isCollapsed) {
                    icon.className = 'fa-solid fa-angles-right';
                    btn.setAttribute('title', 'Mở rộng menu (Ctrl + B)');
                } else {
                    icon.className = 'fa-solid fa-angles-left';
                    btn.setAttribute('title', 'Thu gọn menu (Ctrl + B)');
                }
            }
        });

        // Cập nhật tooltip cho nút toggle trên navbar
        toggleButtons.forEach(btn => {
            btn.setAttribute('title', isCollapsed ? 'Mở rộng sidebar (Ctrl + B)' : 'Thu gọn sidebar (Ctrl + B)');
        });

        logoIcons.forEach(logo => {
            logo.setAttribute('title', isCollapsed ? 'Bấm để mở rộng sidebar' : '');
        });
    }

    // Toggle Desktop Collapse
    function toggleDesktopCollapse() {
        sidebar.classList.toggle('collapsed');
        const isCollapsed = sidebar.classList.contains('collapsed');
        localStorage.setItem(STORAGE_KEY, isCollapsed.toString());
        updateCollapseIcon(isCollapsed);

        // Kích hoạt cập nhật lại Bootstrap Tooltips
        initBootstrapTooltips();
    }

    // Toggle Mobile Drawer
    function toggleMobileDrawer() {
        sidebar.classList.toggle('mobile-open');
        if (backdrop) {
            backdrop.classList.toggle('show');
        }
    }

    function closeMobileDrawer() {
        sidebar.classList.remove('mobile-open');
        if (backdrop) {
            backdrop.classList.remove('show');
        }
    }

    // Sự kiện nút thu gọn / mở rộng trên Sidebar Header
    collapseBtns.forEach(btn => {
        btn.addEventListener('click', function (e) {
            e.preventDefault();
            e.stopPropagation();
            toggleDesktopCollapse();
        });
    });

    // Bấm vào Logo khi thu gọn để mở rộng
    logoIcons.forEach(logo => {
        logo.addEventListener('click', function (e) {
            if (sidebar.classList.contains('collapsed')) {
                e.preventDefault();
                toggleDesktopCollapse();
            }
        });
    });

    // Sự kiện các nút Toggle / Hamburger trên Top Navbar
    toggleButtons.forEach(btn => {
        btn.addEventListener('click', function (e) {
            e.preventDefault();
            if (window.innerWidth >= 992) {
                toggleDesktopCollapse();
            } else {
                toggleMobileDrawer();
            }
        });
    });

    // Phím tắt Ctrl + B (hoặc Cmd + B) để thu gọn / mở rộng nhanh
    document.addEventListener('keydown', function (e) {
        if ((e.ctrlKey || e.metaKey) && e.key.toLowerCase() === 'b') {
            e.preventDefault();
            if (window.innerWidth >= 992) {
                toggleDesktopCollapse();
            } else {
                toggleMobileDrawer();
            }
        }
    });

    // Bấm vào nền mờ để đóng sidebar trên điện thoại
    if (backdrop) {
        backdrop.addEventListener('click', closeMobileDrawer);
    }

    // Tự động đóng mobile sidebar khi click vào link chuyển trang
    const sidebarLinks = sidebar.querySelectorAll('.sidebar-link');
    sidebarLinks.forEach(link => {
        link.addEventListener('click', function () {
            if (window.innerWidth < 992) {
                closeMobileDrawer();
            }
        });
    });

    // Lắng nghe thay đổi kích thước màn hình
    window.addEventListener('resize', function () {
        if (window.innerWidth >= 992) {
            closeMobileDrawer();
        }
    });
}

/* ==========================================================================
   3. TAB SWITCHING ANIMATIONS (CHUYỂN TAB MƯỢT MÀ VÀ RE-TRIGGER COUNT UP)
   ========================================================================== */
function initTabAnimations() {
    const tabTriggers = document.querySelectorAll('button[data-bs-toggle="tab"], a[data-bs-toggle="tab"], button[data-bs-toggle="pill"], a[data-bs-toggle="pill"]');
    tabTriggers.forEach(trigger => {
        trigger.addEventListener('shown.bs.tab', function (event) {
            const targetSelector = event.target.getAttribute('data-bs-target') || event.target.getAttribute('href');
            if (targetSelector) {
                const targetPane = document.querySelector(targetSelector);
                if (targetPane) {
                    // Re-run count-up nếu tab có chứa số liệu thống kê
                    window.runCountUp(targetPane);
                }
            }
        });
    });
}

/* ==========================================================================
   4. SKELETON LOADING HELPER (CÔNG CỤ HIỂN THỊ KHUNG GIẢ TRẢI NGHIỆM TẢI DỮ LIỆU)
   ========================================================================== */
window.ExamBankSkeleton = {
    // Tạo khung giả cho một bảng dữ liệu (Table Skeleton)
    generateTableSkeletonHtml: function (rows = 4, cols = 5) {
        let html = '';
        for (let i = 0; i < rows; i++) {
            html += '<tr class="skeleton-table-row">';
            for (let j = 0; j < cols; j++) {
                const widthPercent = j === 0 ? '40%' : (j === 1 ? '85%' : (j === 2 ? '60%' : '75%'));
                html += `<td><div class="skeleton skeleton-text" style="width: ${widthPercent}; height: 16px;"></div></td>`;
            }
            html += '</tr>';
        }
        return html;
    },

    // Hiển thị skeleton và gọi callback sau delay giả lập
    simulateRefresh: function (tableBodySelector, callback, delayMs = 650) {
        const tbody = document.querySelector(tableBodySelector);
        if (!tbody) return;

        const originalHtml = tbody.innerHTML;
        const colCount = tbody.closest('table')?.querySelectorAll('thead th')?.length || 5;

        // Đặt khung skeleton
        tbody.innerHTML = this.generateTableSkeletonHtml(4, colCount);

        setTimeout(() => {
            if (typeof callback === 'function') {
                callback(tbody);
            } else {
                tbody.innerHTML = originalHtml;
                // Thêm animation mượt mà cho dòng dữ liệu mới
                tbody.querySelectorAll('tr').forEach((tr, index) => {
                    tr.classList.add('animate-fade-in-up');
                    tr.style.animationDelay = `${index * 0.05}s`;
                });
            }
        }, delayMs);
    }
};

/* ==========================================================================
   5. BOOTSTRAP TOOLTIPS CHO SIDEBAR KHI THU GỌN
   ========================================================================== */
function initBootstrapTooltips() {
    if (typeof bootstrap !== 'undefined' && bootstrap.Tooltip) {
        const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        tooltipTriggerList.map(function (tooltipTriggerEl) {
            return bootstrap.Tooltip.getInstance(tooltipTriggerEl) || new bootstrap.Tooltip(tooltipTriggerEl, {
                boundary: 'window',
                trigger: 'hover'
            });
        });
    }
}
