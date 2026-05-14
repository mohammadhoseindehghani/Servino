using app.Application.Contracts.Common;
using app.Application.Contracts.Contracts.Repositories;
using app.Application.Contracts.DTOs.CommentDTOs;
using app.Domain.CommentAgg.Enums;
using app.Domain.SuggestionAgg.Enums;
using MediatR;
using FluentValidation;

namespace app.Application.Features.Comments.Commands.CreateComment;

public class CreateCommentCommandHandler(
    ICommentRepository commentRepository,
    IRequestRepository requestRepository,
    ISuggestionRepository suggestionRepository,
    IValidator<CreateCommentCommand> validator)
    : IRequestHandler<CreateCommentCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CreateCommentCommand request, CancellationToken ct)
    {
        var resultValidation = await validator.ValidateAsync(request, ct);

        if (!resultValidation.IsValid)
            throw new ValidationException(resultValidation.Errors);

        var requestEntity = await requestRepository.GetByIdAsync(request.RequestId, ct);
        if (requestEntity == null)
            return Result<bool>.Failure("درخواست یافت نشد.");

        if (requestEntity.Status != RequestStatus.Paid)
            return Result<bool>.Failure("ثبت نظر فقط پس از پرداخت و اتمام نهایی کار امکان‌پذیر است.");

        if (requestEntity.CustomerId != request.CustomerId)
            return Result<bool>.Failure("شما مالک این درخواست نیستید.");

        var exists = await commentRepository.ExistsByRequestIdAndCustomerIdAsync(request.RequestId, request.CustomerId, ct);
        if (exists)
            return Result<bool>.Failure("شما قبلاً برای این سفارش نظر ثبت کرده‌اید.");

        var suggestion = await suggestionRepository.GetByIdAsync(requestEntity.WinnerSuggestionId.Value, ct);
        if (suggestion.ExpertId != request.ExpertId)
            return Result<bool>.Failure("شما فقط می‌توانید برای متخصص انجام‌دهنده کار نظر دهید.");

        var result = await commentRepository.AddAsync(new CreateCommentDto
        {
            RequestId = request.RequestId,
            CustomerId = request.CustomerId,
            ExpertId = request.ExpertId,
            Title = request.Title,
            Rating = (Rate)request.Score,
            Text = request.Description
        }, ct);

        return result
            ? Result<bool>.Success(true, "نظر شما ثبت شد و پس از تایید نمایش داده می‌شود.")
            : Result<bool>.Failure("خطا در ثبت نظر.");
    }
}
