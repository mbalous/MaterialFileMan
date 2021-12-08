using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace FileManager.Controls
{
    public class SystemMenuWindow : Window
    {
        public static readonly DependencyProperty MenuItemsProperty = DependencyProperty.Register(
            nameof(MenuItems), typeof(FreezableCollection<SystemMenuItem>), typeof(SystemMenuWindow), new PropertyMetadata(new PropertyChangedCallback(OnMenuItemsChanged)));

        public FreezableCollection<SystemMenuItem> MenuItems
        {
            get { return (FreezableCollection<SystemMenuItem>)this.GetValue(MenuItemsProperty); }

            set { this.SetValue(MenuItemsProperty, value); }
        }

        private IntPtr systemMenu;

        /// <summary>
        /// Initializes a new instance of the SystemMenuWindow class.
        /// </summary>
        public SystemMenuWindow()
        {
            this.Loaded += this.SystemMenuWindow_Loaded;

            this.MenuItems = new FreezableCollection<SystemMenuItem>();
        }

        private static void OnMenuItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SystemMenuWindow obj = d as SystemMenuWindow;

            if (obj != null)
            {
                if (e.NewValue != null)
                {
                    obj.MenuItems = e.NewValue as FreezableCollection<SystemMenuItem>;
                }
            }
        }

        private void SystemMenuWindow_Loaded(object sender, RoutedEventArgs e)
        {
            WindowInteropHelper interopHelper = new WindowInteropHelper(this);
            this.systemMenu = Native.GetSystemMenu(interopHelper.Handle, false);

            if (this.MenuItems.Count > 0)
            {
                Native.InsertMenuW(this.systemMenu, -1, Native.MF_BYPOSITION | Native.MF_SEPARATOR, 0, String.Empty);
            }

            foreach (SystemMenuItem item in this.MenuItems)
            {
                Native.InsertMenuW(this.systemMenu, (int)item.Id, Native.MF_BYCOMMAND | Native.MF_STRING, (uint)item.Id, item.Header);
            }

            HwndSource hwndSource = HwndSource.FromHwnd(interopHelper.Handle);
            hwndSource.AddHook(this.WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch ((uint)msg)
            {
                case Native.WM_SYSCOMMAND:
                    SystemMenuItem menuItem = this.MenuItems.Where(mi => mi.Id == wParam.ToInt32()).FirstOrDefault();
                    if (menuItem != null)
                    {
                        menuItem.Command.Execute(menuItem.CommandParameter);
                        handled = true;
                    }

                    break;

                case Native.WM_INITMENUPOPUP:
                    if (this.systemMenu == wParam)
                    {
                        foreach (SystemMenuItem item in this.MenuItems)
                        {
                            uint canExecute = item.Command.CanExecute(item.CommandParameter) ? Native.MF_ENABLED : Native.MF_DISABLED;
                            Native.EnableMenuItem(this.systemMenu, (uint)item.Id, canExecute);
                        }
                        handled = true;
                    }

                    break;
            }

            return IntPtr.Zero;
        }
    }
}
