namespace Vim.Locale.Controller.DataAccess.Models
{
    using System.Linq;

    public class LocaleEventModel
    {
        public int QueueId { get; set; }
        public int EventTypeId { get; set; }
        public string EventMessage { get; set; }
        public string ErrorMessage { get; set; }
        public int EventReferenceId { get; set; }
        public int NumberOfRetry { get; set; }
        public VimStoreModel StoreModel { get; set; }

        public VimStoreModel GetValidatedStoreModel()
        {
            var nullStringProperties = this.StoreModel.GetType()
                .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(string) && p.GetValue(this.StoreModel) == null)
                .ToList();

            nullStringProperties.ForEach(p => p.SetValue(this.StoreModel, string.Empty));

            return this.StoreModel;
        }
    }
}
