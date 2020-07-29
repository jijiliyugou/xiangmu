using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Yaeher
{
    /// <summary>
    /// QueryableToPage
    /// </summary>
    public class QueryableToPage
    {
        /// <summary>
        /// 异步 获取列表 带分页
        /// </summary>
        /// <param name="query">IQueryable</param>
        /// <param name="pageable">条件</param>
        /// <returns></returns>
        public async Task<Pageable<EntityBaseModule>> QueryableToPageAsync(IQueryable<EntityBaseModule> query, Pageable<EntityBaseModule> pageable)
        {
            var expr = pageable.Expression;
            if (pageable.Includes != null)
            {
                foreach (var includeExpr in pageable.Includes)
                {
                    query = query.Include(includeExpr);
                }
            }
            if (expr != null)
            {
                query = query.Where(expr);
            }

            long count = 0;
            if (pageable.PageSize != 0)
            {
                if (expr != null)
                {
                    //query = this.DbSet.Where(expr);
                }
                else
                {
                    //query = this.DbSet;
                }
                count = await query.LongCountAsync();
                pageable.RecordCount = count;
                if (count == 0)
                {
                    pageable.Items = new List<EntityBaseModule>();
                    return pageable;
                }
            }
            //query = DbSet;
            if (pageable.Asc != null)
            {
                query = query.OrderBy(pageable.Asc);
            }
            else if (pageable.Desc != null)
            {
                query = query.OrderByDescending(pageable.Desc);
            }

            if (pageable.PageSize > 0)
            {
                int pageSize = (int)pageable.PageSize;
                var pageCount = count / pageSize;
                if (count % pageSize > 0) pageCount++;
                pageable.PageCount = pageCount;
                pageable.PageCounts = count;
                var pageIndex = pageable.PageIndex;
                if (pageIndex == 0) pageIndex = pageable.PageIndex = 1;
                if (pageIndex > pageCount) pageIndex = pageable.PageIndex = pageCount;
                int skip = (int)((pageIndex - 1) * pageSize);
                query = query.Skip(skip).Take(pageSize);
            }

            pageable.Items = await query.ToListAsync();
            return pageable;
        }

    }

}
