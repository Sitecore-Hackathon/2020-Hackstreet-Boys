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
- Install Demo Package from Sc.Package folder [Sitecore React Trackable Links Module and Demo-1.0.zip](https://github.com/Sitecore-Hackathon/2019-Sitecorps/raw/master/sc.package/Sitecore%20React%20Trackable%20Links%20Module%20and%20Demo-1.0.zip "Sitecore React Trackable Links Module and Demo-1.0.zip")  and then Select **Overwrite all**
- Update the settings inside \App_Config\Include\Foundation\Foundation.Teams.config
  - hackathon.GithubPersonalAccessKey: Your github access account that has all scopes permissions
  - hackathon.GithubOwnerAccount: github username for the owner of the Sitecore Hackathon organization
  - hackathon.GitHubAppName: Github app name created for thee website
  - hackathon.OrganizationName: Hackathon organization name.
- Publish Site
- Rebuild Index (need master and core indexed for module to fully work)
