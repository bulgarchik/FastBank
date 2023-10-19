using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastBank.Services
{
    public interface IMenuService
    {
        public string PasswordStaredInput();

        public void MoveToPreviosLine(int countOfLines = 1);
    }
}
