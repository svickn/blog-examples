using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace System.Web.Helpers
{
    public static partial class MvcHtmlHelpers
    {
        public static KnockoutCollection<TModel, TItem> GetKnockoutCollection<TModel, TItem>(this HtmlHelper<TModel> helper, Expression<Func<TModel, ICollection<TItem>>> collectionExpression)
        {
            return new KnockoutCollection<TModel, TItem>(helper, collectionExpression, null);
        }

        public class KnockoutCollection<TModel, TItem> : IParentCollection, IKnockoutCollection<TItem>, IDisposable
        {
            private HtmlHelper _helper;
            private IParentCollection _parentCollection;
            public string CollectionName { get; set; }
            public bool ForEachInitiated { get; set; }

            #region Constructors
            public KnockoutCollection(HtmlHelper helper, Expression<Func<TModel, ICollection<TItem>>> collectionExpression, string collectionName) : this(helper, collectionExpression, collectionName, null) { }
            private KnockoutCollection(HtmlHelper helper, Expression<Func<TModel, ICollection<TItem>>> collectionExpression, string collectionName, IParentCollection parentCollection)
            {
                _helper = helper;
                _parentCollection = parentCollection;

                if (!string.IsNullOrEmpty(collectionName))
                {
                    CollectionName = collectionName;
                }
                else
                {
                    CollectionName = ExpressionHelper.GetExpressionText(collectionExpression);
                }
            }
            #endregion

            public ForEacher BeginForEach()
            {
                if (_parentCollection != null && !_parentCollection.ForEachInitiated)
                {
                    throw new InvalidOperationException(
                        string.Format("{0}.BeginForEach cannot be begun before its parent {1}.", CollectionName,
                                      _parentCollection.CollectionName));
                }

                return new ForEacher(this);
            }

            public KnockoutCollection<TItem, TChild> ChildCollection<TChild>(Expression<Func<TItem, ICollection<TChild>>> collectionExpression)
            {
                return new KnockoutCollection<TItem, TChild>(_helper, collectionExpression, null, this);
            }

            public string ForEachBinding()
            {
                return string.Format("<!-- ko foreach: {0} -->", CollectionName);
            }

            public string ForEachClosing()
            {
                return "<!-- /ko -->";
            }

            public MvcHtmlString TextBoxFor<TProperty>(Expression<Func<TItem, TProperty>> propertyExpression, object htmlAttributes = null)
            {
                return InputFor(propertyExpression, InputType.Text, htmlAttributes);
            }

            public MvcHtmlString RemoveButton()
            {
                return MvcHtmlString.Create(string.Format("<button class='removeButton' type='button' title='Remove' data-bind='click: $root.remove{0}'>Remove</button>", CollectionName.Split('.').LastOrDefault()));
            }

            public MvcHtmlString AddButton()
            {
                return MvcHtmlString.Create(string.Format("<button class='addButton' type='button' title='Add' data-bind='click: $root.add{0}'>Add</button>", CollectionName.Split('.').LastOrDefault()));
            }

            public string GetParentCollectionName(int currentIndex)
            {
                var collectionInfo = string.Format("{0}[' + {1} + '].", CollectionName, GetKnockoutIndex(currentIndex));
                if (_parentCollection != null)
                {
                    return _parentCollection.GetParentCollectionName(currentIndex + 1)
                           + collectionInfo;
                }

                return collectionInfo;
            }

            public string GetParentCollectionId(int currentIndex)
            {
                var collectionInfo = string.Format("{0}_' + {1} + '__", CollectionName, GetKnockoutIndex(currentIndex));
                if (_parentCollection != null)
                {
                    return _parentCollection.GetParentCollectionId(currentIndex + 1)
                           + collectionInfo;
                }

                return collectionInfo;
            }

            #region Private Methods
            private string GetKnockoutIndex(int currentIndex)
            {
                if (currentIndex == 0)
                {
                    return "$index()";
                }
                return string.Concat(Enumerable.Repeat("$parentContext.", currentIndex)) + "$index()";
            }

            private MvcHtmlString InputFor<TProperty>(Expression<Func<TItem, TProperty>> propertyExpression, InputType type, object htmlAttributes = null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                var propertyName = ExpressionHelper.GetExpressionText(propertyExpression);
                var parentCollectionName = this.GetParentCollectionName(0);
                var parentCollectionId = this.GetParentCollectionId(0);

                var input = new TagBuilder("input");
                input.MergeAttributes(attributes);
                input.MergeAttribute("type", type.ToString().ToLowerInvariant());
                input.MergeAttribute("data-bind", string.Format("value: {0} , attr: {{name: '{1}{0}', id: '{2}{0}'}}", propertyName, parentCollectionName, parentCollectionId));

                return MvcHtmlString.Create(input.ToString(TagRenderMode.SelfClosing));
            }
            #endregion

            #region IDisposable
            public void Dispose()
            {
                this._helper = null;
                this._parentCollection = null;
            }
            #endregion

            #region ForEacher
            public class ForEacher : IDisposable, IKnockoutCollection<TItem>
            {
                private KnockoutCollection<TModel, TItem> _collection;

                public ForEacher(KnockoutCollection<TModel, TItem> collection)
                {
                    _collection = collection;
                    collection._helper.ViewContext.Writer.Write(
                        collection.ForEachBinding()
                    );
                }

                #region IDisposable
                public void Dispose()
                {
                    _collection._helper.ViewContext.Writer.Write(_collection.ForEachClosing());
                }
                #endregion

                #region Implementation of IKnockoutCollection<TItem>
                public MvcHtmlString TextBoxFor<TProperty>(Expression<Func<TItem, TProperty>> propertyExpression, object htmlAttributes = null)
                {
                    return _collection.TextBoxFor(propertyExpression, htmlAttributes);
                }

                public KnockoutCollection<TItem, TChild> ChildCollection<TChild>(Expression<Func<TItem, ICollection<TChild>>> collectionExpression)
                {
                    return _collection.ChildCollection(collectionExpression);
                }

                public MvcHtmlString RemoveButton()
                {
                    return _collection.RemoveButton();
                }

                public MvcHtmlString AddButton()
                {
                    return _collection.AddButton();
                }
                #endregion
            }
            #endregion
        }

        private interface IParentCollection
        {
            string GetParentCollectionName(int currentIndex);
            string GetParentCollectionId(int currentIndex);
            bool ForEachInitiated { get; }
            string CollectionName { get; set; }
        }

        private interface IKnockoutCollection<TItem>
        {
            MvcHtmlString TextBoxFor<TProperty>(Expression<Func<TItem, TProperty>> propertyExpression, object htmlAttributes = null);
            MvcHtmlString RemoveButton();
            MvcHtmlString AddButton();
            KnockoutCollection<TItem, TChild> ChildCollection<TChild>(Expression<Func<TItem, ICollection<TChild>>> collectionExpression);
        }
    }
}