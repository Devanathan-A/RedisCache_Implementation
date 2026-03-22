namespace RedisCache_Implementation.Models
{
    public class GetInformationRequest
    {
        public string UserName { get; set; }
        public string EmailID { get; set; }
        public string MobileNumber { get; set; }
        public string Salary { get; set; }
        public string Gender { get; set; }
    }

    public class GetInformationResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<GetInformationRequest> data { get; set; }
    }
}
