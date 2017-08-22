using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kanban.Properties;
using Prism.Events;

namespace Kanban.ViewModels
{
    public class SettingsViewModel: BindableBase
    {
        private IEventAggregator _eventAggregator;
        private Settings _settings;

        public SettingsViewModel()
        {
            _settings = Settings.Default;

            if (string.IsNullOrEmpty(_settings.DbPath))
                _settings.DbPath = Environment.CurrentDirectory;
        }

        public SettingsViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _settings =Settings.Default;

            if (string.IsNullOrEmpty(_settings.DbPath))
                _settings.DbPath = Environment.CurrentDirectory;
        }

        private string _dbPath;

        public string DbPath
        {
            get { return _settings.DbPath; }
            set
            {
                if (value != _settings.DbPath)
                {
                    _settings.DbPath = value;
                    RaisePropertyChanged(nameof(DbPath));
                }
            }
        }
        

    }
}
