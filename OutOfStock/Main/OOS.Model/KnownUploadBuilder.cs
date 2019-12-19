using System;
using System.Collections.Generic;
using System.Linq;
using Magnum;
using OOSCommon;
using OOSCommon.Import;

namespace OOS.Model
{
    public class KnownUploadBuilder : IBuildKnownUpload
    {
        private IKnownUploadRepository repo;
        private ITranslateKnownUploadToProductStatusProjections translator;
        private KnownUpload upload;

        public KnownUploadBuilder(IKnownUploadRepository uploadRepository, ITranslateKnownUploadToProductStatusProjections translator)
        {
            repo = uploadRepository;
            this.translator = translator;
        }

        public void Build(DateTime date, IEnumerable<OOSKnownItemData> itemData, IEnumerable<OOSKnownVendorRegionMap> maps)
        {
            upload = Create(date, itemData, maps);
            AddUploadEvents();
            AddUpload();
        }

        private void AddUploadEvents()
        {
            //var projections = translator.Translate(upload);
            //foreach (var projection in projections)
            //{
            //    var exist = repo.ExistProductStatusProjection(projection);
            //    if (exist)
            //    {
            //        upload.AddEvent(ProductStatusMapper.ToProductStatusModifiedEvent(projection));
            //    }
            //    else
            //    {
            //        upload.AddEvent(ProductStatusMapper.ToProductStatusInsertedEvent(projection));
            //    }
            //}
        }

        private void AddUpload()
        {
            Guard.AgainstNull(upload);
            if (upload.VendorRegionMap.Length == 0 || upload.ItemData.Length == 0)
                return;

            if (repo.For(upload.UploadDate) == null)
            {
                repo.Insert(upload);
            }
            else
            {
                repo.Modify(upload);
            }
        }

        private KnownUpload Create(DateTime date, IEnumerable<OOSKnownItemData> itemData, IEnumerable<OOSKnownVendorRegionMap> maps)
        {
            var knownUpload = new KnownUpload(date);
            itemData.ToList().ForEach(knownUpload.AddItem);
            maps.ToList().ForEach(knownUpload.AddVendorRegion);
            return knownUpload;
        }

        public KnownUpload ToKnownUpload()
        {
            return upload;
        }
    }
}
