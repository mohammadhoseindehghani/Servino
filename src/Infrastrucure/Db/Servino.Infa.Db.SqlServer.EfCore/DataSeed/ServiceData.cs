using Servino.Domain.Core.CategoryAgg.Entity;
using Servino.Domain.Core.HomeServiceAgg.Entity;

namespace Servino.Infa.Db.SqlServer.EfCore.DataSeed
{
    public static class ServiceData
    {
        public static List<Category> GetCategories()
        {
            return new List<Category>
            {
                new Category
                {
                    Title = "تمیزکاری و نظافت",
                    IsActive = true,
                    SubCategories = new List<Category>
                    {
                        new Category
                        {
                            Title = "نظافت منزل و محل کار",
                            IsActive = true,
                            Services = new List<HomeService>
                            {
                                new HomeService { Title = "نظافتچی خانم (۴ ساعت)", BasePrice = 400000, IsActive = true, ShortDescription = "نظافت ظریف کاری و چیدمان" },
                                new HomeService { Title = "نظافتچی خانم (۸ ساعت)", BasePrice = 750000, IsActive = true, ShortDescription = "نظافت کامل روزانه" },
                                new HomeService { Title = "نظافتچی آقا (۴ ساعت)", BasePrice = 350000, IsActive = true, ShortDescription = "دیوارشویی و شیشه" },
                                new HomeService { Title = "نظافتچی آقا (۸ ساعت)", BasePrice = 650000, IsActive = true },
                                new HomeService { Title = "نظافت مشاعات و راه پله (تا ۴ طبقه)", BasePrice = 250000, IsActive = true },
                                new HomeService { Title = "نظافت شرکت و اداره (ساعتی)", BasePrice = 90000, IsActive = true },
                                new HomeService { Title = "پذیرایی مجالس (هر نفر ساعت)", BasePrice = 120000, IsActive = true }
                            }
                        },
                        new Category
                        {
                            Title = "خشکشویی و شستشو",
                            IsActive = true,
                            Services = new List<HomeService>
                            {
                                new HomeService { Title = "مبل‌شویی (ست ۷ نفره)", BasePrice = 850000, IsActive = true },
                                new HomeService { Title = "شستشوی خوشخواب دو نفره", BasePrice = 400000, IsActive = true },
                                new HomeService { Title = "موکت‌شویی (متری)", BasePrice = 25000, IsActive = true },
                                new HomeService { Title = "پرده‌شویی و نصب مجدد", BasePrice = 150000, IsActive = true },
                                new HomeService { Title = "قالیشویی ماشینی (متری)", BasePrice = 45000, IsActive = true },
                                new HomeService { Title = "قالیشویی دستبافت (متری)", BasePrice = 85000, IsActive = true }
                            }
                        },
                        new Category
                        {
                            Title = "نماشویی و کفسابی",
                            IsActive = true,
                            Services = new List<HomeService>
                            {
                                new HomeService { Title = "نماشویی با طناب (راپل)", BasePrice = 1500000, IsActive = true },
                                new HomeService { Title = "کفسابی سنگ و سرامیک", BasePrice = 80000, IsActive = true },
                                new HomeService { Title = "پیچ و رولپلاک سنگ نما", BasePrice = 50000, IsActive = true }
                            }
                        },
                        new Category
                        {
                            Title = "سم‌پاشی و ضدعفونی",
                            IsActive = true,
                            Services = new List<HomeService>
                            {
                                new HomeService { Title = "سم‌پاشی سوسک فاضلابی", BasePrice = 600000, IsActive = true },
                                new HomeService { Title = "سم‌پاشی ساس (تضیمنی)", BasePrice = 2000000, IsActive = true },
                                new HomeService { Title = "طمع‌گذاری موش", BasePrice = 450000, IsActive = true }
                            }
                        }
                    }
                },
                new Category
                {
                    Title = "سرمایش و گرمایش",
                    IsActive = true,
                    SubCategories = new List<Category>
                    {
                        new Category
                        {
                            Title = "کولر آبی",
                            IsActive = true,
                            Services = new List<HomeService>
                            {
                                new HomeService { Title = "سرویس کامل کولر آبی", BasePrice = 350000, IsActive = true },
                                new HomeService { Title = "تعویض پوشال و تسمه", BasePrice = 150000, IsActive = true },
                                new HomeService { Title = "تعویض موتور کولر", BasePrice = 200000, IsActive = true },
                                new HomeService { Title = "نصب سایبان کولر", BasePrice = 100000, IsActive = true }
                            }
                        },
                        new Category
                        {
                            Title = "کولر گازی و اسپیلت",
                            IsActive = true,
                            Services = new List<HomeService>
                            {
                                new HomeService { Title = "نصب کولر گازی", BasePrice = 900000, IsActive = true },
                                new HomeService { Title = "شارژ گاز (R22)", BasePrice = 1200000, IsActive = true },
                                new HomeService { Title = "رفع نشتی آب پنل", BasePrice = 400000, IsActive = true },
                                new HomeService { Title = "سرویس پنل داخلی و خارجی", BasePrice = 500000, IsActive = true }
                            }
                        },
                        new Category
                        {
                            Title = "پکیج و آبگرمکن",
                            IsActive = true,
                            Services = new List<HomeService>
                            {
                                new HomeService { Title = "سرویس کامل پکیج دیواری", BasePrice = 650000, IsActive = true },
                                new HomeService { Title = "رسوب‌گیری پکیج", BasePrice = 800000, IsActive = true },
                                new HomeService { Title = "تعمیر آبگرمکن دیواری", BasePrice = 400000, IsActive = true },
                                new HomeService { Title = "هواگیری رادیاتورها", BasePrice = 150000, IsActive = true }
                            }
                        },
                         new Category
                        {
                            Title = "لوله کشی و موتورخانه",
                            IsActive = true,
                            Services = new List<HomeService>
                            {
                                new HomeService { Title = "تعمیر مشعل موتورخانه", BasePrice = 900000, IsActive = true },
                                new HomeService { Title = "عایق کاری لوله ها", BasePrice = 300000, IsActive = true }
                            }
                        }
                    }
                },

                new Category
                {
                    Title = "دکوراسیون و بازسازی",
                    IsActive = true,
                    SubCategories = new List<Category>
                    {
                        new Category
                        {
                            Title = "نقاشی ساختمان",
                            IsActive = true,
                            Services = new List<HomeService>
                            {
                                new HomeService { Title = "نقاشی پلاستیک (متری)", BasePrice = 45000, IsActive = true },
                                new HomeService { Title = "نقاشی روغنی (متری)", BasePrice = 75000, IsActive = true },
                                new HomeService { Title = "نقاشی اکریلیک (بدون بو)", BasePrice = 70000, IsActive = true },
                                new HomeService { Title = "بتونه کاری و زیرسازی", BasePrice = 30000, IsActive = true }
                            }
                        },
                        new Category
                        {
                            Title = "کاغذ دیواری و کفپوش",
                            IsActive = true,
                            Services = new List<HomeService>
                            {
                                new HomeService { Title = "نصب کاغذ دیواری (رول)", BasePrice = 120000, IsActive = true },
                                new HomeService { Title = "نصب پوستر سه بعدی", BasePrice = 250000, IsActive = true },
                                new HomeService { Title = "نصب پارکت و لمینت (متری)", BasePrice = 60000, IsActive = true },
                                new HomeService { Title = "نصب قرنیز", BasePrice = 25000, IsActive = true }
                            }
                        },
                        new Category
                        {
                            Title = "بنایی و کاشی‌کاری",
                            IsActive = true,
                            Services = new List<HomeService>
                            {
                                new HomeService { Title = "کاشی‌کاری سرویس بهداشتی", BasePrice = 200000, IsActive = true },
                                new HomeService { Title = "سیمان‌کاری و دیوارچینی", BasePrice = 180000, IsActive = true },
                                new HomeService { Title = "تخریب دیوار و اوپن", BasePrice = 1500000, IsActive = true },
                                new HomeService { Title = "رفع نم و رطوبت (بدون تخریب)", BasePrice = 850000, IsActive = true }
                            }
                        },
                         new Category
                        {
                            Title = "کابینت و نجاری",
                            IsActive = true,
                            Services = new List<HomeService>
                            {
                                new HomeService { Title = "ساخت کابینت MDF (متری)", BasePrice = 4500000, IsActive = true },
                                new HomeService { Title = "تعمیر درب کابینت و کمد", BasePrice = 200000, IsActive = true },
                                new HomeService { Title = "ساخت کمد دیواری", BasePrice = 3000000, IsActive = true }
                            }
                        }
                    }
                },

                new Category
                {
                    Title = "تعمیرات لوازم خانگی",
                    IsActive = true,
                    SubCategories = new List<Category>
                    {
                        new Category
                        {
                            Title = "لوازم آشپزخانه",
                            IsActive = true,
                            Services = new List<HomeService>
                            {
                                new HomeService { Title = "تعمیر یخچال ساید بای ساید", BasePrice = 800000, IsActive = true },
                                new HomeService { Title = "شارژ گاز یخچال", BasePrice = 1200000, IsActive = true },
                                new HomeService { Title = "تعمیر ماشین لباسشویی", BasePrice = 450000, IsActive = true },
                                new HomeService { Title = "تعمیر ماشین ظرفشویی", BasePrice = 550000, IsActive = true },
                                new HomeService { Title = "تعمیر مایکروفر", BasePrice = 300000, IsActive = true },
                                new HomeService { Title = "تعمیر اجاق گاز رومیزی", BasePrice = 250000, IsActive = true }
                            }
                        },
                        new Category
                        {
                            Title = "صوتی و تصویری",
                            IsActive = true,
                            Services = new List<HomeService>
                            {
                                new HomeService { Title = "تعمیر تلویزیون LED/LCD", BasePrice = 600000, IsActive = true },
                                new HomeService { Title = "نصب آنتن مرکزی", BasePrice = 400000, IsActive = true },
                                new HomeService { Title = "نصب پایه دیواری تلویزیون", BasePrice = 150000, IsActive = true }
                            }
                        }
                    }
                },

                new Category
                {
                    Title = "برق و الکترونیک",
                    IsActive = true,
                    SubCategories = new List<Category>
                    {
                        new Category
                        {
                            Title = "برقکاری ساختمان",
                            IsActive = true,
                            Services = new List<HomeService>
                            {
                                new HomeService { Title = "رفع اتصالی برق", BasePrice = 250000, IsActive = true },
                                new HomeService { Title = "نصب لوستر و دیوارکوب", BasePrice = 180000, IsActive = true },
                                new HomeService { Title = "نصب کلید و پریز (تعداد بالا)", BasePrice = 35000, IsActive = true },
                                new HomeService { Title = "سیم‌کشی تلفن و آیفون", BasePrice = 300000, IsActive = true },
                                new HomeService { Title = "نورپردازی کناف و هالوژن", BasePrice = 50000, IsActive = true }
                            }
                        },
                        new Category
                        {
                            Title = "سیستم‌های امنیتی",
                            IsActive = true,
                            Services = new List<HomeService>
                            {
                                new HomeService { Title = "نصب دوربین مداربسته", BasePrice = 400000, IsActive = true },
                                new HomeService { Title = "نصب دزدگیر اماکن", BasePrice = 600000, IsActive = true },
                                new HomeService { Title = "تعمیر درب ریموت دار", BasePrice = 350000, IsActive = true }
                            }
                        }
                    }
                },

                new Category
                {
                    Title = "تأسیسات و لوله کشی",
                    IsActive = true,
                    SubCategories = new List<Category>
                    {
                        new Category
                        {
                            Title = "لوله کشی آب و گاز",
                            IsActive = true,
                            Services = new List<HomeService>
                            {
                                new HomeService { Title = "رفع گرفتگی لوله (فنر زنی)", BasePrice = 350000, IsActive = true },
                                new HomeService { Title = "نصب شیرآلات اهرمی", BasePrice = 120000, IsActive = true },
                                new HomeService { Title = "نصب توالت فرنگی", BasePrice = 450000, IsActive = true },
                                new HomeService { Title = "تشخیص ترکیدگی با دستگاه", BasePrice = 500000, IsActive = true },
                                new HomeService { Title = "نصب فلاش تانک", BasePrice = 150000, IsActive = true }
                            }
                        }
                    }
                },
                new Category
                {
                    Title = "اسباب‌کشی و باربری",
                    IsActive = true,
                    SubCategories = new List<Category>
                    {
                        new Category
                        {
                            Title = "ماشین باربری",
                            IsActive = true,
                            Services = new List<HomeService>
                            {
                                new HomeService { Title = "خاور مسقف (۳ ساعت)", BasePrice = 1200000, IsActive = true },
                                new HomeService { Title = "نیسان بار (۲ ساعت)", BasePrice = 600000, IsActive = true },
                                new HomeService { Title = "وانت بار (۲ ساعت)", BasePrice = 400000, IsActive = true }
                            }
                        },
                        new Category
                        {
                            Title = "کارگر جابجایی",
                            IsActive = true,
                            Services = new List<HomeService>
                            {
                                new HomeService { Title = "کارگر اسباب کشی (۳ ساعت)", BasePrice = 300000, IsActive = true },
                                new HomeService { Title = "کارگر حمل یخچال ساید (طبقاتی)", BasePrice = 100000, IsActive = true },
                                new HomeService { Title = "بسته‌بندی وسایل منزل", BasePrice = 250000, IsActive = true }
                            }
                        }
                    }
                },

                new Category
                {
                    Title = "خدمات خودرو در محل",
                    IsActive = true,
                    SubCategories = new List<Category>
                    {
                        new Category
                        {
                            Title = "کارواش سیار",
                            IsActive = true,
                            Services = new List<HomeService>
                            {
                                new HomeService { Title = "کارواش نانو (سواری)", BasePrice = 180000, IsActive = true },
                                new HomeService { Title = "کارواش نانو (شاسی بلند)", BasePrice = 250000, IsActive = true },
                                new HomeService { Title = "صفرشویی تخصصی", BasePrice = 1500000, IsActive = true },
                                new HomeService { Title = "پولیش و واکس بدنه", BasePrice = 900000, IsActive = true }
                            }
                        },
                        new Category
                        {
                            Title = "مکانیک و باتری‌ساز",
                            IsActive = true,
                            Services = new List<HomeService>
                            {
                                new HomeService { Title = "تعویض روغن در محل", BasePrice = 150000, IsActive = true },
                                new HomeService { Title = "تعویض لنت ترمز", BasePrice = 200000, IsActive = true },
                                new HomeService { Title = "باتری به باتری", BasePrice = 100000, IsActive = true },
                                new HomeService { Title = "دیاگ و عیب یابی", BasePrice = 250000, IsActive = true }
                            }
                        }
                    }
                },

                new Category
                {
                    Title = "زیبایی و سلامت",
                    IsActive = true,
                    SubCategories = new List<Category>
                    {
                        new Category
                        {
                            Title = "خدمات زیبایی بانوان",
                            IsActive = true,
                            Services = new List<HomeService>
                            {
                                new HomeService { Title = "کوتاهی مو در منزل", BasePrice = 250000, IsActive = true },
                                new HomeService { Title = "اصلاح صورت و ابرو", BasePrice = 150000, IsActive = true },
                                new HomeService { Title = "مانیکور و پدیکور", BasePrice = 350000, IsActive = true },
                                new HomeService { Title = "رنگ مو (ریشه)", BasePrice = 400000, IsActive = true }
                            }
                        },
                        new Category
                        {
                            Title = "پیرایش آقایان",
                            IsActive = true,
                            Services = new List<HomeService>
                            {
                                new HomeService { Title = "اصلاح موی سر آقا", BasePrice = 200000, IsActive = true },
                                new HomeService { Title = "اصلاح صورت و ریش", BasePrice = 100000, IsActive = true },
                                new HomeService { Title = "گریم داماد", BasePrice = 1500000, IsActive = true }
                            }
                        },
                         new Category
                        {
                            Title = "خدمات درمانی و پرستاری",
                            IsActive = true,
                            Services = new List<HomeService>
                            {
                                new HomeService { Title = "تزریقات و سرم تراپی", BasePrice = 150000, IsActive = true },
                                new HomeService { Title = "پرستاری از سالمند (روزانه)", BasePrice = 600000, IsActive = true },
                                new HomeService { Title = "فیزیوتراپی در منزل", BasePrice = 500000, IsActive = true }
                            }
                        }
                    }
                },

                new Category
                {
                    Title = "دیجیتال و نرم افزار",
                    IsActive = true,
                    SubCategories = new List<Category>
                    {
                        new Category
                        {
                            Title = "خدمات کامپیوتری",
                            IsActive = true,
                            Services = new List<HomeService>
                            {
                                new HomeService { Title = "نصب ویندوز و درایور", BasePrice = 250000, IsActive = true },
                                new HomeService { Title = "ویروس کشی سیستم", BasePrice = 150000, IsActive = true },
                                new HomeService { Title = "ارتقا رم و هارد", BasePrice = 200000, IsActive = true },
                                new HomeService { Title = "تنظیم مودم و شبکه", BasePrice = 180000, IsActive = true }
                            }
                        }
                    }
                },

                new Category
                {
                    Title = "باغبانی و فضای سبز",
                    IsActive = true,
                    SubCategories = new List<Category>
                    {
                        new Category
                        {
                            Title = "خدمات گل و گیاه",
                            IsActive = true,
                            Services = new List<HomeService>
                            {
                                new HomeService { Title = "هرس درختان (هر اصله)", BasePrice = 100000, IsActive = true },
                                new HomeService { Title = "باغچه کاری و گلکاری", BasePrice = 300000, IsActive = true },
                                new HomeService { Title = "سم‌پاشی گیاهان", BasePrice = 250000, IsActive = true },
                                new HomeService { Title = "تعویض خاک گلدان", BasePrice = 50000, IsActive = true }
                            }
                        }
                    }
                }
            };
        }
    }
}