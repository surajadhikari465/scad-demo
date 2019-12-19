using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using OOS.Model.Feed;

namespace OOS.Model
{
    public class StoreFeedInfoDecoder
    {
        private StoreFeedInfoDecoder()
        {}

        public static List<StoreFeed> Decode(IEnumerable<StoreFeed> feeds)
        {
            return feeds.Select(DecodeInfo).ToList();
        }

        private static StoreFeed DecodeInfo(StoreFeed feed)
        {
            var feedItem = CreateStoreFeed(feed);
            feedItem.name = HttpUtility.UrlDecode(feed.name);
            feedItem.address = HttpUtility.UrlDecode(feed.address);
            feedItem.address2 = HttpUtility.UrlDecode(feed.address2);
            feedItem.hours = HttpUtility.UrlDecode(feed.hours);
            feedItem.modified_dt = HttpUtility.UrlDecode(feed.modified_dt);
            feedItem.state = HttpUtility.UrlDecode(feed.state);
            feedItem.city = HttpUtility.UrlDecode(feed.city);
            feedItem.country = HttpUtility.UrlDecode(feed.country);
            feedItem.miscinfo = HttpUtility.UrlDecode(feed.miscinfo);
            feedItem.facility = HttpUtility.UrlDecode(feed.facility);
            return feedItem;
        }

        private static StoreFeed CreateStoreFeed(StoreFeed feed)
        {
            var feedItem = new StoreFeed
            {
                address = feed.address,
                address2 = feed.address2,
                blogid = feed.blogid,
                city = feed.city,
                country = feed.country,
                fax = feed.fax,
                folder = feed.folder,
                hours = feed.hours,
                image = feed.image,
                lat = feed.lat,
                lon = feed.lon,
                mapblurb = feed.mapblurb,
                miscinfo = feed.miscinfo,
                modified_dt = feed.modified_dt,
                name = feed.name,
                phone = feed.phone,
                region = feed.region,
                state = feed.state,
                status = feed.status,
                store_keys = feed.store_keys,
                tlc = feed.tlc,
                zipchar = feed.zipchar,
                facility = feed.facility
            };
            return feedItem;
        }

    }
}
