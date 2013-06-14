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

		// <summary>
		/// Gets the container instance.
		/// </summary>
		/// <returns>The container</returns>
		public static IUnityContainer GetContainer(string containerName)
		{
			lock (keylock)
			{
				if (_container == null)
				{
					var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
					_container = new UnityContainer();

					section.Configure(_container, containerName);
				} 
			}

			return _container;
		}

		/// <summary>
		/// Gets the container instance.
		/// </summary>
		/// <returns>The container</returns>
		public static IUnityContainer GetContainer()
		{
			lock (keylock)
			{
				var defaultContainer = ConfigurationManager.AppSettings["defaultApplicationName"];
				if (!string.IsNullOrEmpty(defaultContainer))
				{
					return GetContainer(defaultContainer);
				}
				else
				{
					throw new ArgumentException("Please specify the application name on the command line, or a valid default application in through configuration");
				}
			}

			return null;
		}
	}
}
