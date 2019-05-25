using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Api.Infrastructure.Models
{
    public partial class MstTestModel
    {
        public int Id { get; set; }
        public int NoOfParticipant { get; set; }
        [Required]
        public string TestType { get; set; }
        [Required]
        public DateTime? TestDate { get; set; }
    }
}
