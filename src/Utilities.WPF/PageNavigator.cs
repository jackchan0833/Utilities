using JC.Utilities.Logging;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace JC.Utilities.WPF
{
    public class PageNavigator
    {
        /// <summary>
        /// Naviage to destination page.
        /// </summary>
        /// <typeparam name="TDestPage">The destination page to navigate.</typeparam>
        /// <param name="currentPage">The current page.</param>
        public static Task GoToAsync<TDestPage>(Page currentPage)
            where TDestPage : Page
        {
            currentPage.Dispatcher.Invoke(() =>
            {
                var type = typeof(TDestPage);
                var destPage = (TDestPage)Activator.CreateInstance(type);
                currentPage.NavigationService.Navigate(destPage);
            });
            return Task.CompletedTask;
        }
        /// <summary>
        /// Go back base on current page.
        /// </summary>
        public static Task GoToBackAsync(Page currentPage)
        {
            currentPage.Dispatcher.Invoke(() =>
            {
                currentPage.NavigationService.GoBack();
            });
            return Task.CompletedTask;
        }
        /// <summary>
        /// Show the exception error page.
        /// </summary>
        public static Task ShowError(Page page, Exception ex, bool logTheException = true)
        {
            if (logTheException)
            {
                Logger.Write(ex);
            }
            page.Dispatcher.Invoke(() =>
            {
                MessageBox.Show("Error occurred：" + ex.Message);
            });
            return Task.CompletedTask;
        }
    }
}
