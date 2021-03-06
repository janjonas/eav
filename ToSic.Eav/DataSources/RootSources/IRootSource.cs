﻿using System.Collections.Generic;
using ToSic.Eav.DataSources.Caches;

namespace ToSic.Eav.DataSources.RootSources
{
	/// <summary>
	/// Root data source interface for standard Eav Cache
	/// </summary>
	public interface IRootSource
	{
		/// <summary>
		/// Get a CacheItem to build Cache for this App
		/// </summary>
		CacheItem GetDataForCache(IDataSource cache);
		/// <summary>
		/// Get a Dictionary of all Zones and Apps
		/// </summary>
		Dictionary<int, ZoneModel> GetAllZones();
		/// <summary>
		/// Get a Dictionary of all AssignmentObjectTypes
		/// </summary>
		Dictionary<int, string> GetAssignmentObjectTypes();
	}
}