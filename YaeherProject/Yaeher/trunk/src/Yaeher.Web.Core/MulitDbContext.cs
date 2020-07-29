using Abp;
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.Modules;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Yaeher.Configuration;
using Yaeher.Web;

namespace Yaeher
{
    public class MulitDbContext: DependsOnAttribute
    {
        public MulitDbContext()
        {
            for  (int i=0;i< DependedModuleTypes.Length;i++ )
            {
                if (DependedModuleTypes[i].ToString() == "YaeherDoctorAPIEntityFrameworkModule")
                { DependedModuleTypes[i] = null; }
            }
        }

    }
}
