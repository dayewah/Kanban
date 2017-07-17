using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanban.Model
{
    public class TaskItemRepository
    {
        public IEnumerable<TaskItem> GetAll()
        {
            using (var context = new KanbanDbContext())
            {
                return context.TaskItems.ToList();
            }
        }

        public IEnumerable<TaskItem> GetByStatus(Status status)
        {
            using (var context = new KanbanDbContext())
            {
                return context.TaskItems.Where(t=>t.Status == status).ToList();
            }
        }

        public void Save(TaskItem item)
        {
            using (var context=new KanbanDbContext())
            {
                SaveItem(context, item);
                context.SaveChanges();
            }
        }

        public void SaveAll(IEnumerable<TaskItem> items)
        {
            using (var context = new KanbanDbContext())
            {
                foreach (var item in items)
                {
                    SaveItem(context, item);
                }
                context.SaveChanges();
            }
        }

        public void Delete(TaskItem item)
        {
            if (item == null) return;

            using (var context=new KanbanDbContext())
            {
                var trackedItem = context.TaskItems.Find(item.Id);
                if (trackedItem!=null)
                {
                    context.TaskItems.Remove(trackedItem);
                    context.SaveChanges();

                }
            }
        }

        private void SaveItem(KanbanDbContext context,TaskItem item)
        {
            if (context.TaskItems.Any(e => e.Id == item.Id))
            {
                context.TaskItems.Attach(item);
                context.Entry(item).State = EntityState.Modified;
            }
            else
            {
                context.TaskItems.Add(item);
            }
        }
    }
}
