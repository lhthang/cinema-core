using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Form
{
	public class ActorRequest
	{

		[Required]
		public string Name { get; set; }
		public string Avatar { get; set; }
	}
}
