using System;
using System.Collections.Generic;

namespace FitnessLife.Models;

public partial class Feedback
{
    public decimal Feedbackid { get; set; }

    public decimal Userid { get; set; }

    public string Feedbacktext { get; set; } = null!;
    //public string? Username { get; set; }

    public DateTime? Submittedat { get; set; }

    public bool? Approved { get; set; }

    public virtual User User { get; set; } = null!;
}
