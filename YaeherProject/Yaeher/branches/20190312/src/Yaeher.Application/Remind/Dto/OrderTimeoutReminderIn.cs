using Abp.Application.Services.Dto;


namespace Yaeher.Remind.Dto
{
    /// <summary>
    /// 超时提醒
    /// </summary>
    public class OrderTimeoutReminderIn : ListParameters<OrderTimeoutReminder>, IPagedResultRequest
    {

    }
  
}
