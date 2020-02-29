using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hackathon.Feature.PageContent.Models.ViewModels
{
    public class TwoChoiceViewModel
    {
        public string Title { get; set; }
        public ChoiceViewModel Choice1 { get; set; }
        public ChoiceViewModel Choice2 { get; set; }
        // the choice key that was chosen on the end
        public string ChoiceKey { get; set; } 
    }

    public class ChoiceViewModel
    {
        public string Title { get; set; }
        public string ChoiceKey { get; set; }
    }
}