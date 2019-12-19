﻿using System.Collections.Generic;
using Magnum.TestFramework;
using MassTransit;
using OOS.Model.Handlers;
using OOSCommon.Import;
using OutOfStock.Messages;
using Rhino.Mocks;

namespace OOS.Model.UnitTests
{
    [Scenario]
    public class When_uploading_an_already_uploaded_known_upload : Given_a_known_upload
    {
        private IKnownUploadRepository uploadRepo;
        private ITranslateKnownUploadToProductStatusProjections translator;

        protected override KnownUploadCommand Command()
        {
            return new KnownUploadCommand 
                { 
                    UploadDate = date, 
                    Items = ItemDataMapper.ToKnownUploadItems(itemData), 
                    VendorRegionMaps = VendorRegionMapper.ToKnownUploadVendorRegions(vendorMap) 
                };
        }

        protected override Handles<KnownUploadCommand> OnHandler()
        {
            uploadRepo = MockRepository.GenerateMock<IKnownUploadRepository>();
            translator = MockRepository.GenerateMock<ITranslateKnownUploadToProductStatusProjections>();
            builder = new KnownUploadBuilder(uploadRepo, translator);
            var bus = MockRepository.GenerateStub<IServiceBus>();
            return new UploadKnownUploadCommandHandler(builder, bus);
        }

        protected override void When()
        {
            var upload = new KnownUpload(date);
            uploadRepo.Expect(p => p.For(date)).Return(upload).Repeat.Once();
            uploadRepo.Expect(p => p.Modify(upload)).Repeat.Once();

            var projection = new ProductStatus(Region, VendorKey, Vin, Upc);
            var statuses = new List<ProductStatus> { projection };
            translator.Expect(p => p.Translate(upload)).Return(statuses).Repeat.Once();

            uploadRepo.Expect(p => p.ExistProductStatusProjection(projection)).Return(true).Repeat.Once();
        }

        [Then]
        public void Only_one_event_is_received()
        {
            uploadRepo.VerifyAllExpectations();
            events.Count.ShouldBeEqualTo(1);
        }

        [Then]
        public void The_known_upload_is_modified()
        {
            events[0].ShouldBeAnInstanceOf<ProductStatusModifiedEvent>();
        }

    }
}
