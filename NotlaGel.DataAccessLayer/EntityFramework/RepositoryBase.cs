﻿using NotlaGel.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotlaGel.DataAccessLayer.EntityFramework
{
    public class RepositoryBase
    {
        protected static DatabaseContext context;
        private static object _locSync = new object();
        protected RepositoryBase()
        {
          CreateContext();
        }
        private static void CreateContext()
        {
            if (context == null)
            {
                lock (_locSync)
                {
                    if (context == null) 
                    {
                        context = new DatabaseContext();
                    }
                        
                }
                
            }
        }
    }
}
