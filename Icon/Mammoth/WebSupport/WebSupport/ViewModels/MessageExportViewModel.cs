using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using WebSupport.DataAccess;
using WebSupport.Models;

namespace WebSupport.ViewModels
{
  public class MessageExportViewModel
  {
    public const int MaxNumberOfItems = 1000;
    public enum QueueName { Archive, Inventory, Order, Receipt }
    public enum StatusName { Failed, Processed, Unprocessed }
    
    public string Error             { get; set; }
    public bool IsSuccess           { get { return String.IsNullOrEmpty(Error); }}
    public DataTable ResultTable    { get; set; }
    public SelectListItem[] Regions { get; set; }
    public SelectListItem[] Queues  { get; set; }
    public SelectListItem[] Status  { get; set; }

    [Display(Name = "Key ID")]
    [RegularExpression(ValidationConstants.RegExNumeric, ErrorMessage = ValidationConstants.InvalidNumericInput)]
    public string KeyID { get; set; }

    [Required]
    [Display(Name = "Message Queue")]
    public int QueueIndex { get; set; }

    [Required]
    [Display(Name = "Region")]
    public int RegionIndex { get; set; }

    [Required]
    [Display(Name = "Status")]
    public int StatusIndex { get; set; }
    
    [DataType(DataType.Date)]
    [Display(Name = "Received Date")]
    public DateTime? ReceivedDate { get; set; }

    public MessageExportViewModel()
    {
      Regions = DataConstants.WholeFoodsRegions.Distinct(StringComparer.InvariantCultureIgnoreCase).OrderBy(x => x)
                             .Where(x => x != RegionNameConstants.UK)
                             .Select((x, i) => new SelectListItem{ Text = x, Value = i.ToString(), Selected = (i == 0) })
                             .ToArray();

      Queues = Enum.GetNames(typeof(QueueName)).Cast<string>().OrderBy(x => x, StringComparer.InvariantCultureIgnoreCase)
                   .Select((x, i) => new SelectListItem{ Text = x, Value = ((int)Enum.Parse(typeof(QueueName), x)).ToString(), Selected = (i == 0) })
                   .ToArray();

      Status = Enum.GetNames(typeof(StatusName)).Cast<string>().OrderBy(x => x, StringComparer.InvariantCultureIgnoreCase)
                   .Select((x, i) => new SelectListItem{ Text = x, Value = ((int)Enum.Parse(typeof(StatusName), x)).ToString(), Selected = (i == 0) })
                   .ToArray();
    }
  }
}