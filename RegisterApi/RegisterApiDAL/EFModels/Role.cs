using System;
using System.Collections.Generic;

namespace RegisterApiDAL.EFModels
{
    public partial class Role
    {
        public Role()
        {
            Registrations = new HashSet<Registration>();
        }

        public int Roleid { get; set; }
        public string Roles { get; set; } = null!;
        public int Statusid { get; set; }

        public virtual Status Status { get; set; } = null!;
        public virtual ICollection<Registration> Registrations { get; set; }
    }
}
