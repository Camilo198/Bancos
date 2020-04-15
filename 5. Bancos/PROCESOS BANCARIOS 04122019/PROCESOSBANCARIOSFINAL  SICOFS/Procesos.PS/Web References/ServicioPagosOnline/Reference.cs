﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// Microsoft.VSDesigner generó automáticamente este código fuente, versión=4.0.30319.42000.
// 
#pragma warning disable 1591

namespace Procesos.PS.ServicioPagosOnline {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="BasicHttpBinding_IPagosOnline", Namespace="http://tempuri.org/")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(object[]))]
    public partial class PagosOnline : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback ServicioPagosOnlineOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public PagosOnline() {
            this.Url = global::Procesos.PS.Properties.Settings.Default.Procesos_PS_ServicioPagosOnline_PagosOnline;
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
        public event ServicioPagosOnlineCompletedEventHandler ServicioPagosOnlineCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/IPagosOnline/ServicioPagosOnline", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string ServicioPagosOnline([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string NombreBanco, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string IdCuentaBancoEpicor, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string ArchivoSalidaPagosOnline, [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)] [System.Xml.Serialization.XmlArrayItemAttribute(Namespace="http://schemas.microsoft.com/2003/10/Serialization/Arrays")] object[] CorreosControl, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string CodigoTransito, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string NumCuenta, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string TipoCuenta, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string Remitente, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string Usuario, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string TipoProceso) {
            object[] results = this.Invoke("ServicioPagosOnline", new object[] {
                        NombreBanco,
                        IdCuentaBancoEpicor,
                        ArchivoSalidaPagosOnline,
                        CorreosControl,
                        CodigoTransito,
                        NumCuenta,
                        TipoCuenta,
                        Remitente,
                        Usuario,
                        TipoProceso});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ServicioPagosOnlineAsync(string NombreBanco, string IdCuentaBancoEpicor, string ArchivoSalidaPagosOnline, object[] CorreosControl, string CodigoTransito, string NumCuenta, string TipoCuenta, string Remitente, string Usuario, string TipoProceso) {
            this.ServicioPagosOnlineAsync(NombreBanco, IdCuentaBancoEpicor, ArchivoSalidaPagosOnline, CorreosControl, CodigoTransito, NumCuenta, TipoCuenta, Remitente, Usuario, TipoProceso, null);
        }
        
        /// <remarks/>
        public void ServicioPagosOnlineAsync(string NombreBanco, string IdCuentaBancoEpicor, string ArchivoSalidaPagosOnline, object[] CorreosControl, string CodigoTransito, string NumCuenta, string TipoCuenta, string Remitente, string Usuario, string TipoProceso, object userState) {
            if ((this.ServicioPagosOnlineOperationCompleted == null)) {
                this.ServicioPagosOnlineOperationCompleted = new System.Threading.SendOrPostCallback(this.OnServicioPagosOnlineOperationCompleted);
            }
            this.InvokeAsync("ServicioPagosOnline", new object[] {
                        NombreBanco,
                        IdCuentaBancoEpicor,
                        ArchivoSalidaPagosOnline,
                        CorreosControl,
                        CodigoTransito,
                        NumCuenta,
                        TipoCuenta,
                        Remitente,
                        Usuario,
                        TipoProceso}, this.ServicioPagosOnlineOperationCompleted, userState);
        }
        
        private void OnServicioPagosOnlineOperationCompleted(object arg) {
            if ((this.ServicioPagosOnlineCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ServicioPagosOnlineCompleted(this, new ServicioPagosOnlineCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    public delegate void ServicioPagosOnlineCompletedEventHandler(object sender, ServicioPagosOnlineCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ServicioPagosOnlineCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ServicioPagosOnlineCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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