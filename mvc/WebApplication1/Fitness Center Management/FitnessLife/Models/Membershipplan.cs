﻿using System;
using System.Collections.Generic;

namespace FitnessLife.Models;

public partial class Membershipplan
{
    public decimal Planid { get; set; }

    public string Planname { get; set; } = null!;

    public decimal Durationinmonths { get; set; }

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public DateTime? Createdat { get; set; }

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
