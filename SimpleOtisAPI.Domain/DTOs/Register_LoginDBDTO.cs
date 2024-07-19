using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SimpleOtisAPI.Domain.DTOs
{
    public class Register_LoginDBDTO
    {
        [JsonIgnore]
        public int ID { get; set; }
        public string Azure_ID { get; set; }
        public string User_Name { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
    }
}
