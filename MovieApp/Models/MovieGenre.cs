using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MovieApp.Models
{
    public class MovieGenre
    {
        public List<Movie> Movies { get; set; }
#nullable enable
        public SelectList? Genres { get; set; }
#nullable enable
        public string? Genre { get; set; }
#nullable enable
        public string? Title { get; set; }
    }
}

