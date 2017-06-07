using Digipolis.Iod_abs.Dossier.Domain.Interfaces;
using Digipolis.Iod_abs.Dossier.Model.Models;
using Microsoft.AspNetCore.Mvc;
using Narato.Common;
using Narato.Common.Factory;
using System;
using System.Threading.Tasks;

namespace Digipolis.Iod_abs.Dossier.Api.Controllers
{
    [Route("api/[controller]")]
    public class DossiersController : Controller
    {
        private readonly IResponseFactory _responseFactory;
        private readonly IDossierManager _dossierManager;

        public DossiersController(IResponseFactory responseFactory, IDossierManager dossierManager)
        {
            _responseFactory = responseFactory;
            _dossierManager = dossierManager;
        }

        [HttpGet]
        public IActionResult GetAllDossiers()
        {
            return _responseFactory.CreateGetResponse(() => _dossierManager.GetAllDossiers(), this.GetRequestUri());
        }

        [HttpGet("{id}")]
        public IActionResult GetDossierById(Guid id)
        {
            return _responseFactory.CreateGetResponse(() => _dossierManager.GetCompanyAssetDossierById(id), this.GetRequestUri());
        }

        [HttpPost]
        public IActionResult PostDossier([FromBody] GenericDossier dossier, [FromQuery] Guid dossierTypeId)
        {
            return _responseFactory.CreatePostResponse(() => _dossierManager.InsertDossier(dossier, dossierTypeId), this.GetRequestUri());
        }

        [HttpPut("{id}")]
        public IActionResult PutDossier(Guid id, [FromBody] GenericDossier dossier)
        {
            return _responseFactory.CreatePutResponse(() => _dossierManager.UpdateDossier(id, dossier), this.GetRequestUri());
        }
    }
}
