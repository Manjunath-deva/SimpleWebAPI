using Microsoft.EntityFrameworkCore;
using SimpleOtisAPI.Domain.DTOs;
using SimpleOtisAPI.Domain.Interfaces;
using SimpleOtisAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOtisAPI.Domain.Services
{
    public class Register_Login : IRegister_Login
    {
        private readonly SimpleOtisAPIContext _context;
        public Register_Login(SimpleOtisAPIContext context)
        {
            _context = context;
        }

        public async Task<bool> Register(Register_LoginDBDTO register)
        {
            _context.tblUsers.Add(register);
            var response = await _context.SaveChangesAsync();
            return response == 0 ? false : true;
        }

        public async Task<Register_LoginDBDTO> Login(Register_LoginDBDTO login)
        {
            var response = await _context.tblUsers.FirstOrDefaultAsync(e => e.User_Name == login.User_Name);

            return response ?? new Register_LoginDBDTO();

        }
    }
}
