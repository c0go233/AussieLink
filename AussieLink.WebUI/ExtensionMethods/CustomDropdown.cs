using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AussieLink.WebUI.ExtensionMethods
{
    public static class CustomDropdown
    {
        public static MvcHtmlString CustomDropdownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, 
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> list, string optionLabel,
            object htmlAttributes = null)
        {
            HtmlHelper.ClientValidationEnabled = true;

            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            string name = ExpressionHelper.GetExpressionText((LambdaExpression)expression);
            return CustomDropdownList(htmlHelper, metadata, name, optionLabel, list, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        private static MvcHtmlString CustomDropdownList(this HtmlHelper htmlHelper, 
            ModelMetadata metadata, string name, string optionLabel, 
            IEnumerable<SelectListItem> list, IDictionary<string, object> htmlAttributes)
        {
            string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (String.IsNullOrEmpty(fullName))
            {
                throw new ArgumentException("name");
            }

            var selectedValue = IsSelected(list, metadata.Model) ? metadata.Model.ToString() : String.Empty;
            var selectBox = GetSelectBox(htmlAttributes);
            var hiddenInput = GetHiddenInput(fullName, name, metadata, htmlHelper, selectedValue);
            var selectedBox = GetSelectedBox();
            var selectOptionBox = GetSelectOptionBox(optionLabel, list);

            selectBox.InnerHtml = hiddenInput.ToString(TagRenderMode.Normal)
                                + selectedBox.ToString(TagRenderMode.Normal) 
                                + selectOptionBox.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(selectBox.ToString(TagRenderMode.Normal));
        }

        private static bool IsSelected(IEnumerable<SelectListItem> list, object selectedValue)
        {
            if (selectedValue == null)
                return false;

            selectedValue = selectedValue.ToString();

            foreach(var item in list)
            {
                if (item.Value.Equals(selectedValue))
                    return true;
            }
            return false;
        }

        private static TagBuilder GetSelectBox(IDictionary<string, object> htmlAttributes)
        {
            TagBuilder selectBox = new TagBuilder("div");
            if (htmlAttributes != null)
                selectBox.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return selectBox;
        }

        private static TagBuilder GetHiddenInput(string fullName, string name, ModelMetadata metadata, HtmlHelper htmlHelper, string selectedValue)
        {
            TagBuilder hiddenInput = new TagBuilder("input");
            hiddenInput.Attributes.Add("name", fullName);
            hiddenInput.Attributes.Add("id", fullName);
            hiddenInput.Attributes.Add("value", selectedValue);
            hiddenInput.Attributes.Add("class", "select-box-input");
            hiddenInput.Attributes.Add("readonly", "readonly");
            hiddenInput.MergeAttributes(htmlHelper.GetUnobtrusiveValidationAttributes(name, metadata));
            return hiddenInput;
        }

        private static TagBuilder GetSelectedBox()
        {
            var selectedBox = GetTagWith("div", "selectedBox fx-spc-btw select-box-toggle");
            var selectedBoxText = GetTagWith("span", "selectedBox__text fx-1");
            var selectedBoxIcon = GetTagWith("i", "ion-chevron-down c-mg-left-5");

            selectedBox.InnerHtml = selectedBoxText.ToString(TagRenderMode.Normal) + selectedBoxIcon.ToString(TagRenderMode.Normal);
            return selectedBox;
        }

        private static TagBuilder GetSelectOptionBox(string optionLabel, IEnumerable<SelectListItem> list)
        {
            var selectOptionBox = GetTagWith("div", "select-option-box select-box-toggle");

            StringBuilder options = new StringBuilder();

            // Make optionLabel the first item that gets rendered.
            if (optionLabel != null)
                options = options.Append(GetSelectOption(String.Empty, optionLabel, "select-option"));

            foreach (var item in list)
            {
                if (item.Value != null)
                    options = options.Append(GetSelectOption(item.Value, item.Text, "select-option"));
            }

            selectOptionBox.InnerHtml = options.ToString();
            return selectOptionBox;
        }

        private static string GetSelectOption(string value, string text, string className)
        {
            var selectOption = "<span value='" + value + "'" + "class='" + className + "'>" + text + "</span>";
            return selectOption;
        }

        private static TagBuilder GetTagWith(string tag, string className)
        {
            TagBuilder tagBuilder = new TagBuilder(tag);
            tagBuilder.Attributes.Add("class", className);
            return tagBuilder;
        }
    }
}
