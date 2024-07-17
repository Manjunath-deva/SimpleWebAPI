using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOtisAPI.Domain.Interfaces
{
    public interface ILogicalPrograms
    {
        string isPalindrome(string text);
        int OccurencesOfCharacters(string text, char ch);
        string SwappingOfNumbers(int a, int b, bool thirdVar);
        string SwappingOfStrings(string str1, string str2);
        string FibonacciSeries(int range);
    }
}
