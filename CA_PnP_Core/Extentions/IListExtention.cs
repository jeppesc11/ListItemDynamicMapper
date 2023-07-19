using PnP.Core.Model;
using PnP.Core.Model.SharePoint;
using PnP.Core.QueryModel;
using System.Linq.Expressions;

namespace CA_PnP_Core.Extentions {
    internal static class IListExtention {
        public static IList GetById<T>(this IListCollection lists, params Expression<Func<IList, object>>[] selectors) where T : class {
            Guid guid = typeof(T).GetListGuid();

            if (guid == Guid.Empty)
            {
                throw new System.Exception("List guid must be set");
            }

            return lists.GetById(guid, selectors);
        }

        public static IList GetByTitle<T>(this IListCollection lists, params Expression<Func<IList, object>>[] selectors) where T : class {
            string title = typeof(T).GetListTitle();

            if (string.IsNullOrEmpty(title))
            {
                throw new System.Exception("List title must be set");
            }

            return lists.GetByTitle(title, selectors);
        }

        public static IEnumerable<T> GetItems<T>(this IWeb web, params Expression<Func<IList, object>>[] selectors) where T : class, new()
        {
            IEnumerable<IListItem> items = web.Lists.GetListByType<T>(selectors).Items.AsRequested();
            return items.Select(p => p.ToObject<T>());
        }

        public static T UpdateItem<T>(this IWeb web, T item) where T : class, new()
        {
            IList list = web.Lists.GetListByType<T>();
            string? idAsString = item.GetType().GetProperty("Id")?.GetValue(item, null)?.ToString();

            if(string.IsNullOrEmpty(idAsString))
            {
                throw new System.Exception("Id must be set");
            }

            int id = int.Parse(idAsString);
            IListItem listItem = list.Items.GetById(id, p => p.Title, p => p.Id, p => p.FieldValuesAsHtml);
            listItem.PopulateFromDictionary(item.AsDictionary()).Update();

            return listItem.ToObject<T>();
        }

        #region Private Methods

        private static IListItem PopulateFromDictionary(this IListItem item, IDictionary<string, object?> dic)
        {

            foreach (var keyValuePair in dic)
            {

                if (keyValuePair.Key.ToLower() == "id")
                {
                    continue;
                }

                item[keyValuePair.Key] = keyValuePair.Value;
            }

            return item;
        }

        private static IList GetListByType<T>(this IListCollection lists, params Expression<Func<IList, object>>[] selectors) where T : class, new()
        {
            Guid guid = typeof(T).GetListGuid();

            if (guid != Guid.Empty)
            {
                return lists.GetById(guid, selectors);
            }

            string title = typeof(T).GetListTitle();

            if (!string.IsNullOrEmpty(title))
            {
                return lists.GetByTitle(title, selectors);
            }

            throw new Exception("List guid or title must be set");
        }

        #endregion

    }
}