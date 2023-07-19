namespace CA_PnP_Core.Attributes {
    [System.AttributeUsage(System.AttributeTargets.Property)]
    internal class SPFieldAttribute : System.Attribute {
        public SPFieldAttribute(string internalName) {
            InternalName = internalName;
        }

        public string InternalName { get; set; }
    }
}
