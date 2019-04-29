﻿using System;

namespace MovieApplication.Models
{
    public class UserMovie
    {
        public int Id { get; set; }
        public Movie Movie { get; set; }
        public int MovieId { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}
