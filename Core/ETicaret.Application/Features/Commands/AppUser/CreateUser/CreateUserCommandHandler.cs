using ETicaret.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ETicaret.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {
        readonly UserManager<ETicaret.Domain.Entities.Identity.AppUser> _userManager;

        public CreateUserCommandHandler(UserManager<ETicaret.Domain.Entities.Identity.AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id=Guid.NewGuid().ToString(),
                UserName = request.Username,
                Email = request.Email,
                NameSurname = request.NameSurname,

            }, request.Password);


            CreateUserCommandResponse response = new() { Succeeded = result.Succeeded };

            if (result.Succeeded)
                response.Message = "Kullanıcı kaydı başarıyla oluşturuldu";
            else
                foreach (var error in result.Errors)
                    response.Message += $"{error.Code} - {error.Description}\n";
            return response;
 
            }
        }

    }
