﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SoundCatcher.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool SettingIsDetectingEvents {
            get {
                return ((bool)(this["SettingIsDetectingEvents"]));
            }
            set {
                this["SettingIsDetectingEvents"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool SettingIsSaving {
            get {
                return ((bool)(this["SettingIsSaving"]));
            }
            set {
                this["SettingIsSaving"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string SettingOutputPath {
            get {
                return ((string)(this["SettingOutputPath"]));
            }
            set {
                this["SettingOutputPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("3")]
        public int SettingSecondsToSave {
            get {
                return ((int)(this["SettingSecondsToSave"]));
            }
            set {
                this["SettingSecondsToSave"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("16384")]
        public int SettingAmplitudeThreshold {
            get {
                return ((int)(this["SettingAmplitudeThreshold"]));
            }
            set {
                this["SettingAmplitudeThreshold"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("44100")]
        public int SettingSamplesPerSecond {
            get {
                return ((int)(this["SettingSamplesPerSecond"]));
            }
            set {
                this["SettingSamplesPerSecond"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("4096")]
        public int SettingBytesPerFrame {
            get {
                return ((int)(this["SettingBytesPerFrame"]));
            }
            set {
                this["SettingBytesPerFrame"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("16")]
        public byte SettingBitsPerSample {
            get {
                return ((byte)(this["SettingBitsPerSample"]));
            }
            set {
                this["SettingBitsPerSample"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public byte SettingChannels {
            get {
                return ((byte)(this["SettingChannels"]));
            }
            set {
                this["SettingChannels"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("-1")]
        public int SettingAudioInputDevice {
            get {
                return ((int)(this["SettingAudioInputDevice"]));
            }
            set {
                this["SettingAudioInputDevice"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("-1")]
        public int SettingAudioOutputDevice {
            get {
                return ((int)(this["SettingAudioOutputDevice"]));
            }
            set {
                this["SettingAudioOutputDevice"] = value;
            }
        }
    }
}
