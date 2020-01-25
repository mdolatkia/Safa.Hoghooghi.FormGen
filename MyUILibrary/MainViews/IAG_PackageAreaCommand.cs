using System;
using System.Collections.Generic;
namespace DataAgent.AgentCore {
	public interface IAG_PackageAreaCommand {


        List<DataManager.DataPackage.DataPackageUISetting.Enum_DPUI_TypeIntracionMode> CompatibaleIntractionMode { get; }
        List<DataManager.DataPackage.DataPackageUISetting.Enum_DPUI_TypeDataMode> CompatibaleIntractionDataMode { get; }
		string Name {
			get;
		}


		string Title {
			get;
			set;
		}


		byte[] Image {
			get;
			set;
		}

		void Execute(IAG_EditPackageArea packageArea);
		void Execute(IAG_SearchViewPackageArea packageArea);

	}

}
