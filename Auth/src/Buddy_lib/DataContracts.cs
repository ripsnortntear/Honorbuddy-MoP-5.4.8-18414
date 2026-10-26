using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Buddy_lib
{
	[ServiceContract(ConfigurationName = "SR.IA")]
	public interface IA
	{
		[ServiceKnownType(typeof(d0))]
		[ServiceKnownType(typeof(r0))]
		[ServiceKnownType(typeof(object[]))]
		[OperationContract(Action = "http://tempuri.org/IA/Do", ReplyAction = "http://tempuri.org/IA/DoResponse")]
		d0 Do(byte b, object[] args);
	}

	[Serializable]
	[KnownType(typeof(d0))]
	[DataContract(Name = "r0", Namespace = "http://schemas.datacontract.org/2004/07/")]
	public class r0 : IExtensibleDataObject, INotifyPropertyChanged
	{
		[OptionalField]
		private string BodyField;

		[NonSerialized]
		private ExtensionDataObject extensionDataField;

		[OptionalField]
		private bool SuccessField;

		[DataMember]
		public string Body
		{
			get { return BodyField; }
			set
			{
				if (BodyField != value)
				{
					BodyField = value;
					RaisePropertyChanged("Body");
				}
			}
		}

		[Browsable(false)]
		public ExtensionDataObject ExtensionData
		{
			get { return extensionDataField; }
			set { extensionDataField = value; }
		}

		[DataMember]
		public bool Success
		{
			get { return SuccessField; }
			set
			{
				if (SuccessField != value)
				{
					SuccessField = value;
					RaisePropertyChanged("Success");
				}
			}
		}

		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}

	[Serializable]
	[DataContract(Name = "d0", Namespace = "http://schemas.datacontract.org/2004/07/")]
	public class d0 : r0
	{
		[OptionalField]
		private byte[] DataField;

		[DataMember]
		public byte[] Data
		{
			get { return DataField; }
			set
			{
				if (DataField != value)
				{
					DataField = value;
					RaisePropertyChanged("Data");
				}
			}
		}
	}
}
