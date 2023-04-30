using Microsoft.EntityFrameworkCore;
using VkNet.Model;
using VkPostReader.Models;

namespace VkPostReader
{
    public class DatabaseContext : DbContext
    {
        public DbSet<VkPostStatistics> VkStatisticRecords { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
    }
}
