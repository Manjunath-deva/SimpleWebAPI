using SimpleOtisAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOtisAPI.Domain.Services
{
    public class LogicalPrograms : ILogicalPrograms
    {
        public string isPalindrome(string text)
        {
            try
            {
                var reversedtext = "";
                for(int i = text.Length-1;i >= 0;i--)
                {
                    reversedtext = reversedtext + text[i];
                }
                var result = (text == reversedtext) ? $"The input text '{text}' is Palindrome" : $"The input text '{text}' is not a Palindrome";
                return result ;
            }
            catch(Exception ex) 
            {
                return ex.Message;
            }
        }

        public int OccurencesOfCharacters(string text, char ch)
        {
            try
            {
                int occurences = 0;
                for (int i=0;i<text.Length;i++)
                {
                    if (text[i] == ch)
                    {
                        occurences++;
                    }
                }

                int result = occurences == 0 ? -1 : occurences;

                return result;
            }
            catch(Exception ex)
            {
                return -1;
            }
        }

        public string SwappingOfNumbers(int a, int b, bool thirdVar)
        {
            try
            {
                if (thirdVar)
                {
                    //Swapping using third variable
                    var result = $"Before Swapping a:{a}, b:{b}";
                    int c = a;
                    a = b;
                    b = c;

                    return result = result + " " + $"After Swapping a:{a}, b:{b}";
                }
                else
                {
                    //Swapping using only two variables
                    var result = $"Before Swapping a:{a}, b:{b}";
                    a = a + b;
                    b = a - b;
                    a = a - b;
                    return result = result + " " + $"After Swapping a:{a}, b:{b}";
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string SwappingOfStrings(string str1, string str2)
        {
            try
            {
                var result = $"Before Swapping: str1:{str1}, str2:{str2}";
                str1 = str1 + str2;
                str2 = str1.Substring(0, str1.Length - str2.Length);
                str1 = str1.Substring(str2.Length);

                return result + $"  After Swapping: str1:{str1}, str2:{str2}";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string FibonacciSeries(int range)
        {
            int a = 0, b = 1;
            string result = "0, 1";

            for (int i=2;i<range;i++)
            {
                int c = a + b;
                a = b;
                b = c;
                result += $", {c}";
            }

            return result;
        }
    }
}
