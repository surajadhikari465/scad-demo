using Icon.Dashboard.CommonDatabaseAccess;
using Icon.Dashboard.Mvc.Enums;
using Icon.Dashboard.Mvc.Models;
using Icon.Dashboard.Mvc.Models.CustomConfigElements;
using Icon.Dashboard.Mvc.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Dashboard.Mvc.UnitTests
{
    public static class CustomAsserts
    {
        public static void EnvironmentModelsEqual(EnvironmentModel expected, EnvironmentModel actual)
        {
            Assert.AreEqual(expected.Name, actual.Name, $"{nameof(expected.Name)} property did not match for {expected.Name} environment");
            Assert.AreEqual(expected.IsEnabled, actual.IsEnabled, $"{nameof(expected.IsEnabled)} property did not match for {expected.Name} environment");
            Assert.AreEqual(expected.DashboardUrl, actual.DashboardUrl, $"{nameof(expected.DashboardUrl)} property did not match for {expected.Name} environment");
            Assert.AreEqual(expected.WebServer, actual.WebServer, $"{nameof(expected.WebServer)} property did not match for {expected.Name} environment");
            ListsAreEqual(expected.AppServers, actual.AppServers);
            Assert.AreEqual(expected.IconWebUrl, actual.IconWebUrl, $"{nameof(expected.IconWebUrl)} property did not match for {expected.Name} environment");
            ListsAreEqual(expected.TibcoAdminUrls, actual.TibcoAdminUrls);
            Assert.AreEqual(expected.IconDatabaseServer, actual.IconDatabaseServer, $"{nameof(expected.IconDatabaseServer)} property did not match for {expected.Name} environment");
            Assert.AreEqual(expected.IconDatabaseName, actual.IconDatabaseName, $"{nameof(expected.IconDatabaseName)} property did not match for {expected.Name} environment");
            Assert.AreEqual(expected.MammothDatabaseServer, actual.MammothDatabaseServer, $"{nameof(expected.Name)} property did not match for {expected.Name} environment");
            Assert.AreEqual(expected.MammothDatabaseName, expected.MammothDatabaseName, $"{nameof(expected.MammothDatabaseName)} property did not match for {expected.Name} environment");
            ListsAreEqual(expected.IrmaDatabaseServers, actual.IrmaDatabaseServers);
            Assert.AreEqual(expected.IrmaDatabaseName, actual.IrmaDatabaseName, $"{nameof(expected.IrmaDatabaseName)} property did not match for {expected.Name} environment");
        }

        public static void EnvironmentElementsEqual(EnvironmentElement expected, EnvironmentElement actual)
        {
            Assert.AreEqual(expected.Name, actual.Name, $"{nameof(expected.Name)} property did not match for {expected.Name} esb environment element");
            Assert.AreEqual(expected.IsEnabled, actual.IsEnabled, $"{nameof(expected.IsEnabled)} property did not match for {expected.Name} esb environment element");
            Assert.AreEqual(expected.DashboardUrl, actual.DashboardUrl, $"{nameof(expected.DashboardUrl)} property did not match for {expected.Name} esb environment element");
            Assert.AreEqual(expected.WebServer, actual.WebServer, $"{nameof(expected.WebServer)} property did not match for {expected.Name} esb environment element");
            Assert.AreEqual(expected.AppServers, actual.AppServers, $"{nameof(expected.AppServers)} property did not match for {expected.Name} esb environment element");
            Assert.AreEqual(expected.IconWebUrl, actual.IconWebUrl, $"{nameof(expected.IconWebUrl)} property did not match for {expected.Name} esb environment element");
            Assert.AreEqual(expected.TibcoAdminUrls, actual.TibcoAdminUrls, $"{nameof(expected.TibcoAdminUrls)} property did not match for {expected.Name} esb environment element");
            Assert.AreEqual(expected.IconDatabaseServers, actual.IconDatabaseServers, $"{nameof(expected.IconDatabaseServers)} property did not match for {expected.Name} esb environment element");
            Assert.AreEqual(expected.IconDatabaseCatalogName, actual.IconDatabaseCatalogName, $"{nameof(expected.IconDatabaseCatalogName)} property did not match for {expected.Name} esb environment element");
            Assert.AreEqual(expected.MammothDatabaseServers, actual.MammothDatabaseServers, $"{nameof(expected.MammothDatabaseServers)} property did not match for {expected.Name} esb environment element");
            Assert.AreEqual(expected.MammothDatabaseCatalogName, expected.MammothDatabaseCatalogName, $"{nameof(expected.MammothDatabaseCatalogName)} property did not match for {expected.Name} esb environment element");
            Assert.AreEqual(expected.IrmaDatabaseServers, actual.IrmaDatabaseServers, $"{nameof(expected.IrmaDatabaseServers)} property did not match for {expected.Name} esb environment element");
            Assert.AreEqual(expected.IrmaDatabaseCatalogName, actual.IrmaDatabaseCatalogName, $"{nameof(expected.IrmaDatabaseCatalogName)} property did not match for {expected.Name} esb environment element");
        }

        public static void EsbEnvironmentModelsEqual(EsbEnvironmentModel expected, EsbEnvironmentModel actual)
        {
            Assert.AreEqual(expected.Name, actual.Name, $"{nameof(expected.Name)} property did not match for {expected.Name} esb environment");
            Assert.AreEqual(expected.ServerUrl, actual.ServerUrl, $"{nameof(expected.ServerUrl)} property did not match for {expected.Name} esb environment");
            Assert.AreEqual(expected.TargetHostName, actual.TargetHostName, $"{nameof(expected.TargetHostName)} property did not match for {expected.Name} esb environment");
            Assert.AreEqual(expected.CertificateName, actual.CertificateName, $"{nameof(expected.CertificateName)} property did not match for {expected.Name} esb environment");
            Assert.AreEqual(expected.CertificateStoreName, actual.CertificateStoreName, $"{nameof(expected.CertificateStoreName)} property did not match for {expected.Name} esb environment");
            Assert.AreEqual(expected.CertificateStoreLocation, actual.CertificateStoreLocation, $"{nameof(expected.CertificateStoreLocation)} property did not match for {expected.Name} esb environment");
            Assert.AreEqual(expected.SslPassword, actual.SslPassword, $"{nameof(expected.SslPassword)} property did not match for {expected.Name} esb environment");
            Assert.AreEqual(expected.SessionMode, actual.SessionMode, $"{nameof(expected.SessionMode)} property did not match for {expected.Name} esb environment");
            Assert.AreEqual(expected.JmsUsernameIcon, actual.JmsUsernameIcon, $"{nameof(expected.JmsUsernameIcon)} property did not match for {expected.Name} esb environment");
            Assert.AreEqual(expected.JmsPasswordIcon, actual.JmsPasswordIcon, $"{nameof(expected.JmsPasswordIcon)} property did not match for {expected.Name} esb environment");
            Assert.AreEqual(expected.JndiUsernameIcon, actual.JndiUsernameIcon, $"{nameof(expected.JndiUsernameIcon)} property did not match for {expected.Name} esb environment");
            Assert.AreEqual(expected.JndiPasswordIcon, actual.JndiPasswordIcon, $"{nameof(expected.JndiPasswordIcon)} property did not match for {expected.Name} esb environment");
            Assert.AreEqual(expected.JmsUsernameMammoth, actual.JmsUsernameMammoth, $"{nameof(expected.JmsUsernameMammoth)} property did not match for {expected.Name} esb environment");
            Assert.AreEqual(expected.JmsPasswordMammoth, actual.JmsPasswordMammoth, $"{nameof(expected.JmsPasswordMammoth)} property did not match for {expected.Name} esb environment");
            Assert.AreEqual(expected.JndiUsernameMammoth, actual.JndiUsernameMammoth, $"{nameof(expected.JndiUsernameMammoth)} property did not match for {expected.Name} esb environment");
            Assert.AreEqual(expected.JndiPasswordMammoth, actual.JndiPasswordMammoth, $"{nameof(expected.JndiPasswordMammoth)} property did not match for {expected.Name} esb environment");
            Assert.AreEqual(expected.JmsUsernameEwic, actual.JmsUsernameEwic, $"{nameof(expected.JmsUsernameEwic)} property did not match for {expected.Name} esb environment");
            Assert.AreEqual(expected.JmsPasswordEwic, actual.JmsPasswordEwic, $"{nameof(expected.JmsPasswordEwic)} property did not match for {expected.Name} esb environment");
            Assert.AreEqual(expected.JndiUsernameEwic, actual.JndiUsernameEwic, $"{nameof(expected.JndiUsernameEwic)} property did not match for {expected.Name} esb environment");
            Assert.AreEqual(expected.JndiPasswordEwic, actual.JndiPasswordEwic, $"{nameof(expected.JndiPasswordEwic)} property did not match for {expected.Name} esb environment");
        }

        public static void EsbEnvironmentElementsEqual(EsbEnvironmentElement expected, EsbEnvironmentElement actual)
        {
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.ServerUrl, actual.ServerUrl);
            Assert.AreEqual(expected.TargetHostName, actual.TargetHostName);
            Assert.AreEqual(expected.JmsUsernameIcon, actual.JmsUsernameIcon);
            Assert.AreEqual(expected.JmsPasswordIcon, actual.JmsPasswordIcon);
            Assert.AreEqual(expected.JndiUsernameIcon, actual.JndiUsernameIcon);
            Assert.AreEqual(expected.JndiPasswordIcon, actual.JndiPasswordIcon);
            Assert.AreEqual(expected.JmsUsernameMammoth, actual.JmsUsernameMammoth);
            Assert.AreEqual(expected.JmsPasswordMammoth, actual.JmsPasswordMammoth);
            Assert.AreEqual(expected.JndiUsernameMammoth, actual.JndiUsernameMammoth);
            Assert.AreEqual(expected.JndiPasswordMammoth, actual.JndiPasswordMammoth);
            Assert.AreEqual(expected.JmsUsernameEwic, actual.JmsUsernameEwic);
            Assert.AreEqual(expected.JmsPasswordEwic, actual.JmsPasswordEwic);
            Assert.AreEqual(expected.JndiUsernameEwic, actual.JndiUsernameEwic);
            Assert.AreEqual(expected.JndiPasswordEwic, actual.JndiPasswordEwic);
            Assert.AreEqual(expected.SslPassword, actual.SslPassword);
            Assert.AreEqual(expected.SessionMode, actual.SessionMode);
            Assert.AreEqual(expected.CertificateName, actual.CertificateName);
            Assert.AreEqual(expected.CertificateStoreName, actual.CertificateStoreName);
            Assert.AreEqual(expected.CertificateStoreLocation, actual.CertificateStoreLocation);
            Assert.AreEqual(expected.ReconnectDelay, actual.ReconnectDelay);
            Assert.AreEqual(expected.NumberOfListenerThreads, actual.NumberOfListenerThreads);
        }

        private static void EnvironmentViewModelsEqual(EnvironmentViewModel expected, EnvironmentViewModel actual)
        {
            Assert.AreEqual(expected.id, actual.id);
            Assert.AreEqual(expected.Name, actual.Name);
            if (expected.AppServers == null)
            {
                Assert.IsNull(actual.AppServers);
            }
            else
            {
                Assert.IsNotNull(actual.AppServers);
                Assert.AreEqual(expected.AppServers.Count, actual.AppServers.Count);
                foreach (var expectedAppServer in expected.AppServers)
                {
                    var actualAppServer = actual.AppServers.FirstOrDefault(server => server.ServerName.Equals(expectedAppServer.ServerName, StringComparison.InvariantCultureIgnoreCase));
                    Assert.IsNotNull(actualAppServer);
                    Assert.AreEqual(expectedAppServer.id, actualAppServer.id);
                    Assert.AreEqual(expectedAppServer.parentId, actualAppServer.parentId);
                    Assert.AreEqual(expectedAppServer.ServerName, actualAppServer.ServerName);
                }
            }
        }

        private static void AppServerViewModelsEqual(AppServerViewModel expected, AppServerViewModel actual)
        {
            Assert.AreEqual(expected.ServerName, actual.ServerName);
            Assert.AreEqual(expected.id, actual.id);
            Assert.AreEqual(expected.parentId, actual.parentId);
        }

        public static void AssertListsEqualOnNullsAndCount<T>(List<T> expected, List<T> actual)
        {
            if (expected == null || actual == null)
            {
                if (expected == null && actual == null)
                {
                    return;
                }
                else
                {
                    if (expected == null)
                    {
                        throw new AssertFailedException("expected object was null but actual object was not");
                    }
                    else
                    {
                        throw new AssertFailedException("actual object was null but expected object was not");
                    }
                }
            }
            Assert.AreEqual(expected.Count, actual.Count, $"expected count {expected.Count} did not match actual count {actual.Count}");
        }

        public static void ListsAreEqual(List<string> expected, List<string> actual)
        {
            //TODO just use CollectionAssert.AreEqual?
            AssertListsEqualOnNullsAndCount(expected, actual);
            // ignore ordering differences
            var orderedExpected = expected.OrderBy(s => s).ToList();
            var orderedActual = actual.OrderBy(s => s).ToList();
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(orderedExpected[i], orderedActual[i]);
            }
        }

        public static void ListsAreEqual(List<EnvironmentModel> expected, List<EnvironmentModel> actual)
        {
            AssertListsEqualOnNullsAndCount(expected, actual);

            // ignore ordering differences
            var orderedExpected = expected.OrderBy(e => e.Name).ToList();
            var orderedActual = actual.OrderBy(e => e.Name).ToList();
            for (int i = 0; i < expected.Count; i++)
            {
                EnvironmentModelsEqual(orderedExpected[i], orderedActual[i]);
            }
        }
        public static void ListsAreEqual(List<EnvironmentElement> expected, List<EnvironmentElement> actual)
        {
            AssertListsEqualOnNullsAndCount(expected, actual);

            // ignore ordering differences
            var orderedExpected = expected.OrderBy(e => e.Name).ToList();
            var orderedActual = actual.OrderBy(e => e.Name).ToList();
            for (int i = 0; i < expected.Count; i++)
            {
                EnvironmentElementsEqual(orderedExpected[i], orderedActual[i]);
            }
        }

        public static void ListsAreEqual(List<EsbEnvironmentModel> expected, List<EsbEnvironmentModel> actual)
        {
            AssertListsEqualOnNullsAndCount(expected, actual);

            // ignore ordering differences
            var orderedExpected = expected.OrderBy(e => e.Name).ToList();
            var orderedActual = actual.OrderBy(e => e.Name).ToList();
            for (int i = 0; i < expected.Count; i++)
            {
                EsbEnvironmentModelsEqual(orderedExpected[i], orderedActual[i]);
            }
        }

        public static void ListsAreEqual(List<AppServerViewModel> expected, List<AppServerViewModel> actual)
        {
            AssertListsEqualOnNullsAndCount(expected, actual);

            // ignore ordering differences
            var orderedExpected = expected.OrderBy(e => e.ServerName).ToList();
            var orderedActual = actual.OrderBy(e => e.ServerName).ToList();
            for (int i = 0; i < expected.Count; i++)
            {
                AppServerViewModelsEqual(orderedExpected[i], orderedActual[i]);
            }
        }

        public static void ListsAreEqual(List<EsbEnvironmentElement> expected, List<EsbEnvironmentElement> actual)
        {
            AssertListsEqualOnNullsAndCount(expected, actual);

            // ignore ordering differences
            var orderedExpected = expected.OrderBy(e => e.Name).ToList();
            var orderedActual = actual.OrderBy(e => e.Name).ToList();
            for (int i = 0; i < expected.Count; i++)
            {
                EsbEnvironmentElementsEqual(orderedExpected[i], orderedActual[i]);
            }
        }

        public static void ListsAreEqual(List<KeyValuePair<string,string>> expected, List<KeyValuePair<string, string>> actual)
        {
            AssertListsEqualOnNullsAndCount(expected, actual);

            // ignore ordering differences
            var orderedExpected = expected.OrderBy(e => e.Key).ToList();
            var orderedActual = actual.OrderBy(e => e.Key).ToList();

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(orderedExpected[i].Key, orderedActual[i].Key);
                Assert.AreEqual(orderedExpected[i].Value, orderedActual[i].Value);
            }
        }

        public static void AssertDictionariesEqual<TKey, TValue>(
            Dictionary<TKey,TValue> expected,
            Dictionary<TKey,TValue> actual)
        {
            var expectedAsSortedList = expected.OrderBy(kvp => kvp.Key).ToList();
            var actualAsSortedList = actual.OrderBy(kvp => kvp.Key).ToList();

            AssertListsEqualOnNullsAndCount(expectedAsSortedList, actualAsSortedList);

            foreach (var key in expected.Select(kvp => kvp.Key))
            {
                Assert.IsTrue(actual.ContainsKey(key), $"Expected key '{key}' not found in dictionary");
                Assert.AreEqual(expected[key], actual[key], $"Non-matching dictionary key value for '{key}'");
            }
        }

        public static void AssertAllExpectedEntriesInDictionary<TKey, TValue>(
          Dictionary<TKey, TValue> expected,
          Dictionary<TKey, TValue> actual)
        {
            foreach (var key in expected.Select(kvp => kvp.Key))
            {
                Assert.IsTrue(actual.ContainsKey(key), $"Expected key '{key}' not found in actual dictionary");
                Assert.AreEqual(expected[key], actual[key], $"Non-matching dictionary key value for '{key}'");
            }
        }

        public static bool AllExpectedEntriesInDictionary<TKey, TValue>(
          Dictionary<TKey, TValue> expected,
          Dictionary<TKey, TValue> actual)
        {
            foreach (var key in expected.Select(kvp => kvp.Key))
            {
                if (!actual.ContainsKey(key))
                {
                    return false;
                }
                if (!expected[key].Equals(actual[key]))
                {
                    return false;
                }
            }
            return true;
        }

        public static void TimesCloseEnough(DateTime expected, DateTime actual,
            bool ignoreMinutes = false,
            bool ignoreSeconds = true)
        {
            if (expected==null && actual == null)
            {
                return;
            }
            Assert.AreEqual(expected.Year, actual.Year);
            Assert.AreEqual(expected.Month, actual.Month);
            Assert.AreEqual(expected.Day, actual.Day);
            Assert.AreEqual(expected.Date, actual.Date);
            Assert.AreEqual(expected.Hour, actual.Hour);
            if (!ignoreMinutes)
            {
                Assert.AreEqual(expected.Minute, actual.Minute);
                if (!ignoreSeconds)
                {
                    Assert.AreEqual(expected.Second, actual.Second);
                    // ignore milliseconds
                }
            }
        }

        /// <summary>
        /// Compares the results of running an expression against a set of values.
        /// The first of the provided values should pass (return true from the filter)
        /// while the other values should fail (return false from the filter). Can be 
        /// used in a Moq It.Is() method to verify that an expression executed by a
        /// target method will give the expected results
        /// </summary>
        /// <typeparam name="T">Type of object passed to the filter delegate</typeparam>
        /// <param name="filter">Lambda expression with a delegate type of Func&lt;T,bool&gt;,
        /// meaning that the delegate function takes a parameter of type T and returns
        /// true/false</param>
        /// <param name="valuesForFilter">Array of values to use as parameters for the
        /// filter delegate. The first value should cause the func to return true while any
        /// other values should cause the func to return false (not match the filter)</param>
        /// <returns>True if the expression results match the expected (first value returns
        /// true from the expression & subsequent values return false), false if the
        /// the expression results do not match the expected.</returns>
        public static bool FilterResultComparer<T>(
            Expression<Func<T, bool>> filter,
            params T[] valuesForFilter)
        {
            var func = filter.Compile();

            bool expectedResult = true;

            foreach(var value in valuesForFilter)
            {
                var actualResult = func(value);
                if (actualResult != expectedResult)
                {
                    return false;
                }
                // for all values after the first, the expression should fail
                expectedResult = false;
            }
            // all values generated the expected pass/fail results
            return true;
        }
    }
  
    /// <summary>
    /// unit tests for unit tests :-)
    /// </summary>
    [TestClass]
    public class CustomAssertsUnitTests
    {
        [TestMethod]
        public void CustomAreEqual_WhenEnvironmentModelsAreEqual_DoesNotThrow()
        {
            //Arrange
            var first = CreateSampleEnvironmentA();
            var second = CreateSampleEnvironmentA();
            //Act
            CustomAsserts.EnvironmentModelsEqual(first, second);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void CustomAreEqual_WhenEnvironmentModelsHaveUnequalNames_Throws()
        {
            //Arrange
            var first = CreateSampleEnvironmentA();
            var second = CreateSampleEnvironmentA();
            first.Name = "Fred";
            second.Name = "Ted";
            //Act
            CustomAsserts.EnvironmentModelsEqual(first, second);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void CustomAreEqual_WhenEnvironmentModelsHaveUnequalIsEnabled_Throws()
        {
            //Arrange
            var first = CreateSampleEnvironmentA();
            var second = CreateSampleEnvironmentA();
            second.IsEnabled = false;
            //Act
            CustomAsserts.EnvironmentModelsEqual(first, second);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void CustomAreEqual_WhenEnvironmentModelsHaveUnequalDashboarUrls_Throws()
        {
            //Arrange
            var first = CreateSampleEnvironmentA();
            var second = CreateSampleEnvironmentA();
            second.DashboardUrl = "somethingElse";
            //Act
            CustomAsserts.EnvironmentModelsEqual(first, second);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void CustomAreEqual_WhenEnvironmentModelsHaveUnequalAppServers_Throws()
        {
            //Arrange
            var first = CreateSampleEnvironmentA();
            var second = CreateSampleEnvironmentA();
            second.AppServers = new List<string> { "appServerA", "appServerB", "appServerC" };
            //Act
            CustomAsserts.EnvironmentModelsEqual(first, second);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void CustomAreEqual_WhenEnvironmentModelsHaveUnequalTibcoAdminUrls_Throws()
        {
            //Arrange
            var first = CreateSampleEnvironmentA();
            var second = CreateSampleEnvironmentA();
            second.TibcoAdminUrls = new List<string>();
            //Act
            CustomAsserts.EnvironmentModelsEqual(first, second);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void CustomAreEqual_WhenEnvironmentModelsHaveUnequalIrmaDbs_Throws()
        {
            //Arrange
            var first = CreateSampleEnvironmentA();
            var second = CreateSampleEnvironmentA();
            second.IrmaDatabaseServers = null;
            //Act
            CustomAsserts.EnvironmentModelsEqual(first, second);
        }
        [TestMethod]
        public void CustomAreEqual_WhenEsbEnvironmentModelsAreEqual_DoesNotThrow()
        {
            //Arrange
            var first = CreateSampleEsbEnvironmentA();
            var second = CreateSampleEsbEnvironmentA();
            //Act
            CustomAsserts.EsbEnvironmentModelsEqual(first, second);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void CustomAreEqual_WhenEsbEnvironmentModelsHaveUnequalNames_Throws()
        {
            //Arrange
            var first = CreateSampleEsbEnvironmentA();
            var second = CreateSampleEsbEnvironmentA();
            first.Name = "Fred";
            second.Name = "Ted";
            //Act
            CustomAsserts.EsbEnvironmentModelsEqual(first, second);
        }

        [TestMethod]
        [ExpectedException(typeof(AssertFailedException))]
        public void CustomAreEqual_WhenEsbEnvironmentModelsHaveUnequalCertificateNames_Throws()
        {
            //Arrange
            var first = CreateSampleEsbEnvironmentA();
            var second = CreateSampleEsbEnvironmentA();
            second.CertificateName = "CertificateName_test \"Austin \"";
            //Act
            CustomAsserts.EsbEnvironmentModelsEqual(first, second);
        }

        protected EnvironmentModel CreateSampleEnvironmentA()
        {
            var sampleEnvironment = new EnvironmentModel
            {
                Name = "Dev0",
                IsEnabled = true,
                DashboardUrl = "http://something.org",
                WebServer = "myTestServer",
                AppServers = new List<string> { "appServerA", "appServerB" },
                IconWebUrl = "http://icon",
                TibcoAdminUrls = new List<string> { "http://tibco1:8090", "http://tibco2:8090" },
                IconDatabaseServer = @"xyz1001\sql",
                IconDatabaseName = "IconDb",
                MammothDatabaseServer = @"abc132\mammoth",
                MammothDatabaseName = "MammothDb",
                IrmaDatabaseServers = new List<string> { @"irma-fl\fl_irma", @"irma-ma\ma_irma", @"irma-rm\rm_irma" },
                IrmaDatabaseName = "ItemCatalog_Irma"
            };
            return sampleEnvironment;
        }
        protected EsbEnvironmentModel CreateSampleEsbEnvironmentA()
        {
            var sampleEnvironment = new EsbEnvironmentModel
            {
                Name = "Name_test",
                ServerUrl = "ServerUrl_test",
                TargetHostName = "TargetHostName_test",
                CertificateName = "CertificateName_test",
                CertificateStoreName = "CertificateStoreName_test",
                CertificateStoreLocation = "CertificateStoreLocation_test",
                SslPassword = "SslPassword_test",
                SessionMode = "SessionMode_test",
                JmsUsernameIcon = "JmsUsernameIcon_test",
                JmsPasswordIcon = "JmsPasswordIcon_test",
                JndiUsernameIcon = "JndiUsernameIcon_test",
                JndiPasswordIcon = "JndiPasswordIcon_test",
                JmsUsernameMammoth = "JmsUsernameMammoth_test",
                JmsPasswordMammoth = "JmsPasswordMammoth_test",
                JndiUsernameMammoth = "JndiUsernameMammoth_test",
                JndiPasswordMammoth = "JndiPasswordMammoth_test",
                JmsUsernameEwic = "JmsUsernameEwic_test",
                JmsPasswordEwic = "JmsPasswordEwic_test",
                JndiUsernameEwic = "JndiUsernameEwic_test",
                JndiPasswordEwic = "JndiPasswordEwic",
            };
            return sampleEnvironment;
        }
    }
}
