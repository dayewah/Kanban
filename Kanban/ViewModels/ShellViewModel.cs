using GongSolutions.Wpf.DragDrop;
using Kanban.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Kanban.ViewModels
{
    public class ShellViewModel : BindableBase//, IDropTarget
    {
        private TaskItemRepository _repository;

        public ShellViewModel(TaskItemRepository repository)
        {
            _repository = repository;
            LoadTaskItems();
            //PropertyChanged += ShellViewModel_PropertyChanged;
            Backlog.CollectionChanged += CollectionChanged;
            Ready.CollectionChanged += CollectionChanged;
            Doing.CollectionChanged += CollectionChanged;
            Done.CollectionChanged += CollectionChanged;
        }

        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action==NotifyCollectionChangedAction.Add)
            {
                //var col = (ObservableCollection<TaskItem>)sender;
                //var name = nameof(sender);
                Backlog.ToList().ForEach(t => t.Status = Status.Backlog);
                Ready.ToList().ForEach(t => t.Status = Status.Ready);
                Doing.ToList().ForEach(t => t.Status = Status.Doing);
                Done.ToList().ForEach(t => t.Status = Status.Done);
                //col.ToList
            }
        }

        //private void ShellViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "SelectedItem")
        //    {
        //        Save();
        //    }
        //}

        private void LoadTaskItems()
        {
            var b = _repository.GetByStatus(Status.Backlog);
            Backlog.AddRange(_repository.GetByStatus(Status.Backlog).OrderByDescending(t=>t.Created));
            Ready.AddRange(_repository.GetByStatus(Status.Ready).OrderByDescending(t => t.Created));
            Doing.AddRange(_repository.GetByStatus(Status.Doing).OrderByDescending(t => t.Created));
            Done.AddRange(_repository.GetByStatus(Status.Done).OrderByDescending(t => t.Created));

            foreach (var item in AllTasks)
            {
                item.PropertyChanged += Item_PropertyChanged;
            }
        }

        private void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Id")
            {
                HasChanges = true;
            }
        }

        public ObservableCollection<TaskItem> Backlog { get; set; } = new ObservableCollection<TaskItem>();
        public ObservableCollection<TaskItem> Ready { get; set; } = new ObservableCollection<TaskItem>();
        public ObservableCollection<TaskItem> Doing { get; set; } = new ObservableCollection<TaskItem>();
        public ObservableCollection<TaskItem> Done { get; set; } = new ObservableCollection<TaskItem>();
        public IEnumerable<TaskItem> AllTasks { get { return Backlog.Concat(Ready).Concat(Doing).Concat(Done); } }

        private TaskItem _selectedItem;
        public TaskItem SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }
        public DelegateCommand AddCommand =>new DelegateCommand(Add);

        private void Add()
        {
            var t = new TaskItem { Header = "New", Created = DateTime.Now };
            Backlog.Insert(0,t);
            t.PropertyChanged += Item_PropertyChanged;
        }

        public DelegateCommand DeleteCommand => new DelegateCommand(Delete);

        private void Delete()
        {
            var item = SelectedItem;
            if (item == null) return;

            item.PropertyChanged -= Item_PropertyChanged;
            switch (item.Status)
            {
                case Status.Backlog:
                    Backlog.Remove(item);
                    break;
                case Status.Ready:
                    Ready.Remove(item);
                    break;
                case Status.Doing:
                    Doing.Remove(item);
                    break;
                case Status.Done:
                    Done.Remove(item);
                    break;
                default:
                    break;
            }
            _repository.Delete(item);
        }

        public DelegateCommand SaveCommand => new DelegateCommand(Save);

        public bool HasChanges { get; internal set; }

        private void Save()
        {
            _repository.SaveAll(AllTasks);

            Backlog.Clear();
            Ready.Clear();
            Doing.Clear();
            Done.Clear();

            LoadTaskItems();
            HasChanges = false;
        }

        //public void DragOver(IDropInfo dropInfo)
        //{
        //    dropInfo.Effects = System.Windows.DragDropEffects.Move;
        //}

        //public void Drop(IDropInfo dropInfo)
        //{
        //    var a=dropInfo.Data;
        //}
    }
}
