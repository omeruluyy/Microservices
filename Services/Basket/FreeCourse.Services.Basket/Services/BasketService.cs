using FreeCourse.Services.Basket.Dtos;
using FreeCourse.Shared.Dtos;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Services
{
    public class BasketService : IBasketService
    {

        private readonly RedisService _redisService;

        public BasketService(RedisService redisService)
        {
            _redisService = redisService;
        }
        public async Task<Response<bool>> DeleteBasket(string userId)
        {
            var status =await _redisService.GetDb().KeyDeleteAsync(userId);
            return status ? Response<bool>.Success(204) : Response<bool>.Fail("basket not found", 404);
        }

        public async Task<Response<BasketDto>> GetBasket(string userId)
        {
            var existBasket= await _redisService.GetDb().StringGetAsync(userId);
            Console.WriteLine("exist basket : " + existBasket);
            if (String.IsNullOrEmpty(existBasket)){
                return Response<BasketDto>.Fail("basket not found",404);
            }

            return Response<BasketDto>.Success(JsonSerializer.Deserialize<BasketDto>(existBasket),200);
        }

        public async Task<Response<bool>> SaveOrUpdate(BasketDto basketDto)
        {
            var status =await _redisService.GetDb().StringSetAsync(basketDto.UserId, JsonSerializer.Serialize(basketDto));
            return status ? Response<bool>.Success(204) : Response<bool>.Fail("basket was not saved", 500);
        }

      
    }
}
