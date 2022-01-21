﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.WebAPI.Clients.Base
{
    public abstract class BaseClient
    {
        protected HttpClient Http { get; }

        protected string Address { get; }

        protected BaseClient(HttpClient client, string Address)
        {
            Http = client;
            this.Address = Address;
        } 
    }
}
