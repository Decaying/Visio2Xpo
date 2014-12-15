using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using cvo.buyshans.Visio2Xpo.UI.Messages;

namespace cvo.buyshans.Visio2Xpo.UI.ViewModels
{
    [Export]
    public class DisplayViewModel : PropertyChangedBase, IHandle<LoadMessage>, IHandle<SaveMessage>
    {
        private readonly IEventAggregator _EventAggregator;

        [ImportingConstructor]
        public DisplayViewModel(IEventAggregator eventAggregator)
        {
            _EventAggregator = eventAggregator;
            _EventAggregator.Subscribe(this);
        }

        #region "Handles"
        public void Handle(LoadMessage message)
        {
        }

        public void Handle(SaveMessage message)
        {
        }
        #endregion "Handles"
    }
}