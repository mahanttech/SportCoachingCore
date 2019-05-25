using System;
using System.Collections.Generic;

namespace Web.Api.Core.Entity
{
    public partial class TestAthleteMapping
    {
        public int Id { get; set; }
        public int testId { get; set; }
        public int? AthleteId { get; set; }
        public decimal? Distance { get; set; }
        public string FitnessRating { get; set; }
    }
}
