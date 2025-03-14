using Microsoft.EntityFrameworkCore;
using Models.DTO;

namespace Models
{
    public class DBManagement : DbContext
    {
        public DBManagement(DbContextOptions<DBManagement> options) : base(options)
        {
            /*
             * Creacion Base de Datos
             */
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<TaskModels> Tasks { get; set; }

    }
}
