using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanban.Model
{
    public class KanbanDbContext: DbContext
    {
        private string _path="Kanban.db";

        public KanbanDbContext()
        {
        }
        public DbSet<TaskItem> TaskItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=" + _path);
        }
    }
}
