using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace CaliburnTutorial.ViewModels
{
    [Export(typeof(TopViewModel))]
    public class TopViewModel : PropertyChangedBase
    {
        private IEventAggregator _EventAggregator;

        [ImportingConstructor]
        public TopViewModel(IEventAggregator eventAggregator)
        {
            _EventAggregator = eventAggregator;
        }

        public void SendMessage()
        {
            _EventAggregator.PublishOnUIThread(new Messages.Message("Hello from the top!"));
        }
    }
}