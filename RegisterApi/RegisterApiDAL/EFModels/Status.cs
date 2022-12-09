using System;
using System.Collections.Generic;

namespace RegisterApiDAL.EFModels
{
    public partial class Status
    {
        public Status()
        {
            Registrations = new HashSet<Registration>();
            Roles = new HashSet<Role>();
        }

        public int Statusid { get; set; }
        public string? Status1 { get; set; }

        public virtual ICollection<Registration> Registrations { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}
