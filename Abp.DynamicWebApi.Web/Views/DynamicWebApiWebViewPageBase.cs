using Abp.Web.Mvc.Views;

namespace Abp.DynamicWebApi.Web.Views
{
    public abstract class DynamicWebApiWebViewPageBase : DynamicWebApiWebViewPageBase<dynamic>
    {

    }

    public abstract class DynamicWebApiWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected DynamicWebApiWebViewPageBase()
        {
            LocalizationSourceName = DynamicWebApiConsts.LocalizationSourceName;
        }
    }
}