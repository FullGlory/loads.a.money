// -----------------------------------------------------------------------
// <copyright file="UnityHelper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace SpreadBet.Common.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Linq;
	using System.Text;
	using Microsoft.Practices.Unity;
	using Microsoft.Practices.Unity.Configuration;

	/// <summary>
	/// Provides support for unity
	/// </summary>
	public static class UnityHelper
	{
		private static readonly object keylock = new object();
		private static IUnityContainer _container = null;

		/// <summary>
		/// Gets the container instance.
		/// </summary>
		/// <returns>The container</returns>
		public static IUnityContainer GetContainer()
		{
			lock (keylock)
			{
				if (_container == null)
				{
					ConfigureContainer();
				}
			}

			return _container;
		}

		/// <summary>
		/// Configures the container.
		/// </summary>
		private static void ConfigureContainer()
		{
			var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
			_container = new UnityContainer();

			section.Configure(_container);
		}
	}
}
