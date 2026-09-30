/****************/
/* Mã sinh viên: 202418890 */
/* Họ tên: Nguyễn Minh Hà */
/****************/

using System;
using System.Collections.Generic;

namespace ProjectTeamManagement
{
    /// <summary>
    /// Lớp ProjectTeam: nhóm dự án gồm một trưởng nhóm và danh sách thành viên.
    /// Quan hệ với Employee là KẾT TẬP (aggregation, liên kết không sở hữu):
    /// nhóm chỉ giữ tham chiếu, không tạo ra và không hủy Employee;
    /// một Employee có thể thuộc nhiều nhóm.
    /// 
    /// Các bất biến được duy trì:
    ///  (1) Trưởng nhóm (nếu có) luôn nằm trong danh sách thành viên.
    ///  (2) Không có hai thành viên cùng mã trong một nhóm.
    ///  (3) Một nhân sự không xuất hiện hai lần trong cùng nhóm.
    ///  (4) Không xóa trưởng nhóm khi chưa chọn người thay thế.
    /// </summary>
    public class ProjectTeam : IDisposable
    {
        private readonly string _projectCode;   // mã dự án (không rỗng)
        private readonly string _projectName;   // tên dự án (không rỗng)
        private Employee? _leader;              // liên kết không sở hữu tới trưởng nhóm (có thể null)
        private readonly List<Employee> _members = new List<Employee>(); // danh sách liên kết không sở hữu
        private bool _disposed;                 // tránh hủy nhóm hai lần

        /// <summary>Mã dự án.</summary>
        public string ProjectCode => _projectCode;

        /// <summary>Tên dự án.</summary>
        public string ProjectName => _projectName;

        /// <summary>Trưởng nhóm hiện tại (null nếu chưa có).</summary>
        public Employee? Leader => _leader;

        /// <summary>Số thành viên (đã gồm trưởng nhóm).</summary>
        public int MemberCount => _members.Count;

        // ===== Constructor (nạp chồng) =====

        /// <summary>Tạo nhóm chưa có trưởng nhóm.</summary>
        public ProjectTeam(string projectCode, string projectName)
        {
            if (string.IsNullOrWhiteSpace(projectCode))
                throw new ArgumentException("Mã dự án không được rỗng.", nameof(projectCode));
            if (string.IsNullOrWhiteSpace(projectName))
                throw new ArgumentException("Tên dự án không được rỗng.", nameof(projectName));

            _projectCode = projectCode.Trim();
            _projectName = projectName.Trim();
        }

        /// <summary>Tạo nhóm có trưởng nhóm; trưởng nhóm tự động được đưa vào danh sách thành viên.</summary>
        public ProjectTeam(string projectCode, string projectName, Employee leader)
            : this(projectCode, projectName)
        {
            if (leader == null)
                throw new ArgumentNullException(nameof(leader), "Trưởng nhóm không được null.");

            _members.Add(leader); // bất biến: trưởng nhóm thuộc danh sách thành viên
            _leader = leader;
        }

        // ===== Nạp chồng phương thức addMember =====

        /// <summary>Thêm một thành viên thường.</summary>
        /// <returns>true nếu thêm thành công; false nếu nhân sự đã có trong nhóm (trùng mã).</returns>
        public bool AddMember(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            // Không thêm trùng (theo mã nhân sự -> cũng chặn cùng đối tượng thêm hai lần)
            if (Contains(employee.Id))
                return false;

            _members.Add(employee);
            return true;
        }

        /// <summary>
        /// Thêm thành viên và tùy chọn đặt làm trưởng nhóm.
        /// Trưởng nhóm cũ vẫn là thành viên của nhóm.
        /// </summary>
        /// <returns>true nếu thêm thành công; false nếu bị trùng (khi đó không thay đổi gì).</returns>
        public bool AddMember(Employee employee, bool makeLeader)
        {
            if (!AddMember(employee))
                return false;

            if (makeLeader)
                _leader = employee;

            return true;
        }

