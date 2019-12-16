﻿using Cohousing.Server.Model.Factories;

namespace Cohousing.Server.Model.Models
{
    public class CommonMealRegistration
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public bool Attending { get; set; }
        public CommonMealGuestRegistration Guests { get; set;}
    }
}