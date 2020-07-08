using cinema_core.DTOs.PromotionDTOs;
using cinema_core.ErrorHandle;
using cinema_core.Form;
using cinema_core.Models;
using cinema_core.Models.Base;
using cinema_core.Repositories.Base;
using cinema_core.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Implements
{
	public class PromotionRepository : BaseRepository,IPromotionRepository
	{
		public PromotionRepository(MyDbContext context) : base(context)
		{
		}
		public void AutoUpdatePromotion()
		{
			DateTime date = DateTime.Now;
			var promotions = dbContext.Promotions.Where(p => p.ExpiredDate.CompareTo(date) <= 0 && p.IsActive == true).ToList();

			foreach (var promotion in promotions)
			{
				promotion.IsActive = false;
				dbContext.Update(promotion);
			}
			Save();
		}

		public PromotionDTO CreatePromotion(PromotionRequest promotionRequest)
		{
			var isExist = dbContext.Promotions.Where(p => p.Code == promotionRequest.Code && p.IsActive == true).FirstOrDefault();
			if (isExist != null) throw new CustomException(HttpStatusCode.BadRequest, "This promotion is active");
			Promotion promotion = new Promotion()
			{
				Code = promotionRequest.Code,
				DiscountAmount = promotionRequest.DiscountAmount,
				ExpiredDate = promotionRequest.ExpiredDate,
				IsActive = promotionRequest.IsActive,
			};
			dbContext.Add(promotion);
			var isSuccess = dbContext.SaveChanges();
			if (isSuccess<=0) throw new CustomException(HttpStatusCode.BadRequest, "Something went wrong when save this promotion");
			return new PromotionDTO(promotion);
		}

		public PromotionDTO UpdatePromotion(int id, PromotionRequest promotionRequest)
		{
			var isExist = dbContext.Promotions.Where(p => p.Code == promotionRequest.Code && p.IsActive == true).FirstOrDefault();
			if (isExist != null) throw new CustomException(HttpStatusCode.BadRequest, "This promotion is active");

			var promotion = dbContext.Promotions.FirstOrDefault(x => x.Id == id);
			if (promotion == null)
				throw new CustomException(HttpStatusCode.BadRequest, "Promotion not found");

			promotion.Code = promotionRequest.Code;
			promotion.DiscountAmount = promotionRequest.DiscountAmount;
			promotion.ExpiredDate = promotionRequest.ExpiredDate;
			promotion.IsActive = promotionRequest.IsActive;
			Save();

			return new PromotionDTO(promotion);
		}

		public bool DeletePromotion(int id)
		{
			var promotion = dbContext.Promotions.Where(p => p.Id == id).FirstOrDefault();
			if (promotion==null) throw new CustomException(HttpStatusCode.NotFound, "Not found");
			dbContext.Remove(promotion);
			return dbContext.SaveChanges() > 0;
		}

		public PromotionDTO GetPromotionById(int Id)
		{
			var promotion = dbContext.Promotions.Where(p => p.Id == Id).FirstOrDefault();
			if (promotion == null) throw new CustomException(HttpStatusCode.NotFound, "Not found");
			return new PromotionDTO(promotion);
		}

		public PromotionDTO CheckPromotion(string promotionCode)
		{
			var promotionFound = dbContext.Promotions.FirstOrDefault(p => p.Code == promotionCode && p.IsActive == true);
			if (promotionFound == null)
			{
				return null;
			}

			return new PromotionDTO(promotionFound);
		}

		public ICollection<PromotionDTO> GetPromotions(int skip, int limit)
		{
			List<PromotionDTO> promotionDTOs = new List<PromotionDTO>();
			var promotions = dbContext.Promotions.Skip(skip).Take(limit).ToList();
			foreach(var promotion in promotions)
			{
				promotionDTOs.Add(new PromotionDTO(promotion));
			}
			return promotionDTOs;
		}
	}
}
