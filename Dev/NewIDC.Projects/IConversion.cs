using System.Collections.Generic;

namespace NewIDC.Projects
{
	public interface IConversion
	{
		List<string[]> Convert();
		string[] GetHeader();

	}

}

