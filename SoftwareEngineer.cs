/****************/
/* Mã sinh viên: 202418890 */
/* Họ tên: Nguyễn Minh Hà */
/****************/

using System;

namespace ProjectTeamManagement
{
    /// <summary>
    /// Lớp SoftwareEngineer: kỹ sư phần mềm, kế thừa công khai từ Employee.
    /// Bổ sung ngôn ngữ lập trình chính và phụ cấp kỹ thuật.
    /// </summary>
    public class SoftwareEngineer : Employee
    {
        // ===== Thuộc tính bổ sung =====
        private readonly string _primaryLanguage;   // ngôn ngữ lập trình chính (không rỗng)
        private readonly double _technicalAllowance; // phụ cấp kỹ thuật (không âm)

        /// <summary>Ngôn ngữ lập trình chính.</summary>
        public string PrimaryLanguage => _primaryLanguage;

        /// <summary>Phụ cấp kỹ thuật.</summary>
        public double TechnicalAllowance => _technicalAllowance;

        // ===== Constructor (nạp chồng) =====

        /// <summary>Constructor ba tham số: lương cơ bản = 0, phụ cấp = 0.</summary>
        public SoftwareEngineer(string id, string fullName, string primaryLanguage)
            : this(id, fullName, 0, primaryLanguage, 0)
        {
        }

        /// <summary>Constructor đầy đủ: kiểm tra ngôn ngữ chính và phụ cấp.</summary>
        /// <exception cref="ArgumentException">Ngôn ngữ chính rỗng.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Phụ cấp âm hoặc không hợp lệ.</exception>
        public SoftwareEngineer(string id, string fullName, double baseSalary,
                                string primaryLanguage, double technicalAllowance)
            : base(id, fullName, baseSalary) // Employee kiểm tra mã, họ tên, lương
        {
            if (string.IsNullOrWhiteSpace(primaryLanguage))
                throw new ArgumentException("Ngôn ngữ lập trình chính không được rỗng.", nameof(primaryLanguage));

            if (!IsValidNumber(technicalAllowance) || technicalAllowance < 0)
                throw new ArgumentOutOfRangeException(nameof(technicalAllowance), "Phụ cấp kỹ thuật phải là số không âm.");

            _primaryLanguage = primaryLanguage.Trim();
            _technicalAllowance = technicalAllowance;
        }

        // ===== Ghi đè phương thức ảo =====

        /// <summary>Chi phí hằng tháng = lương cơ bản + phụ cấp kỹ thuật.</summary>
        public override double CalculateMonthlyCost()
        {
            return BaseSalary + _technicalAllowance;
        }

        /// <summary>Hiển thị thông tin kỹ sư (gồm ngôn ngữ chính và phụ cấp).</summary>
        public override void DisplayInfo()
        {
            Console.WriteLine(
                $"[Kỹ sư PM] {Id} | {FullName} | Lương cơ bản: {FormatMoney(BaseSalary)} | " +
                $"Ngôn ngữ: {PrimaryLanguage} | Phụ cấp: {FormatMoney(TechnicalAllowance)} | " +
                $"Chi phí/tháng: {FormatMoney(CalculateMonthlyCost())}");
        }

        /// <summary>In thông báo hủy của lớp dẫn xuất, sau đó hủy phần lớp cơ sở.</summary>
        public override void Dispose()
        {
            Console.WriteLine($"[SoftwareEngineer] Hủy đối tượng: {Id} - {FullName} ({PrimaryLanguage})");
            base.Dispose();
        }
    }
}
