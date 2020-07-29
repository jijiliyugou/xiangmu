using System;
using System.Collections.Generic;
using System.Text;

namespace Yaeher.Doctor
{
    /// <summary>
    /// 收藏病例夹
    /// </summary>
    public class CollectConsultation: EntityBaseModule
    {
        /// <summary>
        /// 医生ID
        /// </summary>
        public int DoctorID { get; set; }
        /// <summary>
        /// 咨询Id
        /// </summary>
        public int ConsultID { get; set; }
    }
}
