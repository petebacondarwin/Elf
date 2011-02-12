namespace Elf.Web.Mvc {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using Elf.Persistence.Entities;
    using Elf.Configuration;

    public interface IControllerFinder {
        /// <summary>
        /// Find a type that will be the controller for the given content item.
        /// </summary>
        /// <param name="item">A content item which needs a controller.</param>
        /// <returns></returns>
        Type FindControllerFor(ContentItem item);
    }

    public class ControllerFinder : IControllerFinder {
        readonly AssemblyList assemblies;
        public ControllerFinder(AssemblyList assemblies) {
            this.assemblies = assemblies;
        }
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
    }
}
