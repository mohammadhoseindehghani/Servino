using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Providers_Services;
using MediatR;

namespace app.Application.Features.Users.Commands.SendOtpCommand;

public class SendOtpCommandHandler(IIdentityService identityService) : IRequestHandler<SendOtpCommand, Result<string>>
{
    public async Task<Result<string>> Handle(SendOtpCommand request, CancellationToken ct)
    {
        var code = await identityService.SendOtpAsync(request.Command, ct);
        return Result<string>.Success(code, "کد تایید ارسال شد.");
    }
}
