using Icon.Esb.Schemas.Wfm.ContractTypes;
using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.Price.Constants;
using Icon.Infor.Listeners.Price.DataAccess.Models;
using Icon.Infor.Listeners.Price.DataAccess.Queries;
using Icon.Infor.Listeners.Price.Models;
using Icon.Infor.Listeners.Price.Services;
using Mammoth.Common.DataAccess.CommandQuery;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Infor.Listeners.Price.Decorators
{
    public class ReplaceValidationPriceServiceDecorator : IService<PriceModel>
    {
        private IService<PriceModel> service;
        private IQueryHandler<GetPricesByGpmIdsParameters, IEnumerable<DbPriceModel>> getPricesQuery;
        private IQueryHandler<GetDeletedPricesByGpmIdsParameters, IEnumerable<DeletedPriceModel>> getDeletedPricesQuery;

        public ReplaceValidationPriceServiceDecorator(
            IService<PriceModel> service,
            IQueryHandler<GetPricesByGpmIdsParameters, IEnumerable<DbPriceModel>> getPricesQuery,
            IQueryHandler<GetDeletedPricesByGpmIdsParameters, IEnumerable<DeletedPriceModel>> getDeletedPricesQuery)
        {
            this.service = service;
            this.getPricesQuery = getPricesQuery;
            this.getDeletedPricesQuery = getDeletedPricesQuery;
        }

        public void Process(IEnumerable<PriceModel> data, IEsbMessage message)
        {
            var allReplacePrices = data.Where(price => price.Action == ActionEnum.Replace);

            if (allReplacePrices.Any())
            {
                List<Guid> addPriceIds = allReplacePrices.Select(price => price.GpmId).ToList();
                List<Guid> deletePriceIds = allReplacePrices.Select(price => price.ReplacedGpmId.GetValueOrDefault()).ToList();
                var existingPricesInDb = GetExistingPrices(data);

                var addIdToIsValidDictionary = ValidateAddPrices(addPriceIds, existingPricesInDb);
                var deleteIdToIsValidDictionary = ValidateDeletePrices(deletePriceIds, existingPricesInDb);

                if (addIdToIsValidDictionary.Any(d => !d.Value))
                {
                    data.Join(existingPricesInDb, d => d.GpmId, pid => pid.GpmID, (d, pid) => d)
                        .Where(price => string.IsNullOrEmpty(price.ErrorCode))
                        .ToList()
                        .ForEach(p =>
                        {
                            p.ErrorCode = Errors.Codes.ReplacePricesAddError;
                            p.ErrorDetails = Errors.Details.ReplacePriceAddAlreadyExists;
                        });
                }

                if (deleteIdToIsValidDictionary.Any(d => !d.Value))
                {
                    // Check DeletedPrices table to see if prices were already deleted
                    List<Guid> priceIdsNotFound = allReplacePrices
                        .Select(arp => arp.ReplacedGpmId.GetValueOrDefault())
                        .Except(existingPricesInDb.Select(pdb => pdb.GpmID))
                        .ToList();

                    List<DeletedPriceModel> alreadyDeletedPrices = this.getDeletedPricesQuery
                        .Search(new GetDeletedPricesByGpmIdsParameters { PriceIds = priceIdsNotFound })
                        .ToList();

                    if (priceIdsNotFound.Count != alreadyDeletedPrices.Count)
                    {
                        IEnumerable<Guid> gpmIdsNotInDb = priceIdsNotFound.Except(alreadyDeletedPrices.Select(adp => adp.GpmID));

                        data.Join(gpmIdsNotInDb, d => d.ReplacedGpmId, gpmId => gpmId, (d, gpmId) => d)
                            .Where(price => string.IsNullOrEmpty(price.ErrorCode))
                            .ToList()
                            .ForEach(p =>
                            {
                                p.ErrorCode = Errors.Codes.ReplacePricesDeleteError;
                                p.ErrorDetails = Errors.Details.ReplacePriceDoesNotExist;
                            });
                    }
                }
            }

            this.service.Process(data, message);
        }

        /// <summary>
        /// Returns all the prices in the database that exist in the Price_XX table.
        /// This will combine both the prices that need to be deleted and the prices that need to be added
        /// so that there is only one database query.
        /// </summary>
        /// <param name="prices">Prices that need to be checked in Database</param>
        /// <returns>List of all prices where the GpmIDs and ReplaceGpmIDs exist in the database</returns>
        private List<DbPriceModel> GetExistingPrices(IEnumerable<PriceModel> prices)
        {
            var pricesToVerify = new List<DbPriceModel>();
            pricesToVerify.AddRange(prices.Select(p => new DbPriceModel { Region = p.Region, GpmID = p.GpmId })); // for the prices to Add
            pricesToVerify.AddRange(prices.Select(p => new DbPriceModel { Region = p.Region, GpmID = p.ReplacedGpmId.GetValueOrDefault() })); // for the prices to Delete

            List<DbPriceModel> existingPrices = this.getPricesQuery.Search(new GetPricesByGpmIdsParameters { Prices = pricesToVerify }).ToList();

            return existingPrices;
        }

        private Dictionary<Guid, bool> ValidateAddPrices(List<Guid> addPriceIds, List<DbPriceModel> existingPrices)
        {
            Dictionary<Guid, bool> idToIsValidDictionary = new Dictionary<Guid, bool>();

            foreach (var id in addPriceIds)
            {
                bool isValid = !existingPrices.Select(ep => ep.GpmID).Contains(id); // if existing price is not found then it's a valid id
                idToIsValidDictionary.Add(id, isValid);
            }

            return idToIsValidDictionary;
        }

        private Dictionary<Guid, bool> ValidateDeletePrices(List<Guid> deletePriceIds, List<DbPriceModel> existingPrices)
        {
            Dictionary<Guid, bool> idToIsValidDictionary = new Dictionary<Guid, bool>();

            foreach (var id in deletePriceIds)
            {
                bool isValid = existingPrices.Select(ep => ep.GpmID).Contains(id); // if existing price is found then it's a valid id
                idToIsValidDictionary.Add(id, isValid);
            }

            return idToIsValidDictionary;
        }
    }
}
