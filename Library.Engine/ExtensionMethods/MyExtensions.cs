using System;
using System.Linq;

namespace Library.Engine.ExtensionMethods
{
    // I made this extension method to solve the Acrynom problem.
    // Shortly explained: I split the title to an string array.
    // Then loop over it and build up a new string. I substring to
    // get only the first letter in the string.
    public static class MyExtensions
    {
        public static string ToAcronym(this String title)
        {
            var strSplit = title.Split(' ');
            var acronym = "";

            foreach (var s in strSplit)
            {
                if (s.All(char.IsDigit))
                {
                    acronym += s;
                }

                else
                {
                    acronym += s.Substring(0, 1).ToUpper();
                }
            }

            return title + " " + "(" + acronym + ")";
        }
    }
}