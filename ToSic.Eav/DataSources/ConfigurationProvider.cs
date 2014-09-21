using System.Collections.Generic;
using System.Linq;
using ToSic.Eav.DataSources.Tokens;

namespace ToSic.Eav.DataSources
{
	/// <summary>
	/// Provides Configuration for a configurable DataSource
	/// </summary>
	public class ConfigurationProvider : IConfigurationProvider
	{
		//public string DataSourceType { get; internal set; }
		public Dictionary<string, IPropertyAccess> Sources { get; private set; }
		/// <summary>
		/// List of all Configurations for this DataSource
		/// </summary>
		//public IDictionary<string, string> configList { get; internal set; }
		private readonly TokenReplace _tokenReplace;

		/// <summary>
		/// Constructs a new Configuration Provider
		/// </summary>
		public ConfigurationProvider()
		{
			Sources = new Dictionary<string, IPropertyAccess>();
			_tokenReplace = new TokenReplace(Sources);
		}

        /// <summary>
        /// Go through an entire dictionary of configurations and resolve them
        /// </summary>
        /// <param name="configList"></param>
		public void LoadConfiguration(IDictionary<string, string> configList )
		{
			foreach (var o in configList.ToList())
                configList[o.Key] = LoadConfiguration(o.Value);
		}

        /// <summary>
        /// Loads the configuration for one string. This string could be
        /// * a single token like "[Module:ModuleId]"
        /// * a mix or multiple like "Hello [Profile:FirstName] welcome to [Portal:PortalName]"
        /// * fallback tokens like "Hello [Profile:FirstName||anonymous user]"
        /// * recursive fallback token like "Hello [Profile:Firstname||[App:DefaultName]]"
        /// </summary>
        /// <param name="singleValue"></param>
        /// <returns></returns>
	    public string LoadConfiguration(string singleValue, int recursions = 3)
	    {
	        if (!_tokenReplace.ContainsTokens(singleValue))
	            return singleValue;

	        var newValue = _tokenReplace.ReplaceTokens(singleValue);

	        // do recursion 3 more times for tokens in tokens if nothing found
	        for (var i = 0; i < recursions; i++)
	        {
	            if (_tokenReplace.ContainsTokens(newValue))
	                newValue = _tokenReplace.ReplaceTokens(newValue);
	            else
	                break;
	        }
	        return newValue;
	    }
	}
}