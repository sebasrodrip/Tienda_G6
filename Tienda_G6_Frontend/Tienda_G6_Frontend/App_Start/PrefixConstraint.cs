using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace Tienda_G6_Frontend.App_Start
{
    public class PrefixConstraint : IRouteConstraint
    {
        private readonly string _prefix;

        public PrefixConstraint(string prefix)
        {
            _prefix = prefix;
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            // Obtén el valor del prefijo en la URL
            var prefixValue = values["controller"] as string;

            // Verifica si el prefijo coincide con el valor esperado
            return string.Equals(prefixValue, _prefix, StringComparison.OrdinalIgnoreCase);
        }
    }

}