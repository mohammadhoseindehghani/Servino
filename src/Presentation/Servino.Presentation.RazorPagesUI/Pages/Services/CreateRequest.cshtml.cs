using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core._common;
using Servino.Domain.Core.HomeServiceAgg.Contracts.AppService;
using Servino.Domain.Core.LocationAgg.Contracts.AppService;
using Servino.Domain.Core.LocationAgg.Dtos;
using Servino.Domain.Core.RequestAgg.Contracts.AppService;
using Servino.Domain.Core.RequestAgg.Dtos;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Presentation.RazorPagesUI.Extensions;
using Servino.Presentation.RazorPagesUI.Services.File;
using System.ComponentModel.DataAnnotations;
using Servino.Framework.Extensions;

namespace Servino.Presentation.RazorPagesUI.Pages.Services
{
    [Authorize(Roles = "Customer")]
    public class CreateRequestModel(
            IRequestAppService requestAppService,
            IHomeServiceAppService homeServiceAppService,
            IProvinceAppService provinceAppService,
            ICityAppService cityAppService,
            IUserAppService userAppService,
            IFileService fileService,
            ILogger<CreateRequestModel> logger) : PageModel
    {
        public string ServiceTitle { get; set; }
        public string ServiceImage { get; set; }
        public decimal BasePrice { get; set; }

        public List<SelectListDto> Provinces { get; set; } = [];

        [BindProperty]
        public CreateRequestInput Input { get; set; } = new();

        [TempData] public string? ErrorMessage { get; set; }
        [TempData] public string? SuccessMessage { get; set; }

        public async Task<IActionResult> OnGet(int serviceId, CancellationToken ct)
        {
            var serviceResult = await homeServiceAppService.GetByIdAsync(serviceId, ct);
            if (serviceResult == null) return RedirectToPage("/Index");

            ServiceTitle = serviceResult.Data.Title;
            BasePrice = serviceResult.Data.BasePrice;
            ServiceImage = serviceResult.Data.ImagePath;

            Input.HomeServiceId = serviceId;

            Provinces = await provinceAppService.GetAllForDropdownAsync(ct);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await RepopulatePageData(Input.HomeServiceId, ct);
                return Page();
            }

            var customerId = User.GetCustomerId();

            if (customerId == 0)
            {
                ErrorMessage = "خطا: شناسه مشتری یافت نشد.";
                await RepopulatePageData(Input.HomeServiceId, ct);
                return Page();
            }

            var uploadedPaths = new List<string>();
            if (Input.Images != null && Input.Images.Count > 0)
            {
                foreach (var file in Input.Images)
                {
                    try
                    {
                        var path = await fileService.Upload(file, "requests", ct);
                        uploadedPaths.Add(path);
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = $"خطا در آپلود عکس: {ex.Message}";
                        await RepopulatePageData(Input.HomeServiceId, ct);
                        return Page();
                    }
                }
            }

            var createDto = new CreateRequestDto
            {
                Title = Input.Title,
                Description = Input.Description,
                Address = Input.Address,
                CityId = Input.CityId,
                DateRequired = Input.DateRequired.ToGregorianDateTime()!.Value, 
                HomeServiceId = Input.HomeServiceId,
                CustomerId = customerId,
                ImagePaths = uploadedPaths
            };

            var result = await requestAppService.CreateAsync(createDto, ct);

            if (result.IsSuccess)
            {
                logger.LogInformation($"Request created successfully. CustomerId = {customerId}");
                SuccessMessage = "سفارش شما با موفقیت ثبت شد. منتظر پیشنهاد متخصصین باشید.";
                return RedirectToPage("/MyRequests", new { area = "Customer" });
            }
            else
            {
                logger.LogWarning($"Request creation failed. Message = {result.Message}");
                ErrorMessage = result.Message;
                await RepopulatePageData(Input.HomeServiceId, ct);
                return Page();
            }
        }


        public async Task<JsonResult> OnGetGetCities(int serviceId, int provinceId, CancellationToken ct)
        {
            var cities = await cityAppService.GetCitiesByProvinceIdAsync(provinceId, ct);
            return new JsonResult(cities);
        }

        private async Task RepopulatePageData(int serviceId, CancellationToken ct)
        {
            var serviceResult = await homeServiceAppService.GetByIdAsync(serviceId, ct);
            if (serviceResult != null!)
            {
                ServiceTitle = serviceResult.Data!.Title;
                BasePrice = serviceResult.Data!.BasePrice;
                ServiceImage = serviceResult.Data.ImagePath!;
            }
            Provinces = await provinceAppService.GetAllForDropdownAsync(ct);
        }

        private async Task<int> GetCurrentUserIdAsync(CancellationToken ct)
        {
            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName)) return 0;
            var search = new PaginationRequestDto { SearchKey = userName };
            var listResult = await userAppService.GetUsersListAsync(search, ct);
            return listResult.IsSuccess && listResult.Data!.Any() ? listResult.Data!.First().Id : 0;
        }

        public class CreateRequestInput
        {
            public int HomeServiceId { get; set; }

            [Required(ErrorMessage = "عنوان سفارش الزامی است")]
            public string Title { get; set; }

            [Required(ErrorMessage = "توضیحات سفارش الزامی است")]
            [MinLength(10, ErrorMessage = "توضیحات باید حداقل ۱۰ کاراکتر باشد")]
            public string Description { get; set; }

            [Required(ErrorMessage = "آدرس دقیق الزامی است")]
            public string Address { get; set; }

            [Required(ErrorMessage = "انتخاب استان الزامی است")]
            public int ProvinceId { get; set; } 

            [Required(ErrorMessage = "انتخاب شهر الزامی است")]
            public int CityId { get; set; }

            [Required(ErrorMessage = "تاریخ انجام کار الزامی است")]
            public string DateRequired { get; set; } 

            public List<IFormFile>? Images { get; set; }
        }
    }
}
