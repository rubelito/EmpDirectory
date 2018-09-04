using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace BCS.HtmlHelpers
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, Func<TEnum, bool> predicate, string optionLabel, object htmlAttributes) where TEnum : struct, IConvertible
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("TEnum");
            }

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            IList<SelectListItem> selectList = Enum.GetValues(typeof(TEnum))
                    .Cast<TEnum>()
                    .Where(e => predicate(e))
                    .Select(e => new SelectListItem
                    {
                        Value = Convert.ToUInt64(e).ToString(),
                        Text = ((Enum)(object)e).GetDisplayName(),
                    }).ToList();
            if (!string.IsNullOrEmpty(optionLabel))
            {
                selectList.Insert(0, new SelectListItem
                {
                    Text = optionLabel,
                });
            }

            return htmlHelper.DropDownListFor(expression, selectList, htmlAttributes);
        }

        public static string GetDisplayName(this Enum enumeration)
        {
            Type enumType = enumeration.GetType();
            string enumName = Enum.GetName(enumType, enumeration);
            string displayName = enumName;
            try
            {
                MemberInfo member = enumType.GetMember(enumName)[0];

                object[] attributes = member.GetCustomAttributes(typeof(DisplayAttribute), false);
                DisplayAttribute attribute = (DisplayAttribute)attributes[0];
                displayName = attribute.Name;

                if (attribute.ResourceType != null)
                {
                    displayName = attribute.GetName();
                }
            }
            catch { }
            return displayName;
        }
    }
}
