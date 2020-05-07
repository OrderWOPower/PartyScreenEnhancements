﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.GauntletUI;
using TaleWorlds.MountAndBlade.GauntletUI.Widgets;

namespace PartyScreenEnhancements.Widgets
{
    public class CategoryHeaderWidget : ToggleWidget
    {
		public CategoryHeaderWidget(UIContext context) : base(context)
		{
		}

		protected override void OnClick(Widget widget)
		{
			base.OnClick(widget);
			this.UpdateCollapseIndicator();
		}

		private void OnListSizeChange(Widget widget)
		{
			this.UpdateSize();
		}

		private void OnListSizeChange(Widget parentWidget, Widget addedWidget)
		{
			this.UpdateSize();
		}

		private void UpdateSize()
		{
			if (this.TransferButtonWidget != null)
			{
				this.TransferButtonWidget.IsEnabled = (this._listPanel.ChildCount > 0);
			}
			if (this.IsRelevant)
			{
				base.IsVisible = true;
				if (this._listPanel.ChildCount > 0)
				{
					this._listPanel.IsVisible = true;
				}
				base.IsEnabled = (this._listPanel.ChildCount > 0);
				if (this._listPanel.ChildCount > this._latestChildCount && !base.WidgetToClose.IsVisible)
				{
					this.OnClick();
				}
			}
            this._latestChildCount = this._listPanel.ChildCount;
			this.UpdateCollapseIndicator();
		}

		private void ListPanelUpdated()
		{
			if (this.TransferButtonWidget != null)
			{
				this.TransferButtonWidget.IsEnabled = false;
			}
			this._listPanel.ItemAfterRemoveEventHandlers.Add(new Action<Widget>(this.OnListSizeChange));
			this._listPanel.ItemAddEventHandlers.Add(new Action<Widget, Widget>(this.OnListSizeChange));
			this.UpdateSize();
		}

		private void TransferButtonUpdated()
		{
			this.TransferButtonWidget.IsEnabled = false;
		}

		private void CollapseIndicatorUpdated()
		{
			this.CollapseIndicator.AddState("Collapsed");
			this.CollapseIndicator.AddState("Expanded");
			this.UpdateCollapseIndicator();
		}

		private void UpdateCollapseIndicator()
		{
			if (base.WidgetToClose != null && this.CollapseIndicator != null)
			{
				if (base.WidgetToClose.IsVisible)
				{
					this.CollapseIndicator.SetState("Expanded");
					return;
				}
				this.CollapseIndicator.SetState("Collapsed");
			}
		}

		[Editor(false)]
		public ListPanel ListPanel
		{
			get
			{
				return this._listPanel;
			}
			set
			{
				if (this._listPanel != value)
				{
					this._listPanel = value;
					base.OnPropertyChanged(value, "ListPanel");
					this.ListPanelUpdated();
				}
			}
		}

		[Editor(false)]
		public ButtonWidget TransferButtonWidget
		{
			get
			{
				return this._transferButtonWidget;
			}
			set
			{
				if (this._transferButtonWidget != value)
				{
					this._transferButtonWidget = value;
					base.OnPropertyChanged(value, "TransferButtonWidget");
					this.TransferButtonUpdated();
				}
			}
		}

		[Editor(false)]
		public Widget CollapseIndicator
		{
			get
			{
				return this._collapseIndicator;
			}
			set
			{
				if (this._collapseIndicator != value)
				{
					this._collapseIndicator = value;
					base.OnPropertyChanged(value, "CollapseIndicator");
					this.CollapseIndicatorUpdated();
				}
			}
		}

		[Editor(false)]
		public bool IsRelevant
		{
			get
			{
				return this._isRelevant;
			}
			set
			{
				if (this._isRelevant != value)
				{
					this._isRelevant = value;
					base.OnPropertyChanged(value, "IsRelevant");
					if (!this._isRelevant)
					{
						base.IsVisible = false;
					}
				}
			}
		}

		private int _latestChildCount;

		private ListPanel _listPanel;

		private ButtonWidget _transferButtonWidget;

		private Widget _collapseIndicator;

		private bool _isRelevant = true;
	}
}