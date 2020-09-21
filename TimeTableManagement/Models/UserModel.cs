using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableManagement.Models
{
    [Table("UserTable")]
    public class UserModel
    {
        [Key]
        public string Role { get; set; }
        public int LeaveOnMonday { get; set; }
        public int LeaveOnWednesday { get; set; }
        public int LeaveOnFriday { get; set; }
        public string SubstituteTeacherOnMonday { get; set; }
        public string SubstituteTeacherOnWednesday { get; set; }
        public string SubstituteTeacherOnFriday { get; set; }
    }
}
