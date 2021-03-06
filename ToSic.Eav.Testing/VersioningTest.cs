﻿using System.Diagnostics;
using NUnit.Framework;

namespace ToSic.Eav.Testing
{
	public class VersioningTest
	{
		[Test]
		public void GetEntityVersions()
		{
			var ctx = EavContext.Instance(appId: 2);
			var allVersioned = ctx.GetEntityVersions(5449);

			var unchanged = ctx.GetEntityVersions(330);

			var partlyVersioned = ctx.GetEntityVersions(329);

			const int defaultCultureDimension = 5;

			var version = ctx.GetEntityVersion(5449, 8063, defaultCultureDimension);

			ctx.RestoreEntityVersion(5449, 8065, defaultCultureDimension);

			Debug.Write(version);
		}
	}
}
