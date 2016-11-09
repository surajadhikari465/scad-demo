using Icon.Esb.EwicAplListener.Common.Models;
using Icon.Esb.EwicAplListener.ExclusionGenerators;
using Icon.Esb.EwicAplListener.MappingGenerators;
using Icon.Logging;
using System;

namespace Icon.Esb.EwicAplListener.NewAplProcessors
{
    public class AutoMappingAndExclusionProcessor : INewAplProcessor
    {
        private ILogger<AutoMappingAndExclusionProcessor> logger;
        private IExclusionGenerator exclusionGenerator;
        private IMappingGenerator mappingGenerator;

        public AutoMappingAndExclusionProcessor(
            ILogger<AutoMappingAndExclusionProcessor> logger,
            IExclusionGenerator exclusionGenerator,
            IMappingGenerator mappingGenerator)
        {
            this.logger = logger;
            this.exclusionGenerator = exclusionGenerator;
            this.mappingGenerator = mappingGenerator;
        }

        public void ApplyMappings(AuthorizedProductListModel model)
        {
            foreach (var item in model.Items)
            {
                try
                {
                    mappingGenerator.GenerateMappings(item);
                }
                catch (Exception ex)
                {
                    string errorMessage = String.Format("An error occurred while generating the auto-mappings.  AgencyID: {0}, APL Scan Code: {1}.", item.AgencyId, item.ScanCode);
                    var exception = new Exception(errorMessage, ex);
                    throw exception;
                }
            }
        }

        public void ApplyExclusions(AuthorizedProductListModel model)
        {
            foreach (var item in model.Items)
            {
                try
                {
                    exclusionGenerator.GenerateExclusions(item);
                }
                catch (Exception ex)
                {
                    string errorMessage = String.Format("An error occurred while generating the auto-exclusions.  AgencyID: {0}, APL Scan Code: {1}.", item.AgencyId, item.ScanCode);
                    var exception = new Exception(errorMessage, ex);
                    throw exception;
                }    
            }
        }
    }
}
