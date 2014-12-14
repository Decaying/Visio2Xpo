using System.ComponentModel.Composition;
using System.Windows;
using Caliburn.Micro;

namespace CaliburnTutorial.ViewModels
{
    [Export(typeof(MainWindowViewModel))]
    public class MainWindowViewModel : PropertyChangedBase
    {
        public TopViewModel Top { get; private set; }
        public BottomViewModel Bottom { get; private set; }

        [ImportingConstructor]
        public MainWindowViewModel(TopViewModel topViewModel, BottomViewModel bottomViewModel)
        {
            Top = topViewModel;
            Bottom = bottomViewModel;
        }
    }
}