namespace CouponWeb.Models
{
    public class RequestDto
    {
        public Utility.SD.ApiType ApiType { get; set; } = Utility.SD.ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string AccessToken { get; set; }
    }
}
