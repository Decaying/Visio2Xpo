using System;
using System.ComponentModel.Composition;
using System.Windows;
using Caliburn.Micro;
using cvo.buyshans.Visio2Xpo.UI.Messages;
using DevExpress.Xpf.Core;

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