using ELibrary.Domain;
using ELibrary.Domain.Identity;
using ELibrary.Repository;
using ELibrary.Repository.Implementation;
using ELibrary.Repository.Interface;
using ELibrary.Service.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELibrary.Web
{
    public class Startup
    {
        private EmailSettings emailSettings;
        public Startup(IConfiguration configuration)
        {
            emailSettings = new EmailSettings();
            Configuration = configuration;
            Configuration.GetSection("EmailSettings").Bind(emailSettings);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("PostgresqlRemodelConnection")));
            services.AddDefaultIdentity<ELibraryUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<ELibraryRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IPublisherRepository), typeof(PublisherRepository));
            services.AddScoped(typeof(ICategoryRepository), typeof(CategoryRepository));
            services.AddScoped(typeof(IAuthorRepository), typeof(AuthorRepository));
            services.AddScoped(typeof(IBookRepository), typeof(BookRepository));
            services.AddScoped(typeof(IBookAuthorRepository), typeof(BookAuthorRepository));
            services.AddScoped(typeof(IBookCategoriesRepository), typeof(BookCategoriesRepository));
            services.AddScoped(typeof(ICartRepository), typeof(CartRepository));
            services.AddScoped(typeof(IBookCartRepository), typeof(BookCartRepository));
            services.AddScoped(typeof(IRentRepository), typeof(RentRepository));
            services.AddScoped(typeof(IUserRoleRepository), typeof(UserRoleRepository));
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddScoped(typeof(IRoleRepository), typeof(RoleRepository));
            services.AddScoped(typeof(IReviewRepository), typeof(ReviewRepository));

            services.AddScoped<EmailSettings>(es => emailSettings);

            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));

            services.AddTransient<IAuthorService, Service.Implementation.AuthorService>();
            services.AddTransient<IPublisherService, Service.Implementation.PublisherService>();
            services.AddTransient<ICategoryService, Service.Implementation.CategoryService>();
            services.AddTransient<IBookService, Service.Implementation.BookService>();
            services.AddTransient<ICartService, Service.Implementation.CartService>();
            services.AddTransient<IRentService, Service.Implementation.RentService>();
            services.AddTransient<IUserService, Service.Implementation.UserService>();
            services.AddTransient<IReviewService, Service.Implementation.ReviewService>();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            StripeConfiguration.ApiKey = Configuration.GetSection("Stripe")["SecretKey"];
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
