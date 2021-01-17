using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotlaGel.WebApp.ViewModals
{
    public class OkViewModal:NotifyViewModelBase<string>
    {
        public OkViewModal()
        {
            Title = "İşlem Başarılı.";
        }
    }
}