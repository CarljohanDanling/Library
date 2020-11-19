using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Library.Engine.ExtensionMethods
{
    public static class MyExtensions
    {
        public static string Acronym(this String title)
        {
            var strSplit = title.Split(' ');
            var acronym = "";

            foreach (var s in strSplit)
            {
                if (s.All(char.IsDigit))
                {
                    acronym += s;
                }

                acronym += s.Substring(0, 1).ToUpper();
            }

            return title + " " + "(" + acronym + ")";
        }
    }
}