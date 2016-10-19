using System;
using Eto.Forms;
using Eto.Drawing;

namespace GridTest
{
	internal class PropertyPOCO
	{
		#region --- Fields ---

		private string mName;
		private string mTypeString;
		private bool mChecked;
		private bool mEnabled;

		private readonly bool mGroup;
		private readonly string mGroupName;

		#endregion


		#region --- Properties ---

		public bool PropertyCheck {
			get {
				return mChecked;
			}
			set {
				mChecked = value;
			}
		}

		public bool Group {
			get {
				return mGroup;
			}
		}

		public String PropertyName {
			get {
				return mGroup ? mGroupName : mName;
			}
		}

		public String PropertyType {
			get {
				return mGroup ? "" : mTypeString;
			}
		}

		public bool Enabled {
			get {
				return mEnabled;
			}

			set {
				mEnabled = value;
			}
		}

		#endregion


		#region  --- Ctor \ Dtor ---

		public PropertyPOCO (string name, string typeString, string grName, bool group, bool check, bool enabled)
		{
			mName = name;
			mTypeString = typeString;
			mChecked = check;
			mGroupName = grName;
			mGroup = group;
			mEnabled = enabled;
		}

		#endregion
	}

	internal class PropertyCustomCheckCell : CustomCell
	{
		public CheckBox checkBox = null;
		protected override Control OnCreateCell (CellEventArgs args)
		{
			var item = (PropertyPOCO)args.Item;
			checkBox = new CheckBox ();
			checkBox.CheckedBinding.BindDataContext ((PropertyPOCO m) => m.PropertyCheck);
			return checkBox;
		}

		protected override void OnConfigureCell (CellEventArgs args, Control control)
		{
			if (checkBox != null) {
				var item = (PropertyPOCO)args.Item;
				checkBox.Enabled = item.Enabled;
			}
			base.OnConfigureCell (args, control);
		}
	}

	/// <summary>
	/// Your application's main form
	/// </summary>
	public class MainForm : Form
	{
		private GridColumn mCheck;
		private GridColumn mName;
		private GridColumn mType;
		private GridView mPropertyGridView;

		public MainForm ()
		{
			InitializeComponent ();

			PopulatePropertyView (true);
		}

		void PopulatePropertyView (bool v)
		{
			var coll = new SelectableFilterCollection<PropertyPOCO> (mPropertyGridView);

			for (int i = 0; i < 27; i++) {
				coll.Add (new PropertyPOCO ("Name " + i.ToString (), "Integer", i % 4 == 0 ? "Group " + i.ToString () : "", i % 4 == 0 ? true : false, i % 2 == 0 ? true : false, true));
			}

			mCheck.DataCell = new PropertyCustomCheckCell ();
			mName.DataCell = new TextBoxCell {
				Binding = Binding.Property<PropertyPOCO, string> (r => r.PropertyName)
			};
			mType.DataCell = new TextBoxCell {
				Binding = Binding.Property<PropertyPOCO, string> (r => r.PropertyType)
			};
			mPropertyGridView.DataStore = coll;
		}

		private void InitializeComponent ()
		{
			this.mPropertyGridView = new GridView ();
			this.mCheck = new GridColumn () { DataCell = new PropertyCustomCheckCell (), AutoSize = false };
			this.mName = new GridColumn () { DataCell = new TextBoxCell ("Name"), AutoSize = true };
			this.mType = new GridColumn () { DataCell = new TextBoxCell ("Type"), AutoSize = true };

			// 
			// PropertyListView
			// 
			this.mPropertyGridView.ID = "PropertyGridView";
			this.mPropertyGridView.AllowMultipleSelection = false;
			this.mPropertyGridView.ShowHeader = true;
			this.mPropertyGridView.Size = new Eto.Drawing.Size (305, 336);
			this.mPropertyGridView.Columns.Add (mCheck);
			this.mPropertyGridView.Columns.Add (mName);
			this.mPropertyGridView.Columns.Add (mType);
			this.mPropertyGridView.CellFormatting += PropertyGridView_CellFormatting;
			// 
			// Check
			// 
			this.mCheck.HeaderText = "";
			this.mCheck.Width = 20;
			this.mCheck.Editable = true;
			// 
			// Name
			// 
			this.mName.HeaderText = "Name";
			this.mName.Width = 200;
			// 
			// Type
			// 
			this.mType.HeaderText = "Type";
			this.mType.Width = 80;


			TableLayout tableLayout = new TableLayout (1, 1) { BackgroundColor = Colors.White, Spacing = new Size (5, 5), Padding = new Padding (5) };

			tableLayout.Add (this.mPropertyGridView, 0, 0);
			Content = tableLayout;

			Title = "My Eto Form";
			ClientSize = new Size (350, 600);
		}

		void PropertyGridView_CellFormatting (object sender, GridCellFormatEventArgs e)
		{
			PropertyPOCO poco = e.Item as PropertyPOCO;
			if (poco != null) {
				if (poco.Group) {
					if (e.Font != null) {
						e.Font = new Font (e.Font.FamilyName, e.Font.Size, FontStyle.Bold);
					}
					e.BackgroundColor = Colors.LightGrey;
					if (e.Column.DataCell.GetType () == typeof (PropertyCustomCheckCell)) {
						PropertyCustomCheckCell cell = e.Column.DataCell as PropertyCustomCheckCell;
						if (cell != null) {
							CheckBox checkBox = cell.checkBox as CheckBox;
							if (checkBox != null)
								checkBox.Visible = false;
						}
					}
				} else {
					if (e.Font != null) {
						e.Font = new Font (e.Font.FamilyName, e.Font.Size, FontStyle.None);
					}
					if (e.Column.DataCell.GetType () == typeof (PropertyCustomCheckCell)) {
						PropertyCustomCheckCell cell = e.Column.DataCell as PropertyCustomCheckCell;
						if (cell != null) {
							CheckBox checkBox = cell.checkBox as CheckBox;
							if (checkBox != null)
								checkBox.Visible = true;
						}
					}
				}
			}
		}
}
}
