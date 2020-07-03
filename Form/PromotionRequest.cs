using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Form
{
	public class PromotionRequest
	{
		[Required]
		public string Code { get; set; }
		[Required]
		public decimal DiscountAmount { get; set; }
		[Required]
		public DateTime ExpiredDate { get; set; }
		[Required]
		public bool IsActive { get; set; }
	}
}
