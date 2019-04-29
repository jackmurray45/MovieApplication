using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApplication.Models
{
    public class MovieAndUser
    {
        public HashSet<int> UserMovies { get; set; }
        public List<Movie> Movies { get; set; }
    }
}
