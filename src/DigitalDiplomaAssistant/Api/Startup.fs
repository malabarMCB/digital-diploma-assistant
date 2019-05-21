namespace Api

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Authentication.Cookies
open Microsoft.AspNetCore.Http

type Startup private () =
    new (configuration: IConfiguration) as this =
        Startup() then
        this.Configuration <- configuration

    // This method gets called by the runtime. Use this method to add services to the container.
    member this.ConfigureServices(services: IServiceCollection) =
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(Action<CookieAuthenticationOptions> (fun options -> 
            options.LoginPath <- Microsoft.AspNetCore.Http.PathString("/Login")
            ())) |> ignore
        
        services.AddHttpContextAccessor() |> ignore
        services.AddTransient<IHttpContextAccessor, HttpContextAccessor> |> ignore

        // Add framework services.
        services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1) |> ignore

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment) =
        if (env.IsDevelopment()) then
            app.UseDeveloperExceptionPage() |> ignore

        app.UseAuthentication |> ignore
        app.UseStaticFiles() |> ignore
        
        app.UseMvc(fun routes ->
            routes.MapRoute("default", "{controller=Dashboard}/{action=Index}/{id?}") |> ignore
            ) |> ignore

    member val Configuration : IConfiguration = null with get, set