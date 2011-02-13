namespace Elf.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using Elf.Persistence.Entities;
    using Elf.Configuration;

    public interface IControllerFinder {
        Type FindControllerFor(ContentItem item);
        string FindControllerNameFor(ContentItem item);
    }

    public class ControllerFinder : Elf.Web.Mvc.IControllerFinder {
        readonly AssemblyList assemblies;
        public ControllerFinder(AssemblyList assemblies) {
            this.assemblies = assemblies;
        }

        /// <summary>
        /// Find a type that will be the controller for the given content item.
        /// </summary>
        /// <param name="item">A content item which needs a controller.</param>
        /// <returns>The type of the controller that will control the content item or null if there is none</returns>
        public Type FindControllerFor(ContentItem item) {
            Type itemType = item.GetType();
            while (itemType.BaseType != typeof(Object)) {
                // The convention is that controllers will be named [ContentItemTypeName]Controller
                string controllerName = itemType.Name + "Controller";
                foreach (Assembly assembly in assemblies) {
                    Type controllerType = assembly.GetTypes().SingleOrDefault(type => type.Name == controllerName);
                    // Check that the type exists and is actually an MVC controller
                    if (controllerType != null && typeof(IController).IsAssignableFrom(controllerType)) {
                        return controllerType;
                    }
                }
                itemType = itemType.BaseType;
            }
            return null;
        }


        /// <summary>
        /// Find the standard name of a controller for the given content item.
        /// </summary>
        /// <param name="item">A content item which needs a controller.</param>
        /// <returns>The name of the controller that will control the content item or null if there is none</returns>
        /// <remarks>
        /// Note that this name will be the MVC standard name in the sense that the actual type name will be have Controller on the end.
        /// For example if the name is Page, the actual class will be PageController.
        /// </remarks>
        public string FindControllerNameFor(ContentItem item) {
            Type controllerType = FindControllerFor(item);
            return GetControllerName(controllerType);
        }

        private string GetControllerName(Type controllerType) {
            string typeName = controllerType.Name;
            if (typeName.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)) {
                return typeName.Substring(0, typeName.Length - "Controller".Length);
            }
            return typeName;
        }
    }
}
