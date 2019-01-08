using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothGpmService.AmazonUploader
{
	public interface IAmazonFileUploader
	{
		bool SendMyFileToS3(string region);
	}
}
