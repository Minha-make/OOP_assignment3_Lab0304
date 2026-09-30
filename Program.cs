/****************/
/* Mã sinh viên: 202418890 */
/* Họ tên: Nguyễn Minh Hà */
/****************/

using System;
using System.Text;

namespace ProjectTeamManagement
{
    /// <summary>
    /// Chương trình kiểm thử theo kịch bản 15 bước của đề bài
    /// và bổ sung phần kiểm thử các trường hợp biên (bước 16).
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            // Hiển thị đúng tiếng Việt trên console
            Console.OutputEncoding = Encoding.UTF8;

            // ---------- 1. Tạo hai Employee bằng các constructor khác nhau ----------
            Step(1, "Tạo Employee bằng các constructor khác nhau");
            Employee e0 = new Employee();                                        // constructor mặc định
            Employee e1 = new Employee("E001", "Nguyễn Văn An");                 // 2 tham số (lương = 0)
            Employee e2 = new Employee("E002", "Trần Thị Bình", 12_000_000);     // 3 tham số
            e0.DisplayInfo();
            e1.DisplayInfo();
            e2.DisplayInfo();

            // ---------- 2. Tạo hai SoftwareEngineer bằng hai constructor khác nhau ----------
            Step(2, "Tạo SoftwareEngineer bằng hai constructor khác nhau");
            SoftwareEngineer se1 = new SoftwareEngineer("SE01", "Lê Văn Cường", "C#");
            SoftwareEngineer se2 = new SoftwareEngineer("SE02", "Phạm Thị Dung", 20_000_000, "Java", 3_000_000);
            se1.DisplayInfo();
            se2.DisplayInfo();

            // ---------- 3. Tăng lương bằng số tiền cố định ----------
            Step(3, "Tăng lương cố định: E001 + 5,000,000");
            e1.IncreaseSalary(5_000_000);
            e1.DisplayInfo();

            // ---------- 4. Tăng lương theo phần trăm ----------
            Step(4, "Tăng lương theo phần trăm: E002 + 10%");
            e2.IncreaseSalary(10, true);
            e2.DisplayInfo();

            // ---------- 5. Tạo nhóm không có trưởng nhóm ----------
            Step(5, "Tạo nhóm dự án PRJ01 chưa có trưởng nhóm");
            ProjectTeam team1 = new ProjectTeam("PRJ01", "Hệ thống quản lý nhân sự");
            team1.DisplayTeam();

            // ---------- 6. Thêm nhân sự bằng addMember(employee) ----------
            Step(6, "Thêm thành viên bằng AddMember(employee)");
            Console.WriteLine($"AddMember(E001) -> {team1.AddMember(e1)}");
            Console.WriteLine($"AddMember(E002) -> {team1.AddMember(e2)}");
            Console.WriteLine($"AddMember(SE01) -> {team1.AddMember(se1)}");

            // ---------- 7. Thêm kỹ sư và đặt làm trưởng nhóm ----------
            Step(7, "Thêm kỹ sư SE02 bằng AddMember(employee, true) để làm trưởng nhóm");
            Console.WriteLine($"AddMember(SE02, true) -> {team1.AddMember(se2, true)}");
            Console.WriteLine($"Trưởng nhóm hiện tại: {team1.Leader?.Id}");

            // ---------- 8. Thử thêm lại thành viên đã tồn tại ----------
            Step(8, "Thêm lại E001 (đã tồn tại) -> phải bị từ chối");
            Console.WriteLine($"AddMember(E001) -> {team1.AddMember(e1)}");
            Console.WriteLine($"Số thành viên vẫn là: {team1.MemberCount}");

            // ---------- 9. Hiển thị danh sách bằng lời gọi đa hình ----------
            Step(9, "Hiển thị nhóm (đa hình DisplayInfo)");
            team1.DisplayTeam();

            // ---------- 10. Tổng chi phí hằng tháng ----------
            Step(10, "Tổng chi phí nhân sự hằng tháng");
            Console.WriteLine($"Tổng chi phí: {team1.CalculateTotalMonthlyCost():N0} VND");

            // ---------- 11. Thử xóa trưởng nhóm hiện tại ----------
            Step(11, "Xóa trưởng nhóm hiện tại (SE02) -> phải bị từ chối");
            bool removed = team1.RemoveMember("SE02");
            Console.WriteLine($"RemoveMember(SE02) -> {removed} " + (removed ? "(LỖI: lẽ ra phải bị từ chối)" : "(đã bị từ chối đúng)"));

            // ---------- 12. Đổi trưởng nhóm rồi xóa người từng là trưởng nhóm ----------
            Step(12, "Đổi trưởng nhóm sang E002 rồi xóa SE02");
            Console.WriteLine($"ChangeLeader(E002) -> {team1.ChangeLeader(e2)}");
            Console.WriteLine($"RemoveMember(SE02) -> {team1.RemoveMember("SE02")}");
            Console.WriteLine($"Nhóm còn chứa SE02 không? {team1.Contains("SE02")}");
            Console.WriteLine("SE02 vẫn tồn tại ngoài nhóm:");
            se2.DisplayInfo();
            team1.DisplayTeam();
            Console.WriteLine($"Tổng chi phí mới: {team1.CalculateTotalMonthlyCost():N0} VND");

