using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using Message = CaliburnTutorial.Messages.Message;

namespace CaliburnTutorial.ViewModels
{
    [Export(typeof(BottomViewModel))]
    public class BottomViewModel : PropertyChangedBase, IHandle<Message>
    {
        private IEventAggregator _EventAggregator;

        private String _LabelContent;
        public String LabelContent
        {
            get
            {
                return _LabelContent;
            }
            set
            {
                if (value == _LabelContent) return;
                _LabelContent = value;
                NotifyOfPropertyChange();
            }
        }

        [ImportingConstructor]
        public BottomViewModel(IEventAggregator eventAggregator)
        {
            _EventAggregator = eventAggregator;

            _EventAggregator.Subscribe(this);
        }

        public void Handle(Message message)
        {
            LabelContent = message.MessageText;
        }
    }
}