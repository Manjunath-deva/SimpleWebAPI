using SimpleOtisAPI.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleOtisAPI.Domain.Interfaces
{
    public interface IRegister_Login
    {
        Task<bool> Register(Register_LoginDBDTO register);
        Task<Register_LoginDBDTO> Login(Register_LoginDBDTO login);
    }
}
