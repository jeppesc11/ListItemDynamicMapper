﻿using CA_PnP_Core.Extentions;
using CA_PnP_Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PnP.Core.Auth;
using PnP.Core.Model;
using PnP.Core.QueryModel;
using PnP.Core.Services;

string clientId = "";
string siteUrl = "";

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) => {
        // Add PnP Core SDK
        services.AddPnPCore(options => {
            // Configure the interactive authentication provider as default
            options.DefaultAuthenticationProvider = new InteractiveAuthenticationProvider() {
                ClientId = clientId,
                RedirectUri = new Uri("http://localhost")
            };
        });
    })
    .UseConsoleLifetime()
    .Build();

// Start the host
await host.StartAsync();

using (var scope = host.Services.CreateScope()) {
    // Ask an IPnPContextFactory from the host
    var pnpContextFactory = scope.ServiceProvider.GetRequiredService<IPnPContextFactory>();

    // Create a PnPContext
    using (var context = await pnpContextFactory.CreateAsync(new Uri(siteUrl))) {
        var listByGuid = context.Web.Lists.GetById<EmployeeTaskModel>(p => p.Title, p => p.Id, p => p.Items, p => p.Fields);
        var listByGuidItems = listByGuid.Items.AsRequested();

        var listByTitle = context.Web.Lists.GetByTitle<EmployeeTaskModel>(p => p.Title, p => p.Id, p => p.Items, p => p.Fields);
        var listByTitleItems = listByTitle.Items.AsRequested();

        var items = context.Web.GetItems<EmployeeTaskModel>(p => p.Title, p => p.Id, p => p.Items, p => p.Fields);

        var firstItem = items.First();
        firstItem.Title = "N/A";
        var updateItem = context.Web.UpdateItem(firstItem);

        Console.WriteLine();
    }
}