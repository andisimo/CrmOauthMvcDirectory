using CrmOauthMvc.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CrmOauthMvc.Controllers
{
    public class AccountsController : Controller
    {
        private static AuthenticationResult _AuthenticationResult;
        private static AccountsModel _jAccounts;

        // GET: Accounts
        public ActionResult Index()
        {
            // TODO Substitute your correct CRM root service address, 
            string resource = "https://demogold.crm.dynamics.com";

            // TODO Substitute your app registration values that can be obtained after you
            // register the app in Active Directory on the Microsoft Azure portal.
            string clientId = "c447dac5-f9bc-4ca2-bc0f-4d8c649583f9";
            string redirectUrl = "https://localhost:44300/";


            // Authenticate the registered application with Azure Active Directory.
            AuthenticationContext authContext = new AuthenticationContext("https://login.windows.net/common", false);
            _AuthenticationResult = authContext.AcquireToken(resource, clientId, new Uri(redirectUrl));

            //using this page: https://github.com/jlattimer/CrmWebApiCSharp/blob/master/CrmWebApiCSharp/Program.cs
            Task.WaitAll(Task.Run(async () => await GetAccounts()));

            return View(_jAccounts);
        }

        private static async Task GetAccounts()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.Timeout = new TimeSpan(0, 2, 0);  // 2 minutes
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _AuthenticationResult.AccessToken);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await httpClient.GetAsync("https://demogold.crm.dynamics.com/api/data/v8.0/accounts?$select=name&$top=3");

                _jAccounts = JsonConvert.DeserializeObject<AccountsModel>(response.ToString());
            }
        }

        // GET: Accounts/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Accounts/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Accounts/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
