using InsightBlog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsightBlog.Logic
{
    public interface IInsightsService
    {
        InsightPage GetInsightsPage(int pageLength, int page);
        void AddInsight(Insight insight);
    }
}
