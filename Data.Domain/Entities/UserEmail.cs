using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Data.Domain.Entities
{
    [Table("UserEmail")]
    public class UserEmail
    {
        [Key, ForeignKey("ApplicationUser"), Column(Order = 1)]
        public string UserId { get; set; }

        [Key, ForeignKey("Email"), Column(Order = 2)]
        public Guid EmailId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Email Email { get; set; }

        public bool IsView { get; set; }

        [DisplayName("توضیحات")]
        public string Description { get; set; }
    }
}