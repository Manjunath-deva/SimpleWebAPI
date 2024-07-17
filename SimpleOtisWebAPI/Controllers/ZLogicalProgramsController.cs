using Azure.Messaging;
using Microsoft.AspNetCore.Mvc;
using SimpleOtisAPI.Domain.Interfaces;

namespace SimpleOtisWebAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class ZLogicalProgramsController : ControllerBase
    {
        private readonly ILogicalPrograms _logicalPrograms;
        public ZLogicalProgramsController(ILogicalPrograms logicalPrograms)
        {
            _logicalPrograms = logicalPrograms;
        }

        [HttpGet]
        public IActionResult Palindrome([FromQuery]string text)
        {
            if(string.IsNullOrEmpty(text))
            {
                var response = new
                {
                    Success = false,
                    Message = "The input text is empty",
                    Code = StatusCodes.Status400BadRequest,
                };
                return BadRequest(response);
            }
            var result = _logicalPrograms.isPalindrome(text.ToLower());

            var response1 = new
            {
                Success = true,
                Message = result,
                Code = StatusCodes.Status200OK,
            };
            return Ok(response1);
            
        }

        [HttpGet]
        public IActionResult OccurencesOfCharactersInText([FromQuery]string text, char ch)
        {
            if (string.IsNullOrEmpty(text))
            {
                var response = new
                {
                    Success = false,
                    Message = "The input text is empty",
                    Code = StatusCodes.Status400BadRequest,
                };
                return BadRequest(response);
            }

            int occurences = _logicalPrograms.OccurencesOfCharacters(text.ToLower(), char.ToLower(ch));

            if(occurences == -1)
            {
                var response1 = new
                {
                    Occurences = $"Occuerences of character '{ch}' in '{text}' is 0",
                    Result = occurences,
                };

                return Ok(response1);
            }

            else
            {
                var response2 = new
                {
                    Occurences = $"Occuerences of character '{ch}' in '{text}' is {occurences}",
                    Result = occurences,
                };
                return Ok(response2);
            }
        }

        [HttpGet]
        public IActionResult SwappingOfNumbers([FromQuery]int a, int b, bool thirdVar)
        {
            if(a == 0 && b == 0)
            {
                var response = new
                {
                    Success = false,
                    Message = $"Please add some values to the variables",
                    Code = StatusCodes.Status400BadRequest,
                };
                return BadRequest(response);
            }
            
            var result = _logicalPrograms.SwappingOfNumbers(a, b, thirdVar);

            var response1 = new
            {
                Success = true,
                Message = result,
                Code = StatusCodes.Status200OK
            };

            return Ok(response1);
        }

        [HttpGet]
        public IActionResult SwappingOfStrings(string str1, string str2)
        {
            if(string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2))
            {
                var response = new
                {
                    Success = false,
                    Message = $"Please add some data to the string values and not null",
                    Code = StatusCodes.Status400BadRequest,
                };
                return BadRequest(response);
            }

            var result = _logicalPrograms.SwappingOfStrings(str1, str2);

            var response1 = new
            {
                Success = true,
                Message = result,
                Code = StatusCodes.Status200OK,
            };

            return Ok(response1);
        }

        [HttpGet]
        public IActionResult FibonacciSeries(int range)
        {
            if(range == 0)
            {
                var response = new
                {
                    Success = false,
                    Message = $"Please provide some range to the variable",
                    Code = StatusCodes.Status400BadRequest,
                };
                return BadRequest(response);
            }
            
            var result = _logicalPrograms.FibonacciSeries(range);

            var response1 = new
            {
                Success = true,
                Message = $"The Sum of fibonacci series is : {result}",
                Code = StatusCodes.Status200OK,
            };
            return Ok(response1);
        }
    }
}
