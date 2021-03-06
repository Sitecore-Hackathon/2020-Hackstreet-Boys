﻿using System.Collections.Generic;
using System.Linq;
using System;
using System.Web.Mvc;
using Hackathon.Feature.PageContent.Models;
using Hackathon.Feature.PageContent.Models.ViewModels;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using Sitecore.Data.Fields;
using Sitecore.Mvc.Presentation;
using Sitecore.Configuration;
using Hackathon.Foundation.Teams.Models;
using Hackathon.Foundation.Teams.Repositories;
using Sitecore.Links;
using Hackathon.Foundation.Account.Services;

namespace Hackathon.Feature.PageContent.Controllers
{
    public class PageContentController : Controller
    {
        private TeamsRepository _teamsRepository;
        private LoginUser _loginUser;

        public PageContentController()
        {
            _teamsRepository = new TeamsRepository();
            _loginUser = new LoginUser(); 
        }

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

            foreach (Item child in ds.Axes.GetDescendants())
            {
                if (child.TemplateID.ToString() == Hackathon.Foundation.Teams.Constants.Templates.Team.TemplateId)
                {
                    viewModel.Teams.Add(_teamsRepository.GetTeamFromItem(child));
                }
            }
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

        /// <summary>
        /// Choose between two choices. Depending on the choice, either placeholder choice1 or choice2 will appear in its place
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TwoChoice()
        {
            var ds = Sitecore.Context.Database.GetItem(RenderingContext.CurrentOrNull.Rendering.DataSource);
            if (ds == null || ds.TemplateID.ToString() != Constants.Templates.TwoChoice.TemplateId)
                return View();

            var viewModel = new TwoChoiceViewModel()
            {
                Title = ds[Constants.Templates.TwoChoice.Title],
                Choice1 = new ChoiceViewModel()
                {
                    Title = ds[Constants.Templates.TwoChoice.Choice1Title],
                    ChoiceKey = "/AssignTeam/Create"
                },
                Choice2 = new ChoiceViewModel()
                {
                    Title = ds[Constants.Templates.TwoChoice.Choice2Title],
                    ChoiceKey = "/AssignTeam/Join"
                }
            }; 
            return View(viewModel); 
        }

        /// <summary>
        /// View a team as an admin (will automatically grab the team of the logged in user)
        /// </summary>
        public ActionResult TeamAdmin()
        {
            // TODO: get the currently logged in user 
            string loggedInGitUser = _loginUser.GetCurrentUserGithub(); 

            if (string.IsNullOrEmpty(loggedInGitUser))
            {
                // redirect to login page
                return Redirect("/Login"); 
            }
            else
            {
                var userTeam = _teamsRepository.GetMyTeam(loggedInGitUser);

                if (userTeam == null)
                {
                    return Redirect("/AssignTeam");
                }

                // get team details 
                var userTeamBuilt = _teamsRepository.GetTeamFromItem(userTeam);
                var viewModel = new TeamAdminViewModel()
                {
                    Team = userTeamBuilt,
                    SecretJoinString = userTeamBuilt.Slug
                };
                return View(viewModel); 
            }
        }

        /// <summary>
        /// Just for viewing superficial information about a team (no admin) 
        /// </summary>

        public ActionResult TeamDetails(string team)
        {
            var teamMatch = _teamsRepository.GetMatchingTeamWithSlug(team);

            if (teamMatch != null)
            {
                var viewModel = new TeamDetailsViewModel()
                {
                    Team = teamMatch,
                    TeamMembers = new List<TeamMember>()  // TODO: get all members for this team
                };
                return View(viewModel);
            }

            // no matching team found :( 
            return View(); 
        }

        public ActionResult Menu()
        {
            var ds = Sitecore.Context.Database.GetItem(RenderingContext.CurrentOrNull.Rendering.DataSource);
            if (ds == null)
                return View();

            string loggedInGitUser = _loginUser.GetCurrentUserGithub();

            var viewModel = new MenuViewModel()
            {
                MenuLinks = ds.Children
                    .Where(c => c.TemplateID.ToString() == Constants.Templates.MenuLink.TemplateId)
                    .Select(c => {
                        var linkField = (LinkField)c.Fields[Constants.Templates.MenuLink.Link];
                        var linkUrl = string.Empty;
                        if (linkField.IsInternal)
                        {
                            linkUrl = LinkManager.GetItemUrl(linkField.TargetItem);
                        }
                        return new MenuLink()
                        {
                            Label = c[Constants.Templates.MenuLink.Label],
                            LinkUrl = linkUrl + (!string.IsNullOrEmpty(linkField.Anchor) ? "#" + linkField.Anchor : string.Empty),
                            ShowOnlyOnLogin = !string.IsNullOrEmpty(c[Constants.Templates.MenuLink.ShowOnlyWithLoggedInUser]) && c[Constants.Templates.MenuLink.ShowOnlyWithLoggedInUser].Equals("1"),
                            HideWithLoggedInUser = !string.IsNullOrEmpty(c[Constants.Templates.MenuLink.HideWithLoggedInUser]) && c[Constants.Templates.MenuLink.HideWithLoggedInUser].Equals("1")
                        };
                    })
                    .ToList(),
                UserLoggedIn = !string.IsNullOrEmpty(loggedInGitUser)
            };
            return View(viewModel); 
        } 

        public ActionResult StatCounter()
        {
            var ds = Sitecore.Context.Database.GetItem(RenderingContext.CurrentOrNull.Rendering.DataSource);
            if (ds == null)
                return View();

            var viewModel = new StatCounterViewModel()
            {
                StatBlocks = new List<StatBlock>()
            };
            viewModel.StatBlocks.Add(new StatBlock()
            {
                Label = ds["Users Text"],
                Value = _teamsRepository.GetTeamMemberCount()
            });
            viewModel.StatBlocks.Add(new StatBlock()
            {
                Label = ds["Countries Text"],
                Value = 39
            });
            viewModel.StatBlocks.Add(new StatBlock()
            {
                Label = ds["Teams Text"],
                Value = _teamsRepository.GetTeamsCount()
            });
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

