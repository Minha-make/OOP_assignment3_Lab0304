/****************/
/* Mã sinh viên: 202418890 */
/* Họ tên: Nguyễn Minh Hà */
/****************/

using System;
using System.Globalization;

namespace ProjectTeamManagement
{
    /// <summary>
    /// Lớp Employee: đại diện cho một nhân sự thông thường.
    /// Trách nhiệm: quản lý thông tin cơ bản (mã, họ tên, lương cơ bản),
    /// tính chi phí hằng tháng và hiển thị thông tin.
    /// Nhân sự tồn tại độc lập với dự án (không phụ thuộc vào ProjectTeam).
    /// Cài đặt IDisposable để quan sát vòng đời đối tượng (thay cho destructor của C++).
    /// </summary>
    public class Employee : IDisposable
    {
        // ===== Thuộc tính (private field + property chỉ đọc để bảo vệ bất biến) =====
        private readonly string _id;      // mã nhân sự (không rỗng)
        private readonly string _fullName; // họ tên (không rỗng)
        private double _baseSalary;        // lương cơ bản (không âm)

        /// <summary>Mã nhân sự.</summary>
        public string Id => _id;

        /// <summary>Họ tên nhân sự.</summary>
        public string FullName => _fullName;

        /// <summary>Lương cơ bản.</summary>
        public double BaseSalary => _baseSalary;

        // ===== Constructor (nạp chồng) =====

        /// <summary>Constructor mặc định: id = "UNKNOWN", fullName = "Unnamed employee", baseSalary = 0.</summary>
        public Employee() : this("UNKNOWN", "Unnamed employee", 0)
        {
        }

        /// <summary>Constructor hai tham số: lương cơ bản mặc định = 0.</summary>
        public Employee(string id, string fullName) : this(id, fullName, 0)
        {
        }

        /// <summary>Constructor đầy đủ: kiểm tra toàn bộ ràng buộc dữ liệu.</summary>
        /// <exception cref="ArgumentException">Mã hoặc họ tên rỗng.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Lương cơ bản âm hoặc không hợp lệ.</exception>
        public Employee(string id, string fullName, double baseSalary)
        {
            // Bất biến 1: mã nhân sự không rỗng
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Mã nhân sự không được rỗng.", nameof(id));

            // Họ tên không rỗng
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Họ tên không được rỗng.", nameof(fullName));

            // Bất biến 2: lương cơ bản không âm (chặn cả NaN và vô cực)
            if (!IsValidNumber(baseSalary) || baseSalary < 0)
                throw new ArgumentOutOfRangeException(nameof(baseSalary), "Lương cơ bản phải là số không âm.");

            _id = id.Trim();
            _fullName = fullName.Trim();
            _baseSalary = baseSalary;
        }

        // ===== Nạp chồng phương thức increaseSalary =====

        /// <summary>Tăng lương theo một số tiền cố định.</summary>
        /// <param name="amount">Số tiền tăng, phải dương.</param>
        public void IncreaseSalary(double amount)
        {
            // Phiên bản 1 = phiên bản 2 với byPercentage = false
            IncreaseSalary(amount, false);
        }

        /// <summary>
        /// Tăng lương theo số tiền cố định hoặc theo phần trăm.
        /// </summary>
        /// <param name="value">Giá trị tăng (số tiền hoặc số phần trăm), phải dương.</param>
        /// <param name="byPercentage">true: tăng value% trên lương hiện tại; false: tăng value đồng.</param>
        public void IncreaseSalary(double value, bool byPercentage)
        {
            // Giá trị tăng phải dương
            if (!IsValidNumber(value) || value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Giá trị tăng phải dương.");

            double increment = byPercentage ? _baseSalary * value / 100.0 : value;
            double newSalary = _baseSalary + increment;

            // Bảo vệ trường hợp tràn số
            if (!IsValidNumber(newSalary))
                throw new OverflowException("Lương sau khi tăng vượt quá giới hạn biểu diễn.");

            _baseSalary = newSalary;
        }

        // ===== Phương thức ảo (đa hình) =====

        /// <summary>Chi phí nhân sự hằng tháng. Mặc định bằng lương cơ bản.</summary>
        public virtual double CalculateMonthlyCost()
        {
            return _baseSalary;
        }

        /// <summary>Hiển thị thông tin nhân sự (một dòng).</summary>
        public virtual void DisplayInfo()
        {
            Console.WriteLine(
                $"[Nhân sự] {Id} | {FullName} | Lương cơ bản: {FormatMoney(BaseSalary)} | " +
                $"Chi phí/tháng: {FormatMoney(CalculateMonthlyCost())}");
        }

        /// <summary>
        /// "Destructor" quan sát vòng đời: in thông báo khi đối tượng bị hủy.
        /// Lớp dẫn xuất ghi đè và gọi base.Dispose() để mô phỏng thứ tự hủy của C++
        /// (lớp dẫn xuất trước, lớp cơ sở sau).
        /// </summary>
        public virtual void Dispose()
        {
            Console.WriteLine($"[Employee] Hủy đối tượng: {Id} - {FullName}");
        }

        // ===== Hàm hỗ trợ =====

        /// <summary>Kiểm tra số thực hợp lệ (không NaN, không vô cực).</summary>
        protected static bool IsValidNumber(double value)
        {
            return !double.IsNaN(value) && !double.IsInfinity(value);
        }

        /// <summary>Định dạng tiền: 12,000,000 VND.</summary>
        protected static string FormatMoney(double value)
        {
            return value.ToString("N0", CultureInfo.InvariantCulture) + " VND";
        }
    }
}
