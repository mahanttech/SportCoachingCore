using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Api.Infrastructure.Models
{
    public partial class TestAthleteMappingModel
    {
        public int Id { get; set; }
        [Required]
        public int? AthleteId { get; set; }
        [Required]
        public int testId { get; set; }
        public string  AthleteName { get; set; }
        [Required]
        public decimal? Distance { get; set; }
        public string FitnessRating { get; set; }
    }
}
