using System;
using System.Runtime.InteropServices;

namespace FileManager
{
    internal class Native
    {
        /// <summary>
        /// A window receives this message when the user chooses a command from the Window menu (formerly known as the system
        /// or control menu) or when the user chooses the maximize button, minimize button, restore button, or close button.
        /// </summary>
        public const uint WM_SYSCOMMAND = 0x112;

        /// <summary>
        /// Sent when a drop-down menu or submenu is about to become active.
        /// </summary>
        public const uint WM_INITMENUPOPUP = 0x0117;

        /// <summary>
        /// Draws a horizontal dividing line.
        /// </summary>
        public const uint MF_SEPARATOR = 0x800;

        /// <summary>
        /// Indicates that the uPosition parameter gives the identifier of the menu item.
        /// 
        /// </summary>
        public const uint MF_BYCOMMAND = 0x0;

        public const uint MF_BYPOSITION = 0x400;

        public const uint MF_STRING = 0x0;

        public const uint MF_ENABLED = 0x0;

        public const uint MF_DISABLED = 0x2;

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu([In] IntPtr hWnd, [In] bool bRevert);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool InsertMenuW([In] IntPtr hmenu, [In] int position, [In] uint flags, [In] uint item_id, [In, MarshalAs(UnmanagedType.LPTStr)]string item_text);

        [DllImport("user32.dll")]
        public static extern bool EnableMenuItem([In] IntPtr hMenu, [In] uint uIDEnableItem, [In] uint uEnable);
    }
}
