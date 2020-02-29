using System.Collections.Generic;
using System.Linq;
using System;
using System.Web.Mvc;
using Hackathon.Feature.PageContent.Models;
using Hackathon.Feature.PageContent.Models.ViewModels;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using Sitecore.Data.Fields;
using Sitecore.Mvc.Presentation;

namespace Hackathon.Feature.PageContent.Controllers
{
    public class PageContentController : Controller
    {

        public ActionResult Header()
        {

            return View();
        }


        public ActionResult Footer()
        {

            return View();
        }


        public ActionResult TeamList()
        {
            var ds = Sitecore.Context.Database.GetItem(RenderingContext.CurrentOrNull.Rendering.DataSource);
            if (ds == null || ds.TemplateID.ToString() != Constants.Templates.TeamList.TemplateId)
                return View();

            var viewModel = new TeamListViewModel()
            {
                Title = ds[Constants.Templates.TeamList.Title],
                Teams = new List<Team>()
            };

            foreach (Item child in ds.Children)
            {
                if (child.TemplateID.ToString() == Constants.Templates.Team.TemplateId)
                {
                    viewModel.Teams.Add(new Team()
                    {
                        Name = child[Constants.Templates.Team.TeamName],
                        ThumbnailUrl = GetMediaUrlFromId((ImageField)child.Fields[Constants.Templates.Team.Thumbnail])
                    });
                }
            }
            /*
            viewModel.Teams.Add(new Team() { Name = "Test tsdfasdfeaam *324*(%" });
            viewModel.Teams.Add(new Team() { Name = "Test team *(2*(%" });
            viewModel.Teams.Add(new Team() { Name = "Tesasdft team 324*(%" });
            viewModel.Teams.Add(new Team() { Name = "Testasdfaseam *(43(%" });
            viewModel.Teams.Add(new Team() { Name = "Test asTest team *(*(%" });
            viewModel.Teams.Add(new Team() { Name = "Test team *(234%" });
            viewModel.Teams.Add(new Team() { Name = "Test team *(*(%" });
            */ 
            return View(viewModel); 
        }


        public ActionResult FullHero()
        {
            var ds = Sitecore.Context.Database.GetItem(RenderingContext.CurrentOrNull.Rendering.DataSource);
            if (ds == null || ds.TemplateID.ToString() != Constants.Templates.FullHero.TemplateId)
                return View();

            var viewModel = new FullHeroViewModel()
            {
                MainText = ds[Constants.Templates.FullHero.MainText],
                SubText = ds[Constants.Templates.FullHero.SubText],
                ImageUrl = GetMediaUrlFromId((ImageField)ds.Fields[Constants.Templates.FullHero.Image])
            };
            return View(viewModel);
        }

        public ActionResult RichTextContent()
        {
            var ds = Sitecore.Context.Database.GetItem(RenderingContext.CurrentOrNull.Rendering.DataSource);
            if (ds == null || ds.TemplateID.ToString() != Constants.Templates.RichTextContent.TemplateId)
                return View(); 

            var viewModel = new RichTextContentViewModel()
            {
                TextContent = ds[Constants.Templates.RichTextContent.TextContent]
            };
            return View(viewModel); 
        }


        public ActionResult TwoChoice()
        {

            var viewModel = new TwoChoiceViewModel()
            {
                Title = "MAKE A DECISION JFKSDFSD",
                Choice1 = new ChoiceViewModel()
                {
                    Title = "CHOICE 1"
                },
                Choice2 = new ChoiceViewModel()
                {
                    Title = "CHOCIE @@@@22222@@@@"
                }
            };

            return View(viewModel); 
        }


        private string GetMediaUrlFromId(ImageField imgField)
        {
            if (imgField != null && imgField.MediaItem != null)
            {
                    return MediaManager.GetMediaUrl(imgField.MediaItem);
            }

            return string.Empty;
        }
    }
}

