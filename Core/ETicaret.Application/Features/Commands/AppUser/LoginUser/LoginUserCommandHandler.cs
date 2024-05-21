using ETicaret.Application.Abstractions.Token;
using ETicaret.Application.DTOs;
using ETicaret.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        readonly UserManager<ETicaret.Domain.Entities.Identity.AppUser> _userManager;
        readonly SignInManager<ETicaret.Domain.Entities.Identity.AppUser> _signInManager;
        readonly ITokenHandler _tokenHandler;

        public LoginUserCommandHandler(UserManager<ETicaret.Domain.Entities.Identity.AppUser> userManager, 
            SignInManager<ETicaret.Domain.Entities.Identity.AppUser> signInManager, ITokenHandler tokenHandler)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            ETicaret.Domain.Entities.Identity.AppUser user = await _userManager.FindByNameAsync(request.UsernameOrEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(request.UsernameOrEmail);
            if (user == null)
                throw new NotFounUserException();
            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (result.Succeeded) // Authenticated
            {
                Token token = _tokenHandler.CreateAccessToken(5);
                return new LoginUserSuccessCommandResponse()
                {
                    Token = token
                };
            }
            //return new LoginUserErrorCommandResponse()
            //{
            //    Message = "Kullanıcı adı veya şifre hatalıdır"
            //};

            throw new AuthenticationErrorException();


        }
    }
}
