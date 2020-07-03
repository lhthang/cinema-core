using cinema_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.DTOs.PromotionDTOs
{
	public class PromotionDTO
	{
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime ExpiredDate { get; set; }
        public bool IsActive { get; set; }
        public PromotionDTO(Promotion promotion)
        {
            if (promotion == null)
                return;

            Id = promotion.Id;
            Code = promotion.Code;
            DiscountAmount = promotion.DiscountAmount;
            ExpiredDate = promotion.ExpiredDate;
            IsActive = promotion.IsActive;
        }
    }
}
