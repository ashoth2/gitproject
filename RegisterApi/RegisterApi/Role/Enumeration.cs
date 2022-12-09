using System.ComponentModel;

namespace RegisterApi.Role
{
    public enum Enumeration
    {
        [Description("Admin")]
        Admin,
        [Description("Junior")]
        Junior,
        [Description("Intern")]
        Intern,
        [Description("User")]
        User,
        [Description("Lead")]
        Lead
    }
}
