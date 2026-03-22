using RedisCache_Implementation.Models;

namespace RedisCache_Implementation.DataAccessLayer
{
    public interface IRedisCacheOperationDL
    {
        public Task<AddInformationResponse> AddInformation(AddInformationRequest request);
        public Task<GetInformationResponse> GetInformation(GetInformationRequest request);  
        public Task<UpdateInformationResponse> UpdateInformation(UpdateInformationRequest request);
        public Task<DeleteInformationResponse> DeleteInformation(DeleteInformationRequest request);
        public Task<RefreshRecordTimeResponse> RefreshRecordTime();

    }
}
