﻿#region BSD License
/*
 * 
 * Original BSD 3-Clause License (https://github.com/ComponentFactory/Krypton/blob/master/LICENSE)
 *  © Component Factory Pty Ltd, 2006 - 2016, (Version 4.5.0.0) All rights reserved.
 * 
 *  New BSD 3-Clause License (https://github.com/Krypton-Suite/Standard-Toolkit/blob/master/LICENSE)
 *  Modifications by Peter Wagner(aka Wagnerp) & Simon Coghlan(aka Smurf-IV), et al. 2017 - 2022. All rights reserved. 
 *
 */
#endregion


namespace Krypton.Toolkit
{
    internal class KryptonCheckedListBoxActionList : DesignerActionList
    {
        #region Instance Fields
        private readonly KryptonCheckedListBox _checkedListBox;
        private readonly IComponentChangeService _service;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the KryptonCheckedListBoxActionList class.
        /// </summary>
        /// <param name="owner">Designer that owns this action list instance.</param>
        public KryptonCheckedListBoxActionList(KryptonCheckedListBoxDesigner owner)
            : base(owner.Component)
        {
            // Remember the list box instance
            _checkedListBox = owner.Component as KryptonCheckedListBox;

            // Cache service used to notify when a property has changed
            _service = (IComponentChangeService)GetService(typeof(IComponentChangeService));
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets and sets the style used for list items.
        /// </summary>
        public ButtonStyle ItemStyle
        {
            get => _checkedListBox.ItemStyle;

            set
            {
                if (_checkedListBox.ItemStyle != value)
                {
                    _service.OnComponentChanged(_checkedListBox, null, _checkedListBox.ItemStyle, value);
                    _checkedListBox.ItemStyle = value;
                }
            }
        }

        /// <summary>
        /// Gets and sets the background drawing style.
        /// </summary>
        public PaletteBackStyle BackStyle
        {
            get => _checkedListBox.BackStyle;

            set
            {
                if (_checkedListBox.BackStyle != value)
                {
                    _service.OnComponentChanged(_checkedListBox, null, _checkedListBox.BackStyle, value);
                    _checkedListBox.BackStyle = value;
                }
            }
        }

        /// <summary>
        /// Gets and sets the border drawing style.
        /// </summary>
        public PaletteBorderStyle BorderStyle
        {
            get => _checkedListBox.BorderStyle;

            set
            {
                if (_checkedListBox.BorderStyle != value)
                {
                    _service.OnComponentChanged(_checkedListBox, null, _checkedListBox.BorderStyle, value);
                    _checkedListBox.BorderStyle = value;
                }
            }
        }

        /// <summary>Gets or sets the Krypton Context Menu.</summary>
        /// <value>The Krypton Context Menu.</value>
        public KryptonContextMenu KryptonContextMenu
        {
            get => _checkedListBox.KryptonContextMenu;

            set
            {
                if (_checkedListBox.KryptonContextMenu != value)
                {
                    _service.OnComponentChanged(_checkedListBox, null, _checkedListBox.KryptonContextMenu, value);

                    _checkedListBox.KryptonContextMenu = value;
                }
            }
        }

        /// <summary>
        /// Gets and sets the selection mode.
        /// </summary>
        public CheckedSelectionMode SelectionMode
        {
            get => _checkedListBox.SelectionMode;

            set
            {
                if (_checkedListBox.SelectionMode != value)
                {
                    _service.OnComponentChanged(_checkedListBox, null, _checkedListBox.SelectionMode, value);
                    _checkedListBox.SelectionMode = value;
                }
            }
        }

        /// <summary>
        /// Gets and sets the selection mode.
        /// </summary>
        public bool Sorted
        {
            get => _checkedListBox.Sorted;

            set
            {
                if (_checkedListBox.Sorted != value)
                {
                    _service.OnComponentChanged(_checkedListBox, null, _checkedListBox.Sorted, value);
                    _checkedListBox.Sorted = value;
                }
            }
        }

        /// <summary>
        /// Gets and sets the check on click setting.
        /// </summary>
        public bool CheckOnClick
        {
            get => _checkedListBox.CheckOnClick;

            set
            {
                if (_checkedListBox.CheckOnClick != value)
                {
                    _service.OnComponentChanged(_checkedListBox, null, _checkedListBox.CheckOnClick, value);
                    _checkedListBox.CheckOnClick = value;
                }
            }
        }

        /// <summary>
        /// Gets and sets the palette mode.
        /// </summary>
        public PaletteMode PaletteMode
        {
            get => _checkedListBox.PaletteMode;

            set
            {
                if (_checkedListBox.PaletteMode != value)
                {
                    _service.OnComponentChanged(_checkedListBox, null, _checkedListBox.PaletteMode, value);
                    _checkedListBox.PaletteMode = value;
                }
            }
        }

        /// <summary>Gets or sets the font.</summary>
        /// <value>The font.</value>
        public Font StateCommonShortTextFont
        {
            get => _checkedListBox.StateCommon.Item.Content.ShortText.Font;

            set
            {
                if (_checkedListBox.StateCommon.Item.Content.ShortText.Font != value)
                {
                    _service.OnComponentChanged(_checkedListBox, null, _checkedListBox.StateCommon.Item.Content.ShortText.Font, value);

                    _checkedListBox.StateCommon.Item.Content.ShortText.Font = value;
                }
            }
        }

        /// <summary>Gets or sets the font.</summary>
        /// <value>The font.</value>
        public Font StateCommonLongTextFont
        {
            get => _checkedListBox.StateCommon.Item.Content.LongText.Font;

            set
            {
                if (_checkedListBox.StateCommon.Item.Content.LongText.Font != value)
                {
                    _service.OnComponentChanged(_checkedListBox, null, _checkedListBox.StateCommon.Item.Content.LongText.Font, value);

                    _checkedListBox.StateCommon.Item.Content.LongText.Font = value;
                }
            }
        }

        /// <summary>Gets or sets the corner radius.</summary>
        /// <value>The corner radius.</value>
        [DefaultValue(GlobalStaticValues.PRIMARY_CORNER_ROUNDING_VALUE)]
        public float StateCommonCornerRoundingRadius
        {
            get => _checkedListBox.StateCommon.Border.Rounding;

            set
            {
                if (_checkedListBox.StateCommon.Border.Rounding != value)
                {
                    _service.OnComponentChanged(_checkedListBox, null, _checkedListBox.StateCommon.Border.Rounding, value);

                    _checkedListBox.StateCommon.Border.Rounding = value;
                }
            }
        }
        #endregion

        #region Public Override
        /// <summary>
        /// Returns the collection of DesignerActionItem objects contained in the list.
        /// </summary>
        /// <returns>A DesignerActionItem array that contains the items in this list.</returns>
        public override DesignerActionItemCollection GetSortedActionItems()
        {
            // Create a new collection for holding the single item we want to create
            DesignerActionItemCollection actions = new();

            // This can be null when deleting a control instance at design time
            if (_checkedListBox != null)
            {
                // Add the list of list box specific actions
                actions.Add(new DesignerActionHeaderItem(@"Appearance"));
                actions.Add(new DesignerActionPropertyItem(@"BackStyle", @"Back Style", @"Appearance", @"Style used to draw background."));
                actions.Add(new DesignerActionPropertyItem(@"BorderStyle", @"Border Style", @"Appearance", @"Style used to draw the border."));
                actions.Add(new DesignerActionPropertyItem(@"KryptonContextMenu", @"Krypton Context Menu", @"Appearance", @"The Krypton Context Menu for the control."));
                actions.Add(new DesignerActionPropertyItem(@"ItemStyle", @"Item Style", @"Appearance", @"How to display list items."));
                actions.Add(new DesignerActionPropertyItem(@"StateCommonShortTextFont", @"State Common Short Text Font", @"Appearance", @"The State Common Short Text Font."));
                actions.Add(new DesignerActionPropertyItem(@"StateCommonLongTextFont", @"State Common State Common Long Text Font", @"Appearance", @"The State Common State Common Long Text Font."));
                actions.Add(new DesignerActionPropertyItem(@"StateCommonCornerRoundingRadius", @"State Common Corner Rounding Radius", @"Appearance", @"The corner rounding radius of the control."));
                actions.Add(new DesignerActionHeaderItem(@"Behavior"));
                actions.Add(new DesignerActionPropertyItem(@"SelectionMode", @"Selection Mode", @"Behavior", @"Determines the selection mode."));
                actions.Add(new DesignerActionPropertyItem(@"Sorted", @"Sorted", @"Behavior", @"Should items be sorted according to string."));
                actions.Add(new DesignerActionPropertyItem(@"CheckOnClick", @"CheckOnClick", @"Behavior", @"Should clicking an item toggle its checked state."));
                actions.Add(new DesignerActionHeaderItem(@"Visuals"));
                actions.Add(new DesignerActionPropertyItem(@"PaletteMode", @"Palette", @"Visuals", @"Palette applied to drawing"));
            }

            return actions;
        }
        #endregion
    }
}
