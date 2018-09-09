using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InsightBlog.Logic;
using InsightBlog.Model;
using Microsoft.AspNetCore.Mvc;

namespace InsightBlog.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InsightsController : ControllerBase
    {
        private readonly IInsightsService _insightService;
        public InsightsController(IInsightsService service)
        {
            _insightService = service;
        }

        // GET insights?pageLength=5&page=2
        [HttpGet]
        public ActionResult Get(int pageLength = 10, int page = 1)
        {
            InsightPage insightPage;

            insightPage = _insightService.GetInsightsPage(pageLength, page);

            return Ok(insightPage);
        }

        // POST insights
        [HttpPost]
        public ActionResult Post([FromBody] Insight insight)
        {
            _insightService.AddInsight(insight);

            return Ok(insight);
        }

        // PUT insights/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] string value)
        {
            return Ok();
        }

        // DELETE insights/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return Ok();
        }
    }
}
