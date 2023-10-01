﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastBank.Domain
{
    public sealed class Bank
    {
        private Bank() { }
        private static Bank? instance = null;
        public static Bank Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Bank();
                }
                return instance;
            }
        }
        public Guid Id { get; private set; }
        public int CapitalAmount { get; private set; }
    }
}