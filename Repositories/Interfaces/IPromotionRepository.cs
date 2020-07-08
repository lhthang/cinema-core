using cinema_core.DTOs.PromotionDTOs;
using cinema_core.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Interfaces
{
	public interface IPromotionRepository
	{
		ICollection<PromotionDTO> GetPromotions(int skip, int limit);
		PromotionDTO GetPromotionById(int Id);
		PromotionDTO CreatePromotion(PromotionRequest promotionRequest);
		PromotionDTO UpdatePromotion(int id, PromotionRequest promotionRequest);
		bool DeletePromotion(int id);
	}
}
