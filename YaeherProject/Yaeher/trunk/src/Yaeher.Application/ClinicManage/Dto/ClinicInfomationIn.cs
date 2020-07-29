using Abp.Application.Services.Dto;

namespace Yaeher.ClinicManage.Dto
{
    /// <summary>
    /// 门诊信息
    /// </summary>
    public class ClinicInfomationIn : ListParameters<ClinicInfomation>, IPagedResultRequest
    {
        /// <summary>
        /// ClinicName
        /// </summary>
        public string ClinicName { get; set; }
        /// <summary>
        /// 门诊类型 1成人,2儿科
        /// </summary>
        public int ClinicType { get; set; }
        /// <summary>
        /// 医生ID
        /// </summary>
        public int DoctorId { get; set; }
        /// <summary>
        /// 科室IDstring集合
        /// </summary>
        public string IDArr { get; set;}
    }
    /// <summary>
    /// 科室与医生关系  科室与标签关系
    /// </summary>
    public class ClinicDoctorIn : ListParameters<ClinicInfomation>, IPagedResultRequest
    {
        /// <summary>
        /// 门诊ID
        /// </summary>
        public virtual int ClinicID { get; set; }
        /// <summary>
        /// 门诊名称
        /// </summary>
        public virtual string ClinicName { get; set; }
        /// <summary>
        /// 门诊JSON
        /// </summary>
        public virtual string ClinicJSON { get; set; }
        /// <summary>
        /// 医生ID
        /// </summary>
        public virtual int DoctorID { get; set; }
        /// <summary>
        /// 医生姓名
        /// </summary>
        public virtual string DoctorName { get; set; }
        /// <summary>
        /// 医生JSON
        /// </summary>
        public virtual string DoctorJSON { get; set; }
        /// <summary>
        /// 医生IDJSON
        /// </summary>
        public virtual string DoctorIDJSON { get; set; }
    }
    /// <summary>
    /// 科室与标签关系
    /// </summary>
    public class ClinicLableIn : ListParameters<ClinicInfomation>, IPagedResultRequest
    {
        /// <summary>
        /// 门诊ID
        /// </summary>
        public virtual int ClinicID { get; set; }
        /// <summary>
        /// 门诊名称
        /// </summary>
        public virtual string ClinicName { get; set; }
        /// <summary>
        /// 门诊JSON
        /// </summary>
        public virtual string ClinicJSON { get; set; }
        /// <summary>
        /// 标签ID
        /// </summary>
        public virtual int LableID { get; set; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public virtual string LableName { get; set; }
        /// <summary>
        /// 标签JSon
        /// </summary>
        public virtual string LableJSON { get; set; }
        /// <summary>
        /// 标签IDJSon
        /// </summary>
        public virtual string LableIDJSON { get; set; }
    }
}
