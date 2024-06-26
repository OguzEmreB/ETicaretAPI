﻿using ETicaret.Application.Abstractions.Services; 
using MediatR; 

namespace ETicaret.Application.Features.Commands.GoogleLogin
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
    {
        readonly IAuthService _authService;

        public GoogleLoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
        {
         var token = await _authService.GoogleLoginAsync(request.IdToken, 15);

            return new()
            {
                Token = token,
            };
        }
    }
}
