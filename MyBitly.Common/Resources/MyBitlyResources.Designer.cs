﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyBitly.Common.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class MyBitlyResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal MyBitlyResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MyBitly.Common.Resources.MyBitlyResources", typeof(MyBitlyResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Не указана строка подключения..
        /// </summary>
        public static string ConnectionStringNotConfigure {
            get {
                return ResourceManager.GetString("ConnectionStringNotConfigure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to EMPTY_ARG_URL.
        /// </summary>
        public static string EMPTY_ARG_URL {
            get {
                return ResourceManager.GetString("EMPTY_ARG_URL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to EMPTY_SHORT_URL.
        /// </summary>
        public static string EMPTY_SHORT_URL {
            get {
                return ResourceManager.GetString("EMPTY_SHORT_URL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to EXCEPTION_STORAGE_URL.
        /// </summary>
        public static string EXCEPTION_STORAGE_URL {
            get {
                return ResourceManager.GetString("EXCEPTION_STORAGE_URL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Не удалось получаить информацию о ссылке..
        /// </summary>
        public static string GetException {
            get {
                return ResourceManager.GetString("GetException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Не удалось получить записи..
        /// </summary>
        public static string GetListException {
            get {
                return ResourceManager.GetString("GetListException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INVALID_ARG_URL.
        /// </summary>
        public static string INVALID_ARG_URL {
            get {
                return ResourceManager.GetString("INVALID_ARG_URL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INVALID_REQUEST.
        /// </summary>
        public static string INVALID_REQUEST {
            get {
                return ResourceManager.GetString("INVALID_REQUEST", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ссылка не найдена..
        /// </summary>
        public static string NotFoundException {
            get {
                return ResourceManager.GetString("NotFoundException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Не указан короткий домен..
        /// </summary>
        public static string ShortDomenNotConfigure {
            get {
                return ResourceManager.GetString("ShortDomenNotConfigure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Не удалось сократить ссылку..
        /// </summary>
        public static string ShortenUrlException {
            get {
                return ResourceManager.GetString("ShortenUrlException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to TECH_EXCEPTION.
        /// </summary>
        public static string TECH_EXCEPTION {
            get {
                return ResourceManager.GetString("TECH_EXCEPTION", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Техническая ошибка, повторите запрос позже..
        /// </summary>
        public static string TechnicalException {
            get {
                return ResourceManager.GetString("TechnicalException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to URL_NOT_FOUND.
        /// </summary>
        public static string URL_NOT_FOUND {
            get {
                return ResourceManager.GetString("URL_NOT_FOUND", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Укажите ссылку..
        /// </summary>
        public static string UrlIsNullOrEmptyException {
            get {
                return ResourceManager.GetString("UrlIsNullOrEmptyException", resourceCulture);
            }
        }
    }
}