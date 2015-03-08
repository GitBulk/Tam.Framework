using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tam.Repository
{
    public interface IConnectionStringRepository
    {
        /// <summary>
        /// Get Entity Framework connection string
        /// </summary>
        /// <returns>An Entity Framework connection string</returns>
        string GetEFConnectionString();

        /// <summary>
        // Get SQL Server connection string
        /// </summary>
        /// <returns>A SQL Server connection string</returns>
        string GetSqlConnectionString();
    }
}
