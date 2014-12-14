using System;
using System.ComponentModel.Composition;
using System.Windows;
using Caliburn.Micro;
using cvo.buyshans.Visio2Xpo.UI.Messages;
using DevExpress.Xpf.Core;

namespace cvo.buyshans.Visio2Xpo.UI.ViewModels
{
    [Export]
    public class MainWindowViewModel : PropertyChangedBase, IHandle<ThemeChanged>
    {
        private String _Theme;
        private IEventAggregator _EventAggregator;

        public ActionViewModel Action { get; private set; }
        public DisplayViewModel Display { get; private set; }

        public String Theme
        {
            get
            {
                return _Theme;
            }
            private set
            {
                if (_Theme == value) return;
                _Theme = value;
                ThemeManager.SetThemeName(Application.Current.MainWindow, _Theme);
                NotifyOfPropertyChange();
            }
        }

        [ImportingConstructor]
        public MainWindowViewModel(ActionViewModel action, DisplayViewModel display, IEventAggregator eventAggregator)
        {
            Action = action;
            Display = display;
            _EventAggregator = eventAggregator;
            _EventAggregator.Subscribe(this);
        }

        public void Handle(ThemeChanged message)
        {
            Theme = message.ThemeName;
        }
    }
}