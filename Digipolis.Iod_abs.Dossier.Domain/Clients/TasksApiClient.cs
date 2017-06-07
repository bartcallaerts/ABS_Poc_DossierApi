using Digipolis.Iod_abs.Dossier.Domain.Interfaces;
using Digipolis.Iod_abs.Dossier.Model.Models;
using Narato.Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Digipolis.Iod_abs.Dossier.Domain.Clients
{
    public class TasksApiClient : ITasksApiClient
    {
        private readonly HttpClient _client;

        public TasksApiClient(HttpClient httpClient)
        {
            _client = httpClient;
        }

        public bool StartProcessForDossierAndProcessDefinitionId(GenericDossier dossier, string processDefinitionId)
        {
            var structuredContent = new
            {
                ProcessDefinitionId = processDefinitionId,
                DossierId = dossier.DataObjectId
            };

            var content = new StringContent(structuredContent.ToJson());

            var request = new HttpRequestMessage(HttpMethod.Post, _client.BaseAddress + "api/processinstances");

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Content = content;

            using (var response = _client.SendAsync(request, HttpCompletionOption.ResponseContentRead).Result)
            {
                return response.IsSuccessStatusCode;
            }
        }
    }
}
