using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ReconhecimentoFacialAWS.Helpers;

public class ControllerBaseExtension : Controller
{
    protected async Task<string> RenderViewAsync(Controller controller, string viewName, object model, bool partial = false)
    {
        if (string.IsNullOrEmpty(viewName))
        {
            viewName = controller.ControllerContext.ActionDescriptor.ActionName;
        }

        controller.ViewData.Model = model;

        using StringWriter _writer = new();
        IViewEngine _viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
        ViewEngineResult _viewResult = _viewEngine.FindView(controller.ControllerContext, viewName, !partial);

        if (_viewResult.Success == false)
        {
            return $"A view with the name {viewName} could not be found";
        }

        ViewContext _viewContext = new(
            controller.ControllerContext,
            _viewResult.View,
            controller.ViewData,
            controller.TempData,
            _writer,
            new HtmlHelperOptions()
        );

        await _viewResult.View.RenderAsync(_viewContext);

        return _writer.GetStringBuilder().ToString();
    }
}
