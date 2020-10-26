using System;
using System.Collections.Generic;
using System.Text;

namespace Gyan
{
    public class Article
    {
        public string Id { get; set; }
        public string Uri { get; set; }
        public DateTime Added { get; set; }
        public DateTime Processed { get; set; }
    }
}
