using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Data.Domain.ViewModels
{
    public enum PermissionsEnum
    {
        ///panel
        [Description("مجوز دیدن پنل اطلاعات پایه")]
        ViewBaseInfoPanel,
        [Description("مجوز دیدن پنل کاربران")]
        ViewPersonPanel,
        [Description("مجوز دیدن پنل مدیریت")]
        ViewAdministratorPanel,
        [Description("مجوز دیدن پنل ایمیل ها")]
        ViewEmailPanel,
        [Description("مجوز دیدن پنل گزارشات")]
        ViewReportsPanel,


    }
}