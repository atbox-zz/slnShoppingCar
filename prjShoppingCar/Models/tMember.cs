using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace prjShoppingCar.Models
{
    using System;
    using System.Collections.Generic;

    public partial class tMember
    {
        public int fId { get; set; }

        [DisplayName("帳號")]
        [Required]
        public string fUserId { get; set; }

        [DisplayName("密碼")]
        [Required]
        public string fPwd { get; set; }

        [DisplayName("姓名")]
        [Required]
        public string fName { get; set; }

        [DisplayName("信箱")]
        [Required]
        [EmailAddress]
        public string fEmail { get; set; }
    }
}