        // ===== Các phương thức khác =====

        /// <summary>
        /// Xóa thành viên theo mã.
        /// Từ chối (false) nếu không tìm thấy hoặc nếu đó là trưởng nhóm hiện tại.
        /// </summary>
        public bool RemoveMember(string employeeId)
        {
            int index = IndexOfId(employeeId);
            if (index < 0)
                return false; // không có trong nhóm

            // Không được xóa trưởng nhóm khi chưa chọn người thay thế
            if (_leader != null && _members[index].Id == _leader.Id)
                return false;

            _members.RemoveAt(index); // chỉ gỡ liên kết, KHÔNG hủy Employee
            return true;
        }

        /// <summary>
        /// Đổi trưởng nhóm. Nếu nhân sự chưa thuộc nhóm thì được thêm vào nhóm trước.
        /// </summary>
        /// <returns>false nếu đã có một nhân sự KHÁC (đối tượng khác) trùng mã trong nhóm.</returns>
        public bool ChangeLeader(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            int index = IndexOfId(employee.Id);

            // Trùng mã nhưng là đối tượng khác -> vi phạm bất biến không trùng mã
            if (index >= 0 && !ReferenceEquals(_members[index], employee))
                return false;

            if (index < 0)
                _members.Add(employee); // trưởng nhóm mới phải là thành viên

            _leader = employee;
            return true;
        }

        /// <summary>Kiểm tra nhóm có nhân sự với mã cho trước hay không.</summary>
        public bool Contains(string employeeId)
        {
            return IndexOfId(employeeId) >= 0;
        }

        /// <summary>Tổng chi phí hằng tháng của cả nhóm (gọi đa hình CalculateMonthlyCost).</summary>
        public double CalculateTotalMonthlyCost()
        {
            double total = 0;
            foreach (Employee member in _members)
                total += member.CalculateMonthlyCost(); // trưởng nhóm nằm trong danh sách nên không bị tính hai lần
            return total;
        }

        /// <summary>Hiển thị thông tin nhóm và danh sách thành viên (gọi đa hình DisplayInfo).</summary>
        public void DisplayTeam()
        {
            Console.WriteLine($"=== Dự án {ProjectCode}: {ProjectName} ===");
            Console.WriteLine(_leader == null
                ? "Trưởng nhóm: (chưa có)"
                : $"Trưởng nhóm: {_leader.Id} - {_leader.FullName}");
            Console.WriteLine($"Số thành viên: {MemberCount}");

            if (_members.Count == 0)
            {
                Console.WriteLine("  (nhóm chưa có thành viên)");
                return;
            }

            foreach (Employee member in _members)
            {
                bool isLeader = _leader != null && member.Id == _leader.Id;
                Console.Write(isLeader ? " * " : "   "); // dấu * đánh dấu trưởng nhóm
                member.DisplayInfo();                     // đa hình
            }
        }

        /// <summary>
        /// "Destructor" của nhóm: chỉ hủy cấu trúc danh sách liên kết nội bộ,
        /// TUYỆT ĐỐI không hủy các đối tượng Employee (nhóm không sở hữu chúng).
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            int links = _members.Count;
            _members.Clear(); // gỡ các tham chiếu, không gọi Dispose trên Employee
            _leader = null;
            _disposed = true;

            Console.WriteLine($"[ProjectTeam] Hủy nhóm {_projectCode}: gỡ {links} liên kết, KHÔNG hủy Employee.");
        }

        // ===== Hàm hỗ trợ =====

        /// <summary>Tìm vị trí của nhân sự theo mã; -1 nếu không có hoặc mã rỗng.</summary>
        private int IndexOfId(string employeeId)
        {
            if (string.IsNullOrWhiteSpace(employeeId))
                return -1;

            string key = employeeId.Trim();
            for (int i = 0; i < _members.Count; i++)
            {
                if (_members[i].Id == key)
                    return i;
            }
            return -1;
        }
    }
}
