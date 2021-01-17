using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotlaGel.WebApp.ViewModals
{
    public class WarningViewModal:NotifyViewModelBase<string>
    {
        public WarningViewModal()
        {
            Title = "Uyarı";
        }
    }
}