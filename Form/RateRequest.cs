using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Form
{
	public class RateRequest
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public int MinAge { get; set; }
	}
}
