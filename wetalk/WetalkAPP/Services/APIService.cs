using WetalkAPP.Models;
using WetalkAPP.Models.APIRequests;
using WetalkAPP.Models.APIResponses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WetalkAPP.Services
{
    public class APIService
    {
        #region constructor
        static HttpClient _client;
        public static LoginResponse user;

        public void InitializeAPIService()
        {
            _client = new HttpClient();
        }
        #endregion


        #region auth
        // login
        public async Task Login(string username, string password)
        {
            // build request params from configs
            string request = $"{ConfigurationManager.AppSettings["API.Address"]}/users/login";
            var stringContent = new StringContent(JsonConvert.SerializeObject(
                new AuthenticateUserRequest()
                {
                    Username = username,
                    Password = password
                }),
                    Encoding.UTF8, "application/json");

            HttpResponseMessage result = await _client.PostAsync(request, stringContent);

            if (result.IsSuccessStatusCode)
            {
                // get typed response
                string resultContent = await result.Content.ReadAsStringAsync();
                LoginResponse LoginResponseResult = JsonConvert.DeserializeObject<LoginResponse>(resultContent);

                // set the default token to auth header
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LoginResponseResult.token);
                user = LoginResponseResult;
            }
            else
            {
                if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception("The user doesn't exist or the password is incorrect!");
                }
                throw new Exception(result.ReasonPhrase);
            }
        }

        // register a new user
        public async Task Register(string firstName, string lastName, string username, string password)
        {
            // build request params from configs
            string request = $"{ConfigurationManager.AppSettings["API.Address"]}/users/register";
            var stringContent = new StringContent(JsonConvert.SerializeObject(
                new RegisterUserRequest()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Username = username,
                    Password = password
                }),
                    Encoding.UTF8, "application/json");

            HttpResponseMessage result = await _client.PostAsync(request, stringContent);

            if (result.IsSuccessStatusCode)
            {
                // get typed response
                string resultContent = await result.Content.ReadAsStringAsync();
                LoginResponse LoginResponseResult = JsonConvert.DeserializeObject<LoginResponse>(resultContent);

                // set the default token to auth header
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LoginResponseResult.token);
                user = LoginResponseResult;
            }
            else
            {
                throw new Exception($"{result.StatusCode} - Request wasn't successfull");
            }
        }
        #endregion

        #region chats
        public async Task<List<ChatResponse>> GetUserChats()
        {
            // build request params from configs
            string request = $"{ConfigurationManager.AppSettings["API.Address"]}/chats";
            HttpResponseMessage result = await _client.GetAsync(request);

            if (result.IsSuccessStatusCode)
            {
                string resultContent = await result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ChatResponse>>(resultContent);
            }
            else
            {
            }
            return new List<ChatResponse>();
        }
        public async Task SendChatMessage(int chatID, string message)
        {
            // build request params from configs
            string request = $"{ConfigurationManager.AppSettings["API.Address"]}/chats/{chatID}/message";

            var stringContent = new StringContent(JsonConvert.SerializeObject(
              new CreateChatMessageModel()
              {
                  Message = message
              }), Encoding.UTF8, "application/json");


            HttpResponseMessage result = await _client.PostAsync(request, stringContent);

            if (!result.IsSuccessStatusCode)
            {
            }
        }

        public async Task MarkMessageAsRead(int messageID)
        {
            // build request params from configs
            string request = $"{ConfigurationManager.AppSettings["API.Address"]}/chats/message/{messageID}/read";
            var stringContent = new StringContent("", Encoding.UTF8, "application/json");

            HttpResponseMessage result = await _client.PutAsync(request, stringContent);

            if (!result.IsSuccessStatusCode)
            {
            }
        }

        #endregion
    }
}