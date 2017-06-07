using Digipolis.Iod_abs.Dossier.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Digipolis.Iod_abs.Dossier.Model.Models;
using System.Threading.Tasks;
using Digipolis.Iod_abs.Dossier.DataProvider.Interfaces;
using Narato.Common.Exceptions;
using Narato.Common.Models;

namespace Digipolis.Iod_abs.Dossier.Domain.Managers
{
    public class DossierManager : IDossierManager
    {
        private readonly IDossierDataProvider _dossierDataProvider;
        private readonly IDossierTypeConfigDataProvider _dossierTypeConfigDataProvider;
        private readonly ITasksApiClient _tasksApiClient;

        public DossierManager(IDossierDataProvider dossierDataProvider, IDossierTypeConfigDataProvider dossierTypeConfigDataProvider, ITasksApiClient tasksApiClient)
        {
            _dossierDataProvider = dossierDataProvider;
            _dossierTypeConfigDataProvider = dossierTypeConfigDataProvider;
            _tasksApiClient = tasksApiClient;
        }

        public IEnumerable<GenericDossier> GetAllDossiers()
        {
            return _dossierDataProvider.GetDossiers();
        }

        public GenericDossier GetCompanyAssetDossierById(Guid id)
        {
            return _dossierDataProvider.GetDossierById(id);
        }

        public GenericDossier InsertDossier(GenericDossier dossier, Guid dossierTypeId)
        {
            var config = _dossierTypeConfigDataProvider.GetDossierTypeConfigByDossierTypeId(dossierTypeId);
            var dossierNr = config.DossierNrStructure;
            var count = _dossierDataProvider.CountDossiers().ToString();
            dossier.DossierNr = dossierNr.Replace("XXXX", count.PadLeft(4, '0'));
            var insertedDossier = _dossierDataProvider.InsertDossier(dossier, dossierTypeId);
            foreach (var processConfig in config.Processes)
            {
                _tasksApiClient.StartProcessForDossierAndProcessDefinitionId(insertedDossier, processConfig.ProcessId);
            }
            
            return insertedDossier;
        }

        public GenericDossier UpdateDossier(Guid id, GenericDossier dossier)
        {
            if (dossier.DataObjectId == null)
            {
                throw new ValidationException(FeedbackItem.CreateValidationErrorFeedbackItem("dataObjectId is not filled in"));
            }
            if (id != dossier.DataObjectId)
            {
                throw new ValidationException(FeedbackItem.CreateValidationErrorFeedbackItem("dataObjectId is not filled in"));
            }
            return _dossierDataProvider.UpdateDossier(dossier);
        }
    }
}
