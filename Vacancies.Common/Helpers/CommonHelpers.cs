using System;
using System.Collections.Generic;
using System.Text;
using Vacancies.ViewModels;

namespace Vacancies.Common.Helpers
{
    public class CommonHelpers
    {
        public static string CreateStringFromNull(string foo)
        {
            return foo != null ? foo : "";
        }

       
    }
}
