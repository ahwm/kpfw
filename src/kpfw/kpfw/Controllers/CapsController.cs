﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using kpfw.DataModels;
using kpfw.Models;
using kpfw.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace kpfw.Controllers
{
    public class CapsController : Controller
    {
        private DataContext _context;
        private readonly KpfwSettings settings;
        public CapsController(DataContext context, IConfiguration configuration)
        {
            settings = configuration.GetSection("Kpfw").Get<KpfwSettings>();
            _context = context;
        }
        public IActionResult Index()
        {
            return View("EpListing");
        }

        public IActionResult ViewEpisode()
        {
            bool isValid = false;
            string episodeUrl = RouteData.Values["Episode"].ToString();
            string epUrl = episodeUrl;
            if (episodeUrl.ToLower().Contains("_") && !episodeUrl.ToLower().Contains("veiwpic"))
            {
                episodeUrl = episodeUrl.ToLower().Replace(".php", "").Replace("_", "-");
            }
            EpisodeTitle ep = EpCapsList.S1List.Where(p => p.UrlLabel == episodeUrl).SingleOrDefault();
            if (ep != null && !String.IsNullOrWhiteSpace(ep.Title))
                isValid = true;
            if (ep == null)
            {
                ep = EpCapsList.S2List.Where(p => p.UrlLabel == episodeUrl).SingleOrDefault();
                if (!String.IsNullOrWhiteSpace(ep?.Title))
                    isValid = true;
            }
            if (ep == null)
            {
                ep = EpCapsList.S3List.Where(p => p.UrlLabel == episodeUrl).SingleOrDefault();
                if (!String.IsNullOrWhiteSpace(ep?.Title))
                    isValid = true;
            }
            if (ep == null)
            {
                ep = EpCapsList.S4List.Where(p => p.UrlLabel == episodeUrl).SingleOrDefault();
                if (!String.IsNullOrWhiteSpace(ep?.Title))
                    isValid = true;
            }
            if (ep == null)
            {
                ep = EpCapsList.MovieList.Where(p => p.UrlLabel == episodeUrl).SingleOrDefault();
                if (!String.IsNullOrWhiteSpace(ep?.Title))
                    isValid = true;
            }
            if (ep == null)
            {
                ep = EpCapsList.MiscList.Where(p => p.UrlLabel == episodeUrl).SingleOrDefault();
                if (!String.IsNullOrWhiteSpace(ep?.Title))
                    isValid = true;
            }
            if (RedirectTable.CapsList.ContainsKey(epUrl))
            {
                if (RedirectTable.CapsList[epUrl].StartsWith("/"))
                    return RedirectPermanent(RedirectTable.CapsList[epUrl]);
                else
                    return RedirectPermanent($"/Caps/{RedirectTable.CapsList[epUrl]}");
            }
            if (!isValid)
            {
                using (FileStream fs = new FileStream(AppDomain.CurrentDomain.MapPath("~/App_Data/Urls.txt"), FileMode.Append))
                {
                    byte[] url = System.Text.Encoding.UTF8.GetBytes(Request.GetEncodedPathAndQuery() + "\r\n");
                    fs.Write(url, 0, url.Length);
                }
                return Redirect("/Caps");
            }
            if (isValid && epUrl.ToLower().Contains("_"))
            {
                return RedirectPermanent("/Caps/" + episodeUrl);
            }

            var epData = _context.Episodes.SingleOrDefault(x => x.UrlLabel == epUrl);
            if (epData != null)
                ep.Description = epData.Description;

            return View("EpisodeCaps", ep);
        }
    }
}