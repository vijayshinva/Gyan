using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gyan
{
    internal class GyanConsoleUi
    {
        internal static async Task ListAsync(ILogger logger, IConfigurationRoot configuration, string filter = "*")
        {
            using var gyanContext = new GyanContext(logger, configuration);
            gyanContext.Database.EnsureCreated();
            if (filter == "*")
            {
                foreach (var article in await gyanContext.Articles.ToArrayAsync())
                {
                    Console.WriteLine(article.Uri);
                }
            }
            else
            {
                foreach (var article in await gyanContext.Articles.Where(a => a.Uri.Contains(filter)).ToArrayAsync())
                {
                    Console.WriteLine(article.Uri);
                }
            }
        }

        internal static async Task RemoveAsync(ILogger logger, IConfigurationRoot configuration, Uri uri)
        {
            using var gyanContext = new GyanContext(logger, configuration);
            gyanContext.Database.EnsureCreated();
            var article = await gyanContext.Articles.FirstOrDefaultAsync(a => a.Id == uri.AbsoluteUri.ToSha256());
            if (article != null)
            {
                gyanContext.Remove(article);
            }
            await gyanContext.SaveChangesAsync();
        }

        internal static async Task AddAsync(ILogger logger, IConfigurationRoot configuration, Uri uri)
        {
            using var gyanContext = new GyanContext(logger, configuration);
            gyanContext.Database.EnsureCreated();
            await gyanContext.AddAsync(new Article
            {
                Id = uri.AbsoluteUri.ToSha256(),
                Uri = uri.AbsoluteUri,
                Added = DateTime.UtcNow
            });
            await gyanContext.SaveChangesAsync();
        }

        internal static Task AnalyzeAsync(ILogger logger, IConfigurationRoot configuration)
        {
            using var gyanContext = new GyanContext(logger, configuration);
            gyanContext.Database.EnsureCreated();
            return Task.CompletedTask;
        }
    }
}