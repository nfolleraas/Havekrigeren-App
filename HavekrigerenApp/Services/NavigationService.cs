using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HavekrigerenApp.Services
{
    public static class NavigationService
    {
        // Service for page navigation

        public static IReadOnlyList<Page> ModalStack => throw new NotImplementedException();

        public static IReadOnlyList<Page> NavigationStack => throw new NotImplementedException();

        public static void InsertPageBefore(Page page, Page before)
        {
            throw new NotImplementedException();
        }

        public static async Task<Page> PopAsync()
        {
            Page currentPage = Shell.Current.CurrentPage;

            if (currentPage != null)
            {
                return await currentPage.Navigation.PopAsync();
            }
            else
            {
                throw new InvalidOperationException("Siden du prøver at tilgå er ikke tilgængelig.");
            }
        }

        public static Task<Page> PopAsync(bool animated)
        {
            throw new NotImplementedException();
        }

        public static Task<Page> PopModalAsync()
        {
            throw new NotImplementedException();
        }

        public static Task<Page> PopModalAsync(bool animated)
        {
            throw new NotImplementedException();
        }

        public static Task PopToRootAsync()
        {
            throw new NotImplementedException();
        }

        public static Task PopToRootAsync(bool animated)
        {
            throw new NotImplementedException();
        }

        // Navigate to a page
        public static async Task PushAsync(Page page)
        {
            Page currentPage = Shell.Current.CurrentPage;

            if (currentPage != null)
            {
                await currentPage.Navigation.PushAsync(page);
            }
            else
            {
                throw new InvalidOperationException("Siden du prøver at tilgå er ikke tilgængelig.");
            }
        }

        public static Task PushAsync(Page page, bool animated)
        {
            throw new NotImplementedException();
        }

        public static Task PushModalAsync(Page page)
        {
            throw new NotImplementedException();
        }

        public static Task PushModalAsync(Page page, bool animated)
        {
            throw new NotImplementedException();
        }

        public static void RemovePage(Page page)
        {
            throw new NotImplementedException();
        }
    }
}
