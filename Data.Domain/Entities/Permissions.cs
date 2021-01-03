using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Data.Domain.Entities
{
    [Table("Permissions")]
    public class Permissions
    {
        public Permissions()
        {
            Id = Guid.NewGuid();
            CreateDateTime = DateTime.Now;
            ModifyDateTime = DateTime.Now;
        }

        [Key]
        public Guid Id { get; set; }

        [DisplayName("تاریخ ثبت")]
        public DateTime CreateDateTime { get; set; }

        [DisplayName("تاریخ آخرین ویرایش")]
        public DateTime ModifyDateTime { get; set; }
        public bool IsDelete { get; set; }
        [NotMapped]
        public bool IsActive { get; set; }

        [DisplayName("توضیحات")]
        public string Description { get; set; }

        public string Name { get; set; }
        public virtual ICollection<PermissionsRoles> PermissionsRoles { get; set; }
    }
}