using System;

namespace cvo.buyshans.Visio2Xpo.UI.Messages
{
    public class ThemeChanged
    {
        public String ThemeName { get; private set; }

        public ThemeChanged(String themeName)
        {
            ThemeName = themeName;
        }
    }
}