using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms.Design;

namespace NBarCodes.Forms {

	/// <summary>
	/// Designer class for the <see cref="BarCodeControl"/> class.
	/// </summary>
	class BarCodeControlDesigner : ControlDesigner {

		/// <summary>
		/// Remove some basic properties that are not supported by the 
		/// <see cref="BarCodeControl"/>.
		/// </summary>
		/// <param name="properties">Collection of the control's properties.</param>
		protected override void PostFilterProperties(IDictionary properties) {
			properties.Remove("BackgroundImage"); // not applicable
			properties.Remove("ForeColor");				// "BarColor" and "FontColor" used instead
			properties.Remove("Text");						// "Data" used instead
			properties.Remove("RightToLeft");			// not applicable
		}

		/// <summary>
		/// Defines the selection rules for the <see cref="BarCodeControl"/>.
		/// In particular, does not allow any resizing to occur.
		/// </summary>
		public override SelectionRules SelectionRules {
			get {
				return 
					SelectionRules.Visible |
					SelectionRules.Moveable;
			}
		}
	}
}
