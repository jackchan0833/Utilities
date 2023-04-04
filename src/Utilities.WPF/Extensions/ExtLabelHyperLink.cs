using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace JC.Utilities.WPF.Extensions
{
    /// <summary>
    /// Represents the <see cref="Label"/> extension.
    /// </summary>
    public class ExtLabelHyperLink : Label
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
