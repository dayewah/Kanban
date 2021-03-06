﻿using GongSolutions.Wpf.DragDrop;
using Kanban.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;

namespace Kanban.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private TaskItemRepository _repository;

        public ShellViewModel(TaskItemRepository repository)
        {
            _repository = repository;
            LoadTaskItems();

            Backlog.CollectionChanged += CollectionChanged;
            Ready.CollectionChanged += CollectionChanged;
            Doing.CollectionChanged += CollectionChanged;
            Done.CollectionChanged += CollectionChanged;

            var watcher = new FileSystemWatcher();
            watcher.Path = Environment.CurrentDirectory;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*Kanban.db";
            watcher.Changed += Watcher_Changed;
            watcher.EnableRaisingEvents = true;
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (_collectionChanging == true) return;
            Refresh();
        }

        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _collectionChanging = true;
            if (e.Action==NotifyCollectionChangedAction.Add)
            {
                
                Backlog.ToList().ForEach(t => t.Status = Status.Backlog);
                Ready.ToList().ForEach(t => t.Status = Status.Ready);
                Doing.ToList().ForEach(t => t.Status = Status.Doing);
                Done.ToList().ForEach(t => t.Status = Status.Done);
            }
            _collectionChanging = false;
        }

        

        private void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //if (_collectionChanging == true) return;

            if (e.PropertyName != "IsSelected")
            {
                HasChanges = true;
                _repository.Save(sender as TaskItem);
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
            set
            {
                if(_selectedItem!=null) _selectedItem.IsSelected = false;
                SetProperty(ref _selectedItem, value);
                if(_selectedItem!=null) _selectedItem.IsSelected = true;
            }
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

        public DelegateCommand SaveCommand => new DelegateCommand(Save).ObservesCanExecute(()=>HasChanges);

        private bool _hasChanges;
        private bool _collectionChanging;

        public bool HasChanges
        {
            get { return _hasChanges; }
            set { SetProperty(ref _hasChanges, value); }
        }

        private void Save()
        {
            _repository.SaveAll(AllTasks);

            //LoadTaskItems();
            HasChanges = false;
        }

        private void LoadTaskItems()
        {
            Backlog.AddRange(_repository.GetByStatus(Status.Backlog));//.OrderByDescending(t=>t.Created));
            Ready.AddRange(_repository.GetByStatus(Status.Ready));//.OrderByDescending(t => t.Created));
            Doing.AddRange(_repository.GetByStatus(Status.Doing));//.OrderByDescending(t => t.Created));
            Done.AddRange(_repository.GetByStatus(Status.Done));//.OrderByDescending(t => t.Created));

            foreach (var item in AllTasks)
            {
                item.PropertyChanged += Item_PropertyChanged;
            }
        }

        private void ClearCollections()
        {
            AllTasks.ToList().ForEach(t => t.PropertyChanged -= Item_PropertyChanged);

            Backlog.Clear();
            Ready.Clear();
            Doing.Clear();
            Done.Clear();
        }

        private void Refresh()
        {
            App.Current.Dispatcher.Invoke(() =>//to modify obervable collection on dispatcher thread
            {
                ClearCollections();
                LoadTaskItems();
            });
        }
    }
}
