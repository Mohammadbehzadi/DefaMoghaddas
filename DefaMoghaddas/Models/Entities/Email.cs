using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DefaMoghaddas.Models.Entities
{
    [Table("Email")]
    public class Email
    {
        public Email()
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

        [DisplayName("توضیحات")]
        public string Description { get; set; }


        [DisplayName("موضوع")]
        public string Subject { get; set; }


        [AllowHtml]
        [DisplayName("متن پیام")]
        public string Body { get; set; }
        public virtual ICollection<UserEmail> To { get; set; }
    }
}