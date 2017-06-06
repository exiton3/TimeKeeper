using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace TimeKeeperGadget
{
    public class HiderAltTab
    {
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr window, int index, int value);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr window, int index);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;

        public static void HideFromAltTab(IntPtr handle)
        {
            SetWindowLong(handle, GWL_EXSTYLE, GetWindowLong(handle,
                GWL_EXSTYLE) | WS_EX_TOOLWINDOW);
        }
        public static IntPtr GetWindowHandle(Window window)
        {
            return (new WindowInteropHelper(window)).Handle;
        }
    }
}