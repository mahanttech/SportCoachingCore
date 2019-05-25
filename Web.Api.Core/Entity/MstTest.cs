using System;
using System.Collections.Generic;

namespace Web.Api.Core.Entity
{
    public partial class MstTest
    {
        public int Id { get; set; }
        public string TestType { get; set; }
        public DateTime? TestDate { get; set; }
    }
}
