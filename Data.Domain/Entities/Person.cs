using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Data.Domain.Entities
{
    public class Person
    {
        public Person()
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

        [DisplayName("نام شخص")]
        public string Name { get; set; }

        [DisplayName("نام خانوادگی")]
        public string LastName { get; set; }

        [DisplayName("نام پدر")]
        public string FatherName { get; set; }

        [DisplayName("کد ملی")]
        public long? NationalCode { get; set; }

        [DisplayName("کد پرسنلی")]
        public string Code { get; set; }

        [DisplayName("تاریخ تولد")]
        public DateTime? BirthDate { get; set; }

        [DisplayName("تلفن ثابت")]
        public string Telphone { get; set; }

        [DisplayName("فکس")]
        public string TelFax { get; set; }

        [DisplayName("موبایل")]
        public string Mobile { get; set; }

        [DisplayName("آدرس")]
        public string Address { get; set; }

        [DisplayName("وضعیت")]
        public bool State { get; set; }

        public virtual Person Parent { get; set; }

        [DisplayName("نام کاربری ")]
        public string UserName { get; set; }

        [DisplayName("کلمه عبور ")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("ایمیل")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public virtual ICollection<Person> SubPerson { get; set; }
    }
}