﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Airport.Data.Model
{
    [Table("NextStation")]
    public partial class NextStation
    {
        [Key]
        public int SourceId { get; set; }
        [Key]
        public int TargetId { get; set; }
        public bool? FlightType { get; set; }

        [ForeignKey("SourceId")]
        [InverseProperty("NextStationSources")]
        public virtual Station Source { get; set; } = null!;
        [ForeignKey("TargetId")]
        [InverseProperty("NextStationTargets")]
        public virtual Station Target { get; set; } = null!;
    }
}
