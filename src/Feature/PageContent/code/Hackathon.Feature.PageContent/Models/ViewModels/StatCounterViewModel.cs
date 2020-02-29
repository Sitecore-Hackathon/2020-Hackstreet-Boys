using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hackathon.Feature.PageContent.Models.ViewModels
{
    public class StatCounterViewModel
    {
        public List<StatBlock> StatBlocks { get; set; }
    }

    public class StatBlock
    {
        public string Label { get; set; }
        public int Value { get; set; }
    }
}