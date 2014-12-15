using System.ComponentModel.Composition;
using System.Windows;
using Caliburn.Micro;
using cvo.buyshans.Visio2Xpo.UI.Messages;
using DevExpress.Xpf.Core;

namespace cvo.buyshans.Visio2Xpo.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindowView.xaml
    /// </summary>
    public partial class MainWindowView : IHandle<ThemeChanged>
    {
        [Import]
        private IEventAggregator _EventAggregator;

        public MainWindowView()
        {
            InitializeComponent();
        }
        public void Handle(ThemeChanged message)
        {
            ThemeManager.SetThemeName(Application.Current.MainWindow, message.ThemeName);
        }
    }
}
