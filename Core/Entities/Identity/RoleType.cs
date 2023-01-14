using System.ComponentModel;

namespace Core.Entities.Identity
{
    public enum RoleType
    {
        [Description("系統管理員")]
        Admin,
        [Description("一般會員")]
        RegularMember,
        [Description("黃金會員")]
        GoldMember,
        [Description("鑽石會員")]
        DiamondMember
    }
}