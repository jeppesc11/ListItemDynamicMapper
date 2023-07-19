using PnP.Core.Model.SharePoint;
using System.Linq.Expressions;

namespace CA_PnP_Core.Extentions {
    internal static class IListExtention {
        public static IList GetById<T>(this IListCollection lists, params Expression<Func<IList, object>>[] selectors) where T : class {
            var guid = typeof(T).GetListGuid();
            return lists.GetById(guid, selectors);
        }

        public static IList GetByTitle<T>(this IListCollection lists, params Expression<Func<IList, object>>[] selectors) where T : class {
            var title = typeof(T).GetListTitle();
            return lists.GetByTitle(title, selectors);
        }
    }
}