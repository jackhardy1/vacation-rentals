using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using VacationRental.Contact.Api.Infrastructure.Middleware;
using VacationRental.Domain.Contact.EntityFramework;
using VacationRental.Domain.Contact.Queries;
using VacationRental.Domain.Contact.Commands;

namespace VacationRental.Contact.Api
{
    using AutoMapper;
    using FluentValidation;
    using FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Http;
    using VacationRental.Domain.Contact.Models;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(fv => { });

            services.AddTransient<IValidator<Contact>, ContactPreValidator>();

            services.AddSwaggerGen(opts => opts.SwaggerDoc("v1", new Info { Title = "Vacation rental contact information", Version = "v1" }));

            services.AddSingleton<IDictionary<int, ContactViewModel>>(new Dictionary<int, ContactViewModel>());
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMediatR(typeof(Startup));

            services.AddMediatR(typeof(ContactQuery));
            services.AddMediatR(typeof(CreateContactCommand));
            services.AddMediatR(typeof(UpdateContactCommand));

            services.AddAutoMapper(typeof(Startup));

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ContactContext>(options =>
              options.UseNpgsql(connectionString,
                  optionsBuilder =>
                      optionsBuilder.MigrationsAssembly("VacationRental.Contact.Api")
                  )
             );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
               app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/swagger/v1/swagger.json", "Contact v1"));
        }
    }
}
