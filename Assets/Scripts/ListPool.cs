using System.Collections.Generic;

namespace Miren
{
	public static class ListPool<T>
	{
		private static Stack<List<T>> pool = new Stack<List<T>>();

		public static List<T> Get ()
		{
			List<T> list = pool.Pop ();
			if (list == null)
			{
				list = new List<T> ();
			}
			return list;
		}

		public static void Add (List<T> list)
		{
			if (list == null)
			{
				return;
			}
			list.Clear ();
			pool.Push (list);
		}
	}
}
