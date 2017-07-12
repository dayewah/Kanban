using Kanban.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanban
{
    public class Seed
    {
        public static void Run(bool addItems)
        {
            using (var db=new KanbanDbContext())
            {
                //db.Database.EnsureDeleted();
                //db.Database.EnsureCreated();
                db.Database.Migrate();
                if(addItems)
                {
                    db.Add(new TaskItem
                    {
                        Header = "Greeting",
                        Content = "Hello Daniel",
                        Created = DateTime.Now,
                        Status = Status.Backlog
                    });
                    db.Add(new TaskItem
                    {
                        Header = "1153530 Report",
                        Content = "Write report",
                        Created = DateTime.Now - TimeSpan.FromDays(10),
                        Status = Status.Backlog
                    });
                    db.Add(new TaskItem
                    {
                        Header = "1153016 Proposal",
                        Content = "Write proposal for outstanding tasks",
                        Created = DateTime.Now - TimeSpan.FromDays(50),
                        Status = Status.Doing
                    });
                    db.Add(new TaskItem
                    {
                        Header = "1102205 Write Paper",
                        Content = @"Project completed
- write paper outlining procedure.
- submit to fred for review.
- send to document control.
",
                        Created = DateTime.Now - TimeSpan.FromDays(100),
                        Status = Status.Done
                    });
                    db.SaveChanges();
                }
                
            }
        }
    }
}
