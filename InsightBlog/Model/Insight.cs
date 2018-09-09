using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InsightBlog.Model
{
    public enum InsightType
    {
        News = 1,
        Comment = 2,
        MediaCoverage = 3,
        Event = 4
    } 
    public class Insight
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public long ID { get; set; }
        [JsonRequired]
        [StringLength(200)]
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonRequired]
        [JsonProperty(PropertyName = "type")]
        public InsightType Type { get; set; }
        [JsonRequired]
        [DataType(DataType.DateTime)]
        [JsonProperty(PropertyName = "datePublished")]
        public DateTime DatePublished { get; set; }
        [JsonRequired]
        [JsonProperty(PropertyName = "author")]
        public string Author { get; set; }
        [JsonRequired]
        [JsonProperty(PropertyName = "active")]
        public bool Active { get; set; }
        [JsonRequired]
        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
        [Url]
        [JsonProperty(PropertyName = "imageSource")]
        public string ImageSource { get; set; }
    }

    public class InsightPage
    {
        [JsonProperty(PropertyName = "insights")]
        public List<Insight> Insights { get; set; }
        [JsonProperty(PropertyName = "currentPage")]
        public int CurrentPage { get; set; }
        [JsonProperty(PropertyName = "lastPage")]
        public int LastPage { get; set; }
        [JsonProperty(PropertyName = "pageLength")]
        public int PageLength { get; set; }
    }
}
