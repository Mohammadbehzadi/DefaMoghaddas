using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DefaMoghaddas.Models.Entities
{
    [Table("ActionLog")]
    public class ActionLog
    {
        public ActionLog()
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
        [DisplayName("کد شعبه")]
        public string BranchCode { get; set; }
        public string ActionName { get; set; }
        public string ContollerName { get; set; }
        public string Ip { get; set; }
        public string User { get; set; }
        public string Type { get; set; }
        public Person Person { get; set; }
    }
}