using KutuphaneCore.Entities;
using KutuphaneDataAccess.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneServis.Interfaces
{
    public interface IUserService
    {
        IResponse<UserCreateDto> CreateUser(UserCreateDto user);
        IResponse<string> Login(UserLoginDto user);

    }
}
