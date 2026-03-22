using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using RedisCache_Implementation.DataAccessLayer;
using RedisCache_Implementation.Models;

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
            response.data = null;

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
