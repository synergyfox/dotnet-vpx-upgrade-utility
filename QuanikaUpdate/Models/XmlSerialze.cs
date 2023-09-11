namespace QuanikaUpdate.Models
{
    public class XmlSerialze
    {



        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class Updates
        {

            private UpdatesFiles filesField;

            private string versionField;

            /// <remarks/>
            public UpdatesFiles Files
            {
                get
                {
                    return this.filesField;
                }
                set
                {
                    this.filesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string version
            {
                get
                {
                    return this.versionField;
                }
                set
                {
                    this.versionField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class UpdatesFiles
        {

            private UpdatesFilesAction[] databaseField;

            private UpdatesFilesAction1[] dLLField;

            private UpdatesFilesAction2[] eXEField;

            private UpdatesFilesAction3[] anyFileField;

            private UpdatesFilesAction4[] configKeysField;

            private string[] logField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("action", IsNullable = false)]
            public UpdatesFilesAction[] Database
            {
                get
                {
                    return this.databaseField;
                }
                set
                {
                    this.databaseField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("action", IsNullable = false)]
            public UpdatesFilesAction1[] DLL
            {
                get
                {
                    return this.dLLField;
                }
                set
                {
                    this.dLLField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("action", IsNullable = false)]
            public UpdatesFilesAction2[] EXE
            {
                get
                {
                    return this.eXEField;
                }
                set
                {
                    this.eXEField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("action", IsNullable = false)]
            public UpdatesFilesAction3[] AnyFile
            {
                get
                {
                    return this.anyFileField;
                }
                set
                {
                    this.anyFileField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("action", IsNullable = false)]
            public UpdatesFilesAction4[] Keys
            {
                get
                {
                    return this.configKeysField;
                }
                set
                {
                    this.configKeysField = value;
                }
            }


            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("action", IsNullable = false)]
            public string[] Log
            {
                get
                {
                    return this.logField;
                }
                set
                {
                    this.logField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class UpdatesFilesAction
        {

            private string nameField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class UpdatesFilesAction1
        {

            private string moduleField;

            private string folderField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string module
            {
                get
                {
                    return this.moduleField;
                }
                set
                {
                    this.moduleField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string folder
            {
                get
                {
                    return this.folderField;
                }
                set
                {
                    this.folderField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class UpdatesFilesAction2
        {

            private string moduleField;

            private string folderField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string module
            {
                get
                {
                    return this.moduleField;
                }
                set
                {
                    this.moduleField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string folder
            {
                get
                {
                    return this.folderField;
                }
                set
                {
                    this.folderField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class UpdatesFilesAction3
        {

            private string moduleField;

            private string folderField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string module
            {
                get
                {
                    return this.moduleField;
                }
                set
                {
                    this.moduleField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string folder
            {
                get
                {
                    return this.folderField;
                }
                set
                {
                    this.folderField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        public partial class UpdatesFilesAction4
        {

            private string moduleField;

            private string keyField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string module
            {
                get
                {
                    return this.moduleField;
                }
                set
                {
                    this.moduleField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string key
            {
                get
                {
                    return this.keyField;
                }
                set
                {
                    this.keyField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }



    }
}



