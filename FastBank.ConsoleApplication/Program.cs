﻿using FastBank.Domain;
using FastBank.Services;

namespace FastBank
{
    public class Program
    {
        static void Main(string[] args)
        {
            ICustomerService customerService = new CustomerService();

            MenuOptions.ShowMainMenu();
        }
    }
}