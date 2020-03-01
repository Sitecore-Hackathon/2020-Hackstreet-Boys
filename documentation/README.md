# Documentation

**Sitecore Hackathon website**

## Category:
Sitecore Hackathon website

## Submission:
**Sitecore Hackathon website** 
> Easier teams and members creation with out of the box integration with Github REST API.

We build a Sitecore hackathon website with functionality that makes it easy for the community to participate and join teams. The website will automatically create your github team under Sitecore Hackathon organization, with a repository named after your team and with Github invitation sent out to all of the team members to join the repository.

We hope the organizors of Sitecore hackathon love this submission as we know it will make life easier for them.

## Pre-requisites:

- Sitecore 9.3 XP0
- Github Personal Access token with all permissions assigned
- Github Organization name: The website will use that to build the teams under it.
- Github App : with "Administration" permission assigned for both Repository and Organization

## Installation:

- Install **9.3.0 rev. 003498 (Setup XP0 Developer Workstation rev. 1.1.1-r4)** using SIA.
  - Installation/solution prefix: hackstreetsc
  - Url should be https://hackstreetsc.dev.local/
- Install Demo Package from Sc.Package folder [Sitecore Hackathon - Hackstreet-Boys-1.0.zip](https://github.com/Sitecore-Hackathon/2020-Hackstreet-Boys/raw/master/sc.package/Sitecore%20Hackathon%20-%20Hackstreet-Boys-1.0.zip " Sitecore Hackathon - Hackstreet-Boys-1.0.zip")  and then Select **Overwrite all**
- Update the settings inside \App_Config\Include\Foundation\Foundation.Teams.config
  - hackathon.GithubPersonalAccessKey: Your github access account that has all scopes permissions
  - hackathon.GithubOwnerAccount: github username for the owner of the Sitecore Hackathon organization
  - hackathon.GitHubAppName: Github app name created for thee website
  - hackathon.OrganizationName: Hackathon organization name.
- Publish Site
- Rebuild Indexs

## Usage:

The provided package will include demo items that we will explain how it works and how it can be used in other areas

-  Open home page and navigate to /register page, and fill out the registration form.
-  Navigate to login page /login and login with your email/pwd .
-  To create your team, go to /AssignTeam/Create and fill out the form, once you are done, your team will be created in github, you can go to the organization page in github and verify that both team and repo are created. Also in Sitecore content tree, a new item will be created in /sitecore/content/global/Teams/* . The item will have mapping details that represent the team in github
-  You have the option to join other teams, by going to /AssignTeam/Join page and fill out the form, make sure that you have the correct team name (in github it's called slug name) and a valid github username
- Once joined, you will be added to github team repo, and in Sitecore content tree a new item will be created inside /sitecore/content/global/Members/*
- After joining or creating a team you will be able to see your team members from within the website after you log in.

## Video:

https://www.youtube.com/watch?v=PB-pWRSRspk

[![Watch our video on Youtube](https://img.youtube.com/vi/PB-pWRSRspk/0.jpg)](https://www.youtube.com/watch?v=PB-pWRSRspk)

