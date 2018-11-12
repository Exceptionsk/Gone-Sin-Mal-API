using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Gone_Sin_Mal_API.Class
{
    public class StringMethods
    {
        public string GetBetween(String Source , String Str1  , String Str2  )
        {
            try
            {
                return Regex.Split(Regex.Split(Source, Str1)[1], Str2)[0];    
            }
            catch (Exception)
            {
                return "";
            }
        }


    }
}