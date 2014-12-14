using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace cvo.buyshans.Visio2Xpo.UI.ViewModels
{
    [Export]
    public class MainWindowViewModel : PropertyChangedBase
    {
        public ActionViewModel Action { get; private set; }
        public DisplayViewModel Display { get; private set; }

        [ImportingConstructor]
        public MainWindowViewModel(ActionViewModel action, DisplayViewModel display)
        {
            Action = action;
            Display = display;
        }
    }
}