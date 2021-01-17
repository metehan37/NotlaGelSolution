using NotlaGel.Common;
using NotlaGel.Entities;
using NotlaGel.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotlaGel.WebApp.Init
{
    public class WebCommon: ICommon
    {
        public string GetCurrentUsername()
        {
            NotlaGelUser user = CurrentSession.user;

            if (user!=null)
                return user.UserName;
            else
                return "system";


        }
    }
}