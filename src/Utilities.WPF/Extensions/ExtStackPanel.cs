using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace JC.Utilities.WPF.Extensions
{
    /// <summary>
    /// Represents the <see cref="StackPanel"/> extensions.
    /// </summary>
    public class ExtStackPanel : StackPanel
    {
        /// <summary>
        /// The click event.
        /// </summary>
        public event MouseButtonEventHandler Clicked
        {
            add
            {
                lock (this)
                {
                    MouseUp += value;
                }
            }
            remove
            {
                lock (this)
                {
                    MouseUp -= value;
                }
            }
        }
    }
}
