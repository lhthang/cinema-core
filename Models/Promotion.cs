using cinema_core.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Models
{
	public class Promotion : BaseEntity
	{
		[Required]
		public string Code { get; set; }
		[Required]
		public decimal DiscountAmount { get; set; }
		[Required]
		public DateTime ExpiredDate { get; set; }
		[Required]
		public bool IsActive { get; set; }
		public virtual ICollection<Ticket> Tickets { get; set; }
	}
}
