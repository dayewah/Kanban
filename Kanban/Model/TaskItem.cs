using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanban.Model
{
    public class TaskItem:BindableBase
    {
        public int Id { get; set; }

        private string _header;
        public string Header
        {
            get { return _header; }
            set { SetProperty(ref _header, value); }
        }
        private string _content;
        public string Content
        {
            get { return _content; }
            set { SetProperty(ref _content, value); }
        }

        private DateTime _created;
        public DateTime Created
        {
            get { return _created; }
            set { SetProperty(ref _created, value); }
        }

        private DateTime _dueDate;
        public DateTime DueDate
        {
            get { return _dueDate; }
            set { SetProperty(ref _dueDate, value); }
        }

        private Status _status;
        public Status Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        private bool _isArchived;
        public bool IsArchived
        {
            get { return _isArchived; }
            set { SetProperty(ref _isArchived, value); }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }
    }
}
