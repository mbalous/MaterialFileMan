using System;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using Windows.Win32;

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

        private DestroyMenuSafeHandle systemMenu;

        /// <summary>
        /// Initializes a new instance of the SystemMenuWindow class.
        /// </summary>
        public SystemMenuWindow()
        {
            this.Loaded += this.SystemMenuWindow_Loaded;
            this.Unloaded += this.SystemMenuWindow_Unloaded;

            this.MenuItems = new FreezableCollection<SystemMenuItem>();
        }


        private static void OnMenuItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SystemMenuWindow window && e.NewValue is FreezableCollection<SystemMenuItem> collection)
                window.MenuItems = collection;
        }

        private unsafe void SystemMenuWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var interopHelper = new WindowInteropHelper(this);
            this.systemMenu = PInvoke.GetSystemMenu_SafeHandle((Windows.Win32.Foundation.HWND)interopHelper.Handle, false);

            if (this.MenuItems.Count > 0)
            {
                PInvoke.InsertMenu(
                    this.systemMenu,
                    10,
                    Windows.Win32.UI.WindowsAndMessaging.MENU_ITEM_FLAGS.MF_BYPOSITION | Windows.Win32.UI.WindowsAndMessaging.MENU_ITEM_FLAGS.MF_SEPARATOR,
                    0,
                    string.Empty);
            }

            foreach (var item in MenuItems)
            {
                PInvoke.InsertMenu(
                    this.systemMenu,
                    10,
                    Windows.Win32.UI.WindowsAndMessaging.MENU_ITEM_FLAGS.MF_BYCOMMAND | Windows.Win32.UI.WindowsAndMessaging.MENU_ITEM_FLAGS.MF_STRING,
                    item.Id,
                    item.Header);
            }

            HwndSource hwndSource = HwndSource.FromHwnd(interopHelper.Handle);
            hwndSource.AddHook(this.WndProc);
        }


        private void SystemMenuWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            this.systemMenu.Close();
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {

            switch ((uint)msg)
            {
                case Windows.Win32.PInvoke.WM_SYSCOMMAND:
                    SystemMenuItem menuItem = this.MenuItems.Where(mi => mi.Id == wParam.ToInt32()).FirstOrDefault();
                    if (menuItem != null)
                    {
                        menuItem.Command.Execute(menuItem.CommandParameter);
                        handled = true;
                    }

                    break;

                case Windows.Win32.PInvoke.WM_INITMENUPOPUP:
                    if (this.systemMenu.DangerousGetHandle() == wParam)
                    {
                        foreach (SystemMenuItem item in this.MenuItems)
                        {
                            var flag = item.Command.CanExecute(item.CommandParameter) ? Windows.Win32.UI.WindowsAndMessaging.MENU_ITEM_FLAGS.MF_ENABLED : Windows.Win32.UI.WindowsAndMessaging.MENU_ITEM_FLAGS.MF_DISABLED;
                            Windows.Win32.PInvoke.EnableMenuItem(this.systemMenu, item.Id, flag);
                        }
                        handled = true;
                    }

                    break;
            }

            return IntPtr.Zero;
        }
    }
}

