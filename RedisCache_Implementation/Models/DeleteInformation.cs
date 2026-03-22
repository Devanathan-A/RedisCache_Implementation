namespace RedisCache_Implementation.Models
{
    public class DeleteInformationRequest
    {
        public int UserID { get; set; }
    }

    public class DeleteInformationResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

}
