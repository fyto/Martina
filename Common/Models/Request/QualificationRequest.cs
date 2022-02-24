using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.Models.Request
{
    public class QualificationRequest
    {
        [Required]
        public string UserId { get; set; }

        [Range(0, 5)]
        [Required]
        public float Score { get; set; }

        public string Remarks { get; set; }
    }
}
