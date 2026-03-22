using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RedisCache_Implementation.DataAccessLayer;
using RedisCache_Implementation.Models;
using System.Text;

namespace RedisCache_Implementation.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class RedisCacheOperationController : ControllerBase
    {
        private readonly IRedisCacheOperationDL _redisCacheOperationDL;
        private readonly IDistributedCache _distributedCache;
        string RedisCacheKey = "UserInformation";

        public RedisCacheOperationController(IRedisCacheOperationDL redisCacheOperationDL,IDistributedCache distributedCache)
        {
            _redisCacheOperationDL = redisCacheOperationDL;
            _distributedCache = distributedCache;
        }

        [HttpGet]
        public async Task<IActionResult> GetInformation()
        {
            GetInformationResponse response = new GetInformationResponse();

            response.IsSuccess = true;
            response.Message = "Data Fetched Successfully";

            try
            {
                string SerializeList = string.Empty;
                var EncodedList = await _distributedCache.GetAsync(RedisCacheKey);
                if (EncodedList != null)
                {
                    await _distributedCache.RemoveAsync(RedisCacheKey);
                    response.data = new List<GetInformation>();
                    SerializeList = Encoding.UTF8.GetString(EncodedList);
                    response.data = JsonConvert.DeserializeObject<List<GetInformation>>(SerializeList);
                }
                else
                {
                    response = await _redisCacheOperationDL.GetInformation();
                    if (response.IsSuccess)
                    {
                        SerializeList = JsonConvert.SerializeObject(response.data);
                        EncodedList = Encoding.UTF8.GetBytes(SerializeList);

                        var Option = new DistributedCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(40))
                            .SetAbsoluteExpiration(DateTime.Now.AddHours(6));

                        await _distributedCache.SetAsync(RedisCacheKey, EncodedList, Option);

                    }
                }
            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return Ok(response);

        }

        [HttpPost]
        public async Task<IActionResult> AddInformation(AddInformationRequest request)
        {
            AddInformationResponse response = new AddInformationResponse();
            try
            { 
                await _distributedCache.RemoveAsync(RedisCacheKey);
                response = await _redisCacheOperationDL.AddInformation(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return Ok(response);

        }

        [HttpPatch]
        public async Task<IActionResult> UpdateInformation()
        {
            GetInformationResponse response = new GetInformationResponse();

            response.IsSuccess = true;
            response.Message = "Data Fetched Successfully";
            response.data = null;

            return Ok(response);

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteInformation()
        {
            GetInformationResponse response = new GetInformationResponse();

            response.IsSuccess = true;
            response.Message = "Data Fetched Successfully";
            response.data = null;

            return Ok(response);

        }

        [HttpGet]
        public async Task<IActionResult> RefreshRecordTime()
        {
            GetInformationResponse response = new GetInformationResponse();

            response.IsSuccess = true;
            response.Message = "Data Fetched Successfully";
            response.data = null;

            return Ok(response);

        }
    }
}
