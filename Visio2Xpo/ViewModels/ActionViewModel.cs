using System;
using System.ComponentModel.Composition;
using System.Windows;
using Caliburn.Micro;
using cvo.buyshans.Visio2Xpo.UI.Messages;

namespace cvo.buyshans.Visio2Xpo.UI.ViewModels
{
    [Export]
    public class ActionViewModel : PropertyChangedBase, IHandle<LoadMessage>, IHandle<SaveMessage>
    {
        private Boolean _CanSave;
        private Boolean _CanLoad = true;

        private readonly IEventAggregator _EventAggregator;

        [ImportingConstructor]
        public ActionViewModel(IEventAggregator eventAggregator)
        {
            _EventAggregator = eventAggregator;

            _EventAggregator.Subscribe(this);
        }

        #region "UI Properties"
        public Boolean CanSave
        {
            get
            {
                return _CanSave;
            }
            private set
            {
                if (_CanSave == value) return;
                _CanSave = value;
                NotifyOfPropertyChange();
            }
        }
        
        public Boolean CanLoad
        {
            get
            {
                return _CanLoad;
            }
            private set
            {
                if (_CanLoad != value)
                {
                    _CanLoad = value;
                    NotifyOfPropertyChange();
                }
            }
        }
        #endregion "UI Properties"
        #region "UI Actions"
        public void Save()
        {
            _EventAggregator.PublishOnUIThread(new SaveMessage());
        }

        public void Load()
        {
            _EventAggregator.PublishOnUIThread(new LoadMessage());
        }
        #endregion "UI Actions"

        public void Handle(LoadMessage message)
        {
            MessageBox.Show("Loading....");
            CanSave = true;
            CanLoad = false;
        }

        public void Handle(SaveMessage message)
        {
            MessageBox.Show("Saving....");
            CanSave = false;
            CanLoad = true;
        }
    }
}