using Abp.Application.Services.Dto;


namespace Yaeher.Remind.Dto
{
    /// <summary>
    /// 订单预警
    /// </summary>
    public class OrderEarlyWarningIn : ListParameters<OrderEarlyWarning>, IPagedResultRequest
    {

    }
    
}
