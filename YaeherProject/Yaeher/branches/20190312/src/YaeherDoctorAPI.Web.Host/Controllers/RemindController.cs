using Abp.Authorization;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yaeher;
using Yaeher.Common;
using Yaeher.Common.Constants;
using Yaeher.Remind;
using Yaeher.Remind.Dto;
using Yaeher.SystemManage;

namespace YaeherDoctorAPI.Web.Host.Controllers
{
    /// <summary>
    /// 患者成员管理API
    /// </summary>
    public class RemindController : YaeherAppServiceBase
    {
        private readonly IOrderTimeoutReminderService _OrderTimeoutReminderService;
        private readonly IOrderEarlyWarningService _OrderEarlyWarningService;
        private readonly IAbpSession _IabpSession;
        private readonly IYaeherOperListService _yaeherOperListService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="orderTimeoutReminderService"></param>
        /// <param name="orderEarlyWarningService"></param>
        /// <param name="session"></param>
        /// <param name="yaeherOperListService"></param>
        public RemindController(IOrderTimeoutReminderService orderTimeoutReminderService, 
                                IOrderEarlyWarningService orderEarlyWarningService, 
                                IAbpSession session,
                                IYaeherOperListService yaeherOperListService)
        {
            _OrderTimeoutReminderService = orderTimeoutReminderService;
            _OrderEarlyWarningService = orderEarlyWarningService;
            _IabpSession = session;
            _yaeherOperListService = yaeherOperListService;
        }
        /// <summary>
        /// 新增订单超时提醒记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/CreateOrderTimeoutReminder")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateOrderTimeoutReminder([FromBody] OrderTimeoutReminder input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var create = new OrderTimeoutReminder()
            {
                ConsultNumber = input.ConsultNumber,
                ConsultID = input.ConsultID,
                OrderID = input.OrderID,
                OrderNumber = input.OrderNumber,
                ConsultantID = input.ConsultantID,
                ConsultantName = input.ConsultantName,
                PatientID = input.PatientID,
                PatientName = input.PatientName,
                DoctorID = input.DoctorID,
                RemindDescription = input.RemindDescription,
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var res = await _OrderTimeoutReminderService.CreateOrderTimeoutReminder(create);
            if (res.Id > 0)
            {
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.Object = res;
            }
            else
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 400;
                this.ObjectResultModule.Message = "error!";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CreateOrderTimeoutReminder",
                OperContent = JsonHelper.ToJson(input),
                OperType = "CreateOrderTimeoutReminder",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取订单超时提醒记录Page
        /// </summary>
        /// <param name="OrderTimeoutReminderInPage"> OrderTimeoutReminderInPage 数据</param>
        /// <returns></returns>
        [Route("api/OrderTimeoutReminderPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> OrderTimeoutReminderPage([FromBody]OrderTimeoutReminderIn OrderTimeoutReminderInPage)
        {
            if (!Commons.CheckSecret(OrderTimeoutReminderInPage.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            OrderTimeoutReminderInPage.AndAlso(t => !t.IsDelete);
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _OrderTimeoutReminderService.OrderTimeoutReminderPage(OrderTimeoutReminderInPage);
            if (values.Items.Count == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new OrderTimeoutReminderOut(values, OrderTimeoutReminderInPage);
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "OrderTimeoutReminderPage",
                OperContent = JsonHelper.ToJson(OrderTimeoutReminderInPage),
                OperType = "OrderTimeoutReminderPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取订单超时提醒记录List 
        /// </summary>
        /// <param name="OrderTimeoutReminderList"> OrderTimeoutReminderList 数据</param>
        /// <returns></returns>
        [Route("api/OrderTimeoutReminderList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> OrderTimeoutReminderList([FromBody]OrderTimeoutReminderIn OrderTimeoutReminderList)
        {
            if (!Commons.CheckSecret(OrderTimeoutReminderList.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            OrderTimeoutReminderList.AndAlso(t => !t.IsDelete);
            var value = await _OrderTimeoutReminderService.OrderTimeoutReminderList(OrderTimeoutReminderList);
            if (value.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = value;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "OrderTimeoutReminderList",
                OperContent = JsonHelper.ToJson(OrderTimeoutReminderList),
                OperType = "OrderTimeoutReminderList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 更新订单超时提醒记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/UpdateOrderTimeoutReminder")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateOrderTimeoutReminder([FromBody] OrderTimeoutReminder input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _OrderTimeoutReminderService.OrderTimeoutReminderByID(input.Id);
            if (query != null)
            {
                query.ConsultNumber = input.ConsultNumber;
                query.ConsultID = input.ConsultID;
                query.OrderID = input.OrderID;
                query.OrderNumber = input.OrderNumber;
                query.ConsultantID = input.ConsultantID;
                query.ConsultantName = input.ConsultantName;
                query.PatientID = input.PatientID;
                query.PatientName = input.PatientName;
                query.DoctorID = input.DoctorID;
                query.RemindDescription = input.RemindDescription;
                query.ModifyOn = DateTime.Now;
                query.ModifyBy = userid;
                var res = await _OrderTimeoutReminderService.UpdateOrderTimeoutReminder(query);

                this.ObjectResultModule.Object = res;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            else
            {
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Object = "";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "UpdateOrderTimeoutReminder",
                OperContent = JsonHelper.ToJson(input),
                OperType = "UpdateOrderTimeoutReminder",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 删除订单超时提醒记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/DeleteOrderTimeoutReminder")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteOrderTimeoutReminder([FromBody] OrderTimeoutReminder input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _OrderTimeoutReminderService.OrderTimeoutReminderByID(input.Id);

            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _OrderTimeoutReminderService.DeleteOrderTimeoutReminder(query);

                this.ObjectResultModule.Object = res;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            else
            {
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Object = "";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DeleteOrderTimeoutReminder",
                OperContent = JsonHelper.ToJson(input),
                OperType = "DeleteOrderTimeoutReminder",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 新增订单超时提醒记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/CreateOrderEarlyWarning")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> CreateOrderEarlyWarning([FromBody] OrderEarlyWarning input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var create = new OrderEarlyWarning()
            {
                ConsultNumber = input.ConsultNumber,
                ConsultID = input.ConsultID,
                OrderID = input.OrderID,
                OrderNumber = input.OrderNumber,
                ConsultantID = input.ConsultantID,
                ConsultantName = input.ConsultantName,
                PatientID = input.PatientID,
                PatientName = input.PatientName,
                DoctorID = input.DoctorID,
                DoctorName = input.DoctorName,
                RemindState = input.RemindState,
                RemindDescription = input.RemindDescription,
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var res = await _OrderEarlyWarningService.CreateOrderEarlyWarning(create);
            if (res.Id > 0)
            {
                this.ObjectResultModule.StatusCode = 200;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.Object = res;
            }
            else
            {
                this.ObjectResultModule.Object = "";
                this.ObjectResultModule.StatusCode = 400;
                this.ObjectResultModule.Message = "error!";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "CreateOrderEarlyWarning",
                OperContent = JsonHelper.ToJson(input),
                OperType = "CreateOrderEarlyWarning",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取订单超时提醒记录Page
        /// </summary>
        /// <param name="OrderEarlyWarningInPage"> OrderTimeoutReminderInPage 数据</param>
        /// <returns></returns>
        [Route("api/OrderEarlyWarningPage")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> OrderEarlyWarningPage([FromBody]OrderEarlyWarningIn OrderEarlyWarningInPage)
        {
            if (!Commons.CheckSecret(OrderEarlyWarningInPage.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            OrderEarlyWarningInPage.AndAlso(t => !t.IsDelete);
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var values = await _OrderEarlyWarningService.OrderEarlyWarningPage(OrderEarlyWarningInPage);
            if (values.Items.Count == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = new OrderEarlyWarningOut(values, OrderEarlyWarningInPage);
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "OrderEarlyWarningPage",
                OperContent = JsonHelper.ToJson(OrderEarlyWarningInPage),
                OperType = "OrderEarlyWarningPage",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 获取订单超时提醒记录List 
        /// </summary>
        /// <param name="OrderEarlyWarningInList"> OrderEarlyWarningInList 数据</param>
        /// <returns></returns>
        [Route("api/OrderEarlyWarningList")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> OrderEarlyWarningList([FromBody]OrderEarlyWarningIn OrderEarlyWarningInList)
        {
            if (!Commons.CheckSecret(OrderEarlyWarningInList.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            OrderEarlyWarningInList.AndAlso(t => !t.IsDelete);
            var value = await _OrderEarlyWarningService.OrderEarlyWarningList(OrderEarlyWarningInList);
            if (value.Count() == 0)
            {
                this.ObjectResultModule.StatusCode = 204;
                this.ObjectResultModule.Message = "NoContent";
                this.ObjectResultModule.Object = "";
            }
            else
            {
                this.ObjectResultModule.Object = value;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "OrderEarlyWarningList",
                OperContent = JsonHelper.ToJson(OrderEarlyWarningInList),
                OperType = "OrderEarlyWarningList",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }

        /// <summary>
        /// 更新订单超时提醒记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/UpdateOrderEarlyWarning")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> UpdateOrderEarlyWarning([FromBody] OrderEarlyWarning input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _OrderEarlyWarningService.OrderEarlyWarningByID(input.Id);
            if (query != null)
            {
                query.ConsultNumber = input.ConsultNumber;
                query.ConsultID = input.ConsultID;
                query.OrderID = input.OrderID;
                query.OrderNumber = input.OrderNumber;
                query.ConsultantID = input.ConsultantID;
                query.ConsultantName = input.ConsultantName;
                query.PatientID = input.PatientID;
                query.PatientName = input.PatientName;
                query.DoctorID = input.DoctorID;
                query.DoctorName = input.DoctorName;
                query.RemindState = input.RemindState;
                query.RemindDescription = input.RemindDescription;
                query.ModifyOn = DateTime.Now;
                query.ModifyBy = userid;
                var res = await _OrderEarlyWarningService.UpdateOrderEarlyWarning(query);

                this.ObjectResultModule.Object = res;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            else
            {
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Object = "";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "UpdateOrderEarlyWarning",
                OperContent = JsonHelper.ToJson(input),
                OperType = "UpdateOrderEarlyWarning",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
        /// <summary>
        /// 删除订单超时提醒记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("api/DeleteOrderEarlyWarning")]
        [HttpPost]
        [AbpAuthorize]
        public async Task<ObjectResultModule> DeleteOrderEarlyWarning([FromBody] OrderEarlyWarning input)
        {
            if (!Commons.CheckSecret(input.Secret))
            {
                this.ObjectResultModule.StatusCode = 422;
                this.ObjectResultModule.Message = "Wrong Secret";
                this.ObjectResultModule.Object = "";
                return this.ObjectResultModule;
            }
            var userid = _IabpSession.UserId > 0 ? (int)_IabpSession.UserId : 0;
            var query = await _OrderEarlyWarningService.OrderEarlyWarningByID(input.Id);

            if (query != null)
            {
                query.DeleteBy = userid;
                query.DeleteTime = DateTime.Now;
                query.IsDelete = true;
                var res = await _OrderEarlyWarningService.DeleteOrderEarlyWarning(query);

                this.ObjectResultModule.Object = res;
                this.ObjectResultModule.Message = "sucess";
                this.ObjectResultModule.StatusCode = 200;
            }
            else
            {
                this.ObjectResultModule.Message = "NotFound";
                this.ObjectResultModule.StatusCode = 404;
                this.ObjectResultModule.Object = "";
            }
            #region 操作日志
            var CreateYaeherOperList = new YaeherOperList()
            {
                OperExplain = "DeleteOrderEarlyWarning",
                OperContent = JsonHelper.ToJson(input),
                OperType = "DeleteOrderEarlyWarning",
                CreatedBy = userid,
                CreatedOn = DateTime.Now
            };
            var resultLog = await _yaeherOperListService.CreateYaeherOperList(CreateYaeherOperList);
            #endregion

            return this.ObjectResultModule;
        }
    }
}
