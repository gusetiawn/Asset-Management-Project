using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AssetManagementClient.Base;
using AssetManagementAPI.ViewModel;
using AssetManagementAPI.Models;

namespace AssetManagementClient.Repository.Data
{
    public class AccountRepository : GeneralRepository<Account, string>
    {
        private readonly Address address;
        private readonly string request;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpClient httpClient;
        public AccountRepository(Address address, string request = "Accounts/") : base(address, request)
        {
            this.address = address;
            this.request = request;
            _contextAccessor = new HttpContextAccessor();
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(address.link)
            };
            //nambahin jwt token
        }
        //public async Task<List<RegisterVM>> GetUserData()
        //{
        //    List<RegisterVM> entities = new List<RegisterVM>();

        //    using (var response = await httpClient.GetAsync(request + "userdata"))
        //    {
        //        string apiResponse = await response.Content.ReadAsStringAsync();
        //        entities = JsonConvert.DeserializeObject<List<RegisterVM>>(apiResponse);
        //    }
        //    return entities;
        //}

    }
}
