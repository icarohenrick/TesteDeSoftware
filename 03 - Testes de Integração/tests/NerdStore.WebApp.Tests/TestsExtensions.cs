using System;
using System.Linq;

namespace NerdStore.WebApp.Tests
{
    public static class TestsExtensions
    {
        public static decimal ApenasNumeros(this string value)
        {
            return Convert.ToDecimal(new string(value.Where(char.IsDigit).ToArray()));
        }
    }
}