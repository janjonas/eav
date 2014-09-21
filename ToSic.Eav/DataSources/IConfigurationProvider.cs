using System.Collections.Generic;
using ToSic.Eav.DataSources.Tokens;

namespace ToSic.Eav.DataSources
{
	/// <summary>
	/// Provides Configuration for a configurable DataSource
	/// </summary>
	public interface IConfigurationProvider
	{
		//string DataSourceType { get; }
		/// <summary>
		/// Property Sources this Provider can use
		/// </summary>
		Dictionary<string, IPropertyAccess> Sources { get; }

		/// <summary>
		/// Replaces all Tokens in the ConfigList with actual values provided by the Sources-Provider
		/// </summary>
		void LoadConfiguration(IDictionary<string, string> configList);

        /// <summary>
        /// Replaces all tokens in the singleValue with the data from the sources provider; optionally you can change the amount of recursions 
        /// </summary>
        /// <param name="singleValue"></param>
        /// <param name="recursion"></param>
        /// <returns></returns>
	    string LoadConfiguration(string singleValue, int recursion = 3);
	}
}
