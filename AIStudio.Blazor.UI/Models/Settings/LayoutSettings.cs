using AntDesign;

namespace AIStudio.Blazor.UI.Models.Settings
{
    public class LayoutSettings
    {
        /// <summary>
        /// 更新主题事件
        /// </summary>
        public event StateDelegate StateChanged;
        public delegate void StateDelegate(string name, object oldThemeName, object newThemeName);
        public LayoutSettings()
        {

        }

        private bool _multiTab;
        public bool MultiTab
        {
            get => _multiTab;
            set
            {
                if (_multiTab == value) return;
                StateChanged?.Invoke(nameof(MultiTab), _multiTab, value);
                _multiTab = value;
              
            }
        }

        public string Title { get; set; } = "AIStudio Blazor";
        /// <summary>
        /// 背景颜色
        /// </summary>
        public string Background { get; set; } = "background:#F8F8FF;";
        /// <summary>
        /// 头样式
        /// </summary>
        public string HeaderStyle { get; set; } = "padding:0;background:#F8F8FF;";
        /// <summary>
        /// 内容样式
        /// </summary>
        public string ContentStyle { get; set; } = "margin: 6px 16px;padding: 24px;min-height: 280px;background:#F8F8FF;";

        private bool _accordion;
        /// <summary>
        /// 手风琴模式
        /// </summary>
        public bool Accordion
        {
            get => _accordion;
            set
            {
                if (_accordion == value) return;
                StateChanged?.Invoke(nameof(Accordion), _accordion, value);
                _accordion = value;
            }
        }

        private string _theme = "ant-design-blazor.css";
        /// <summary>
        /// 主题
        /// </summary>
        public string Theme
        {
            get { return _theme; }
            set
            {
                if (_theme == value) return;
                ThemeSwitch(value);
                StateChanged?.Invoke(nameof(Theme), _theme, value);
                _theme = value;
            }
        }

        private void ThemeSwitch(string theme)
        {
            switch (theme)
            {
                case "ant-design-blazor.dark.css":
                    _leftMenuTheme = LeftMenuThemeEnum.Dark;
                    break;
                default:
                    break;
            }
        }

        private string _primaryColor;
        public string PrimaryColor
        {
            get => _primaryColor;
            set
            {
                if (_primaryColor == value) return;
                StateChanged?.Invoke(nameof(PrimaryColor), _primaryColor, value);
                _primaryColor = value;
               
            }
        }

        private LeftMenuThemeEnum _leftMenuTheme = LeftMenuThemeEnum.Dark;
        public LeftMenuThemeEnum LeftMenuTheme
        {
            get { return _leftMenuTheme; }
            set
            {
                if (_leftMenuTheme == value) return;
                _leftMenuTheme = value; // 先赋值，如果黑暗主题选择了明亮菜单，在回调里自动改回来
                ThemeSwitch(_theme);
                StateChanged?.Invoke(nameof(LeftMenuTheme),_theme, _theme);
            }
        }
        public SiderTheme SiderTheme
        {
            get
            {
                switch (LeftMenuTheme)
                {
                    case LeftMenuThemeEnum.Light:
                        SiderTheme.Light.Value = 1;
                        SiderTheme.Light.Name = "Light".ToLowerInvariant();
                        return SiderTheme.Light;
                    case LeftMenuThemeEnum.Dark:
                        SiderTheme.Dark.Value = 2;
                        SiderTheme.Dark.Name = "Dark".ToLowerInvariant();
                        return SiderTheme.Dark;
                    default:
                        return SiderTheme.Dark;

                }
            }
        }
        public MenuTheme MenuTheme
        {
            get
            {
                switch (LeftMenuTheme)
                {
                    case LeftMenuThemeEnum.Light:
                        MenuTheme.Light.Value = 1;
                        MenuTheme.Light.Name = "Light".ToLowerInvariant();
                        return MenuTheme.Light;
                    case LeftMenuThemeEnum.Dark:
                        MenuTheme.Dark.Value = 2;
                        MenuTheme.Dark.Name = "Dark".ToLowerInvariant();
                        return MenuTheme.Dark;
                    default:
                        return MenuTheme.Dark;

                }
            }
        }

        public enum LeftMenuThemeEnum
        {
            Light,
            Dark
        }
    }
}
