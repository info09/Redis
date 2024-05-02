using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace NewIDC.Projects
{
    [XmlInclude(typeof(MoneyConversion))]
    [Serializable]
    public abstract class ConversionBase : IConversion
	{
        public int Order {  get; set; }
		protected IConversion preConversion;
        protected string conversionResultPath;
        protected List<string[]> convertResult;
		protected string[] HeaderString;
		public abstract List<string[]> Convert();
		public void SetPreviousConversion(IConversion conversion) {
			this.preConversion = conversion;
		}

        public virtual string[] GetHeader() {
            return HeaderString;
        }
    }

}

