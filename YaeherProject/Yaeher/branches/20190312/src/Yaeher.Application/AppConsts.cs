using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;

namespace Yaeher
{
    public interface Idepends<TEntity> : IRepository<TEntity, string>, IRepository, ITransientDependency where TEntity : class, IEntity<string>
    {
    }

    public class AppConsts
    {
        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public const string DefaultPassPhrase = "gsKxGZ012HLL3MI5";
    }
}
