using cinema_core.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Models
{
    public class Rate : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int MinAge { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }

    }
}
