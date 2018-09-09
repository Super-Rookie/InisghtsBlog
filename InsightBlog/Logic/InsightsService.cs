using InsightBlog.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsightBlog.Model;
using Microsoft.EntityFrameworkCore;

namespace InsightBlog.Logic
{
    public class InsightsService : IInsightsService, IDisposable
    {
        private InsightsContext _context;

        public InsightsService(string databaseMemoryName = "InsightsDatabase")
        {
            var options = new DbContextOptionsBuilder<InsightsContext>()
                .UseInMemoryDatabase(databaseName: databaseMemoryName)
                .Options;

            _context = new InsightsContext(options);
        }

        public void AddInsight(Insight insight)
        {
            _context.Insights.Add(insight);
            _context.SaveChanges();
        }

        public InsightPage GetInsightsPage(int pageLength, int page)
        {
            // Number of insights to skip, make sure values are valid otherwise use valid numbers
            var totalInsights = _context.Insights.Count();
            // Calculate final page
            var lastPage = totalInsights / pageLength;
            if (totalInsights % pageLength != 0)
            {
                lastPage++;
            }
            // If max is invalid return all
            if (pageLength < 1)
            {
                pageLength = totalInsights;
            }
            // Check page values are valid 
            var actualPage = page;
            if (page < 1)
            {
                actualPage = 1;
            }
            else if (page > lastPage)
            {
                actualPage = lastPage;
            }
            // Calculate how many insights to skip
            var skip = (actualPage - 1) * pageLength;

            List<Insight> insights;

            insights = _context.Insights
                    .Where(b => b.Active && b.DatePublished <= DateTime.UtcNow)
                    .OrderByDescending(b => b.DatePublished)
                    .Skip(skip)
                    .Take(pageLength)
                    .ToList();

            var insightPage = new InsightPage()
            {
                Insights = insights,
                CurrentPage = actualPage,
                LastPage = lastPage,
                PageLength = pageLength
            };
            return insightPage;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
