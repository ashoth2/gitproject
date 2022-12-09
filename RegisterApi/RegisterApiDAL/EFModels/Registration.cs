using System;
using System.Collections.Generic;

namespace RegisterApiDAL.EFModels
{
    public partial class Registration
    {
        public int Userid { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int Roleid { get; set; }
        public int Statusid { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual Status Status { get; set; } = null!;
    }
}
