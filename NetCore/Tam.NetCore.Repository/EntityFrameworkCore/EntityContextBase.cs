using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tam.NetCore.Repository.EntityFrameworkCore
{
    public class EntityContextBase<TContext> : DbContext where TContext : DbContext
    {
        public EntityContextBase() : base()
        { }

        public EntityContextBase(DbContextOptions<TContext> options) : base(options)
        {

        }
    }
}
