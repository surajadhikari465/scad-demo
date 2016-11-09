using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject
{
    public class Dog
    {
        private IBarkService barkService;

        public Dog() : this(new BarkService())
        {
        }

        public Dog(IBarkService barkService)
        {
            this.barkService = barkService;
        }

        public string Status { get; set; }

        public string Bark()
        {
            return barkService.Bark();
        }

        public void GiveItem(string item)
        {
            if (item.Equals("Bone", StringComparison.InvariantCultureIgnoreCase))
            {
                Status = "Happy";
            }
        }
    }

    public interface IBarkService
    {
        string Bark();
    }

    public class BarkService : IBarkService
    {
        public string Bark()
        {
            return "Bark";
        }
    }
}
