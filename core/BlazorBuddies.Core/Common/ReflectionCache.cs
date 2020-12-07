using System;
using System.Collections.Generic;
using System.Reflection;

// We are purposely doing static fields in a generic type here. So ignore any warnings R# may give.
// ReSharper disable StaticMemberInGenericType

namespace BlazorBuddies.Core.Common
{
	/// <summary>
	///   A persistent reflection cache for a given type <typeparamref name="T" />.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public static class ReflectionCache<T>
	{
		static ReflectionCache()
		{
			Type = typeof(T);
			PublicInstanceProperties = Type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
			InstanceProperties = Type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			PublicStaticProperties = Type.GetProperties(BindingFlags.Public | BindingFlags.Static);
		}

		/// <summary>
		///   The <seealso cref="System.Type" /> of <typeparamref name="T" />.
		/// </summary>
		public static Type Type { get; }

		/// <summary>
		///   All of the public, instance level properties on <typeparamref name="T" />.
		/// </summary>
		public static IReadOnlyList<PropertyInfo> PublicInstanceProperties { get; }

		/// <summary>
		///   All of the public and non-public, instance level properties on <typeparamref name="T" />.
		/// </summary>
		public static IReadOnlyList<PropertyInfo> InstanceProperties { get; }

		/// <summary>
		///   All of the public, static level properties on <typeparamref name="T" />.
		/// </summary>
		public static IReadOnlyList<PropertyInfo> PublicStaticProperties { get; }

		/// <summary>
		///   Clones the property values from one instance of
		///   <typeparam name="T"></typeparam>
		///   to another.
		/// </summary>
		/// <param name="from">The instance to clone property values from.</param>
		/// <param name="to">The instance to clone property values to.</param>
		/// <param name="includePrivateProperties">Whether to also clone non-public instance properties.</param>
		/// <exception cref="T:System.MethodAccessException">
		///   There was an illegal attempt to access a private or protected method inside a class.
		/// </exception>
		/// <exception cref="T:System.Reflection.TargetInvocationException">
		///   An error occurred while setting the property value. The <see cref="P:System.Exception.InnerException" /> property
		///   indicates the reason for the error.
		/// </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="from" /> or <paramref name="to" /> is
		///   <see langword="null" />
		/// </exception>
		public static void ClonePropertyValues(T from, T to, bool includePrivateProperties = false)
		{
			if (from == null) {
				throw new ArgumentNullException(nameof(from));
			}

			if (to == null) {
				throw new ArgumentNullException(nameof(to));
			}

			foreach (var prop in includePrivateProperties ? InstanceProperties : PublicInstanceProperties) {
				// If there isn't a getter or setter, don't do anything.
				if ((prop.SetMethod != null) && (prop.GetMethod != null)) {
					prop.SetValue(to, prop.GetValue(from));
				}
			}
		}
	}
}