            // ---------- 13 & 14. Nhóm thứ hai trong khối lệnh cục bộ ----------
            Step(13, "Tạo nhóm PRJ02, thêm E001 (đã có ở PRJ01) -> kết tập nhiều nhóm");
            using (ProjectTeam team2 = new ProjectTeam("PRJ02", "Ứng dụng di động", se2))
            {
                Console.WriteLine($"AddMember(E001) vào PRJ02 -> {team2.AddMember(e1)}");
                team2.DisplayTeam();
                Console.WriteLine($"E001 thuộc PRJ01? {team1.Contains("E001")} | thuộc PRJ02? {team2.Contains("E001")}");

                Step(14, "Kết thúc khối lệnh -> nhóm PRJ02 bị hủy (Dispose)");
            } // <-- team2.Dispose() được gọi tự động tại đây

            // ---------- 15. Chứng minh nhân sự vẫn tồn tại ----------
            Step(15, "Sau khi PRJ02 bị hủy, nhân sự vẫn tồn tại");
            e1.DisplayInfo();
            se2.DisplayInfo();
            Console.WriteLine($"PRJ01 vẫn còn E001: {team1.Contains("E001")} | số thành viên PRJ01: {team1.MemberCount}");

            // ---------- 16. Các trường hợp biên ----------
            Step(16, "Kiểm thử các trường hợp biên");
            RunEdgeCases(team1, e0);

            // ---------- Dọn dẹp: hủy nhóm trước, rồi mới hủy các nhân sự ----------
            Step(17, "Kết thúc chương trình: hủy nhóm rồi hủy nhân sự (quan sát vòng đời)");
            team1.Dispose();
            e0.Dispose();
            e1.Dispose();
            e2.Dispose();
            se1.Dispose();
            se2.Dispose();

            Console.WriteLine();
            Console.WriteLine("Nhấn phím bất kỳ để thoát...");
            Console.ReadKey();
        }

        /// <summary>Kiểm thử các trường hợp biên: dữ liệu sai, trùng mã, null, xóa trưởng nhóm duy nhất...</summary>
        private static void RunEdgeCases(ProjectTeam team1, Employee e0)
        {
            // --- Vi phạm ràng buộc dữ liệu: phải ném ngoại lệ ---
            ExpectException("Employee: mã rỗng", () => { _ = new Employee("", "Ten"); });
            ExpectException("Employee: họ tên chỉ có khoảng trắng", () => { _ = new Employee("E9", "   "); });
            ExpectException("Employee: lương cơ bản âm", () => { _ = new Employee("E9", "Ten", -1); });
            ExpectException("SoftwareEngineer: ngôn ngữ chính rỗng", () => { _ = new SoftwareEngineer("S9", "Ten", ""); });
            ExpectException("SoftwareEngineer: phụ cấp âm", () => { _ = new SoftwareEngineer("S9", "Ten", 1000, "C++", -5); });
            ExpectException("IncreaseSalary(-100)", () => e0.IncreaseSalary(-100));
            ExpectException("IncreaseSalary(0, phần trăm)", () => e0.IncreaseSalary(0, true));
            ExpectException("ProjectTeam: mã dự án rỗng", () => { _ = new ProjectTeam("", "Ten"); });
            ExpectException("ProjectTeam: trưởng nhóm null", () => { _ = new ProjectTeam("P9", "Ten", null!); });
            ExpectException("AddMember(null)", () => team1.AddMember(null!));

            // --- Các thao tác bị từ chối bằng giá trị trả về false ---
            Employee trungMa = new Employee("E001", "Người khác nhưng trùng mã");
            Check("AddMember: đối tượng khác nhưng trùng mã E001 bị từ chối", team1.AddMember(trungMa) == false);
            Check("ChangeLeader: đối tượng khác trùng mã E001 bị từ chối", team1.ChangeLeader(trungMa) == false);
            Check("RemoveMember: mã không tồn tại trả về false", team1.RemoveMember("KHONG_TON_TAI") == false);
            Check("RemoveMember: mã rỗng trả về false", team1.RemoveMember("") == false);
            Check("Contains: mã null/rỗng trả về false", team1.Contains("") == false);

            // --- Nhóm chỉ có một thành viên là trưởng nhóm ---
            using (ProjectTeam solo = new ProjectTeam("P_SOLO", "Nhóm một người", e0))
            {
                Check("Không thể xóa trưởng nhóm duy nhất", solo.RemoveMember(e0.Id) == false);
                Check("Thêm lại chính trưởng nhóm bị từ chối (trùng)", solo.AddMember(e0) == false);
                Check("Nhóm một người: tổng chi phí = chi phí của e0", solo.CalculateTotalMonthlyCost() == e0.CalculateMonthlyCost());
            }

            // --- Nhóm rỗng ---
            using (ProjectTeam empty = new ProjectTeam("P_EMPTY", "Nhóm rỗng"))
            {
                empty.DisplayTeam();
                Check("Nhóm rỗng: tổng chi phí = 0", empty.CalculateTotalMonthlyCost() == 0);
            }

            trungMa.Dispose(); // đối tượng tạo thêm chỉ để kiểm thử
        }

        /// <summary>In tiêu đề cho từng kịch bản kiểm thử.</summary>
        private static void Step(int number, string title)
        {
            Console.WriteLine();
            Console.WriteLine($"--- Kịch bản {number}: {title} ---");
        }

        /// <summary>Chạy một thao tác và mong đợi nó ném ngoại lệ.</summary>
        private static void ExpectException(string description, Action action)
        {
            try
            {
                action();
                Console.WriteLine($"  [FAIL] {description}: KHÔNG ném ngoại lệ.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  [PASS] {description} -> {ex.GetType().Name}");
            }
        }

        /// <summary>In kết quả PASS/FAIL cho một điều kiện.</summary>
        private static void Check(string description, bool condition)
        {
            Console.WriteLine($"  [{(condition ? "PASS" : "FAIL")}] {description}");
        }
    }
}
