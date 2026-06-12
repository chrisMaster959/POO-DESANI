using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SmartBarberV2.Filters
{
    public class AutenticacaoAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var usuarioId = context.HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
            {
                context.Result = new RedirectToActionResult(
                    "Login",
                    "Pessoa",
                    null
                );

                return;
            }

            // CORREÇÃO: impede o navegador de guardar a página em cache,
            // evitando que o botão "voltar" mostre a tela após o logout
            context.HttpContext.Response.Headers["Cache-Control"] =
                "no-cache, no-store, must-revalidate";
            context.HttpContext.Response.Headers["Pragma"] = "no-cache";
            context.HttpContext.Response.Headers["Expires"] = "0";

            base.OnActionExecuting(context);
        }
    }
}