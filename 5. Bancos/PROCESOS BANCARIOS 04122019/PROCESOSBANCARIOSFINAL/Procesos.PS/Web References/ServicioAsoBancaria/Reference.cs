﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.34209
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// Microsoft.VSDesigner generó automáticamente este código fuente, versión=4.0.30319.34209.
// 
#pragma warning disable 1591

namespace Procesos.PS.ServicioAsoBancaria {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="BasicHttpBinding_IAsoBancaria", Namespace="http://tempuri.org/")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(object[]))]
    public partial class AsoBancaria : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback ServicioAsoOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public AsoBancaria() {
            this.Url = global::Procesos.PS.Properties.Settings.Default.Procesos_PS_ServicioAsoBancaria_AsoBancaria;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event ServicioAsoCompletedEventHandler ServicioAsoCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IAsoBancaria/ServicioAso", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string ServicioAso([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string NombreBanco, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string IdCuentaBancoEpicor, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string ArchivoSalidaAsobancaria, bool EsFTP, [System.Xml.Serialization.XmlIgnoreAttribute()] bool EsFTPSpecified, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string UrlFTP, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string UsuarioFTP, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string ClaveFTP, [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)] [System.Xml.Serialization.XmlArrayItemAttribute(Namespace="http://schemas.microsoft.com/2003/10/Serialization/Arrays")] object[] CorreosControl, [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)] [System.Xml.Serialization.XmlArrayItemAttribute(Namespace="http://schemas.microsoft.com/2003/10/Serialization/Arrays")] object[] CorreosEnvio, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string CodigoTransito, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string Remitente, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string Usuario, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string TipoProceso) {
            object[] results = this.Invoke("ServicioAso", new object[] {
                        NombreBanco,
                        IdCuentaBancoEpicor,
                        ArchivoSalidaAsobancaria,
                        EsFTP,
                        EsFTPSpecified,
                        UrlFTP,
                        UsuarioFTP,
                        ClaveFTP,
                        CorreosControl,
                        CorreosEnvio,
                        CodigoTransito,
                        Remitente,
                        Usuario,
                        TipoProceso});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ServicioAsoAsync(string NombreBanco, string IdCuentaBancoEpicor, string ArchivoSalidaAsobancaria, bool EsFTP, bool EsFTPSpecified, string UrlFTP, string UsuarioFTP, string ClaveFTP, object[] CorreosControl, object[] CorreosEnvio, string CodigoTransito, string Remitente, string Usuario, string TipoProceso) {
            this.ServicioAsoAsync(NombreBanco, IdCuentaBancoEpicor, ArchivoSalidaAsobancaria, EsFTP, EsFTPSpecified, UrlFTP, UsuarioFTP, ClaveFTP, CorreosControl, CorreosEnvio, CodigoTransito, Remitente, Usuario, TipoProceso, null);
        }
        
        /// <remarks/>
        public void ServicioAsoAsync(string NombreBanco, string IdCuentaBancoEpicor, string ArchivoSalidaAsobancaria, bool EsFTP, bool EsFTPSpecified, string UrlFTP, string UsuarioFTP, string ClaveFTP, object[] CorreosControl, object[] CorreosEnvio, string CodigoTransito, string Remitente, string Usuario, string TipoProceso, object userState) {
            if ((this.ServicioAsoOperationCompleted == null)) {
                this.ServicioAsoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnServicioAsoOperationCompleted);
            }
            this.InvokeAsync("ServicioAso", new object[] {
                        NombreBanco,
                        IdCuentaBancoEpicor,
                        ArchivoSalidaAsobancaria,
                        EsFTP,
                        EsFTPSpecified,
                        UrlFTP,
                        UsuarioFTP,
                        ClaveFTP,
                        CorreosControl,
                        CorreosEnvio,
                        CodigoTransito,
                        Remitente,
                        Usuario,
                        TipoProceso}, this.ServicioAsoOperationCompleted, userState);
        }
        
        private void OnServicioAsoOperationCompleted(object arg) {
            if ((this.ServicioAsoCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ServicioAsoCompleted(this, new ServicioAsoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    public delegate void ServicioAsoCompletedEventHandler(object sender, ServicioAsoCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ServicioAsoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ServicioAsoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591