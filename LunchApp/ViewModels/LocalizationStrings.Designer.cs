﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ViewModels {
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
    public class LocalizationStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal LocalizationStrings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ViewModels.LocalizationStrings", typeof(LocalizationStrings).Assembly);
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
        ///   Looks up a localized string similar to Нет активного меню.
        /// </summary>
        public static string ActiveMenuDoesntNotExist {
            get {
                return ResourceManager.GetString("ActiveMenuDoesntNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Другое меню на эту дату создано.
        /// </summary>
        public static string AnotherMenuForDateExists {
            get {
                return ResourceManager.GetString("AnotherMenuForDateExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to yyyy-MM-dd.
        /// </summary>
        public static string DateFormat {
            get {
                return ResourceManager.GetString("DateFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Нельзя создать меню в прошлом.
        /// </summary>
        public static string DateShouldBeInFuture {
            get {
                return ResourceManager.GetString("DateShouldBeInFuture", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Меню должно иметь правильную структуру.
        /// </summary>
        public static string IncorrectSectionStructure {
            get {
                return ResourceManager.GetString("IncorrectSectionStructure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to БИЗНЕС – ЛАНЧ на {0}.
        /// </summary>
        public static string MenuDefaultName {
            get {
                return ResourceManager.GetString("MenuDefaultName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Меню на эту дату уже создано.
        /// </summary>
        public static string MenuForThisDateExists {
            get {
                return ResourceManager.GetString("MenuForThisDateExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Меню нельзя редактировать....
        /// </summary>
        public static string MenuIsLocked {
            get {
                return ResourceManager.GetString("MenuIsLocked", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Меню не найдено MenuId = {0}.
        /// </summary>
        public static string MenuNotFound {
            get {
                return ResourceManager.GetString("MenuNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Заказ уже отправлен.
        /// </summary>
        public static string OrderHasBeenSent {
            get {
                return ResourceManager.GetString("OrderHasBeenSent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Views/OrderTemplate.cshtml.
        /// </summary>
        public static string PathToOrderEmailTemplate {
            get {
                return ResourceManager.GetString("PathToOrderEmailTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to dd.MM.yyyy.
        /// </summary>
        public static string RusDateFormat {
            get {
                return ResourceManager.GetString("RusDateFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Вы не ввели имя.
        /// </summary>
        public static string UserNameIsEmpty {
            get {
                return ResourceManager.GetString("UserNameIsEmpty", resourceCulture);
            }
        }
    }
}
