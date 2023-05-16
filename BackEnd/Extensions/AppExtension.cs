﻿using BackEnd.Data;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Extensions;

public static class AppExtension
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString));
    }
}